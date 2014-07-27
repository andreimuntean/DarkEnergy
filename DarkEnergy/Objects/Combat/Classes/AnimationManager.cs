using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat
{
    public class AnimationManager
    {
        private Battle battle;
        private ImpactAnimations impactAnimations;
        private Units units { get { return battle.Units; } }

        private bool initializedImpactAnimations;
        private double counter;
        private float direction;
        private Vector2 orientation;
        private Vector2 initialPosition;
        private Vector2 targetPosition;
        private const float attackDistance = 0;
        private const double chargeSpeed = 1500;
        private const double attackTime = 0.5;
        private const double castTime = 0.8;

        public bool UnitAbilityAnimationStarted { get; protected set; }
        public bool UnitAnimationEnded { get; protected set; }

        public AnimationManager(Battle battle)
        {
            this.battle = battle;
            impactAnimations = new ImpactAnimations(battle);
        }

        private void animateImpact(GameTime gameTime, CombatAction action)
        {
            if (!initializedImpactAnimations)
            {
                if (battle.AbilityManager.EffectsApplied)
                {
                    impactAnimations.Initialize(action);
                    initializedImpactAnimations = true;
                }
            }

            impactAnimations.Update(gameTime);
        }

        private void standAction(GameTime gameTime, CombatAction action)
        {
            var current = action.Current;
            var target = action.Target;

            initializedImpactAnimations = false;
            counter = 0;
            direction = units.GroupA.Contains(current) ? 1 : -1;
            initialPosition = current.Position;
            targetPosition = new Vector2(target.X + (direction > 0 ? -(current.Width + attackDistance) : (target.Width + attackDistance)), target.Y - (current.Height - target.Height));
            orientation = (targetPosition - action.Current.Position) * direction;
            orientation.Normalize();

            action.Current.State = action.Ability.Animation;

            switch (action.Current.State)
            {
                case CharacterState.CastSelf: UnitAbilityAnimationStarted = true; break;
                case CharacterState.CastTarget: UnitAbilityAnimationStarted = true; break;
            }
        }

        private void castAction(GameTime gameTime, CombatAction action)
        {
            var current = action.Current;
            animateImpact(gameTime, action);
            counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (counter >= castTime && battle.AbilityManager.CastCompleted && impactAnimations.Completed)
            {
                current.State = CharacterState.Stand;
                UnitAnimationEnded = true;
            }
        }

        private void chargeAction(GameTime gameTime, CombatAction action)
        {
            var current = action.Current;
            var modifier = (float)(gameTime.ElapsedGameTime.TotalSeconds * direction * chargeSpeed) * orientation;
            var stop = (direction > 0 && current.Position.X + modifier.X >= targetPosition.X) ? true : (direction < 0 && current.Position.X + modifier.X <= targetPosition.X) ? true : false;

            if (stop)
            {
                current.Position = targetPosition;
                current.State = CharacterState.Attack;
                UnitAbilityAnimationStarted = true;
            }
            else
            {
                current.Position += modifier;
            }
        }

        private void retreatAction(GameTime gameTime, CombatAction action)
        {
            var current = action.Current;
            var modifier = (float)(gameTime.ElapsedGameTime.TotalSeconds * -direction * chargeSpeed) * orientation;
            var stop = (-direction > 0 && current.Position.X + modifier.X >= initialPosition.X) ? true : (-direction < 0 && current.Position.X + modifier.X <= initialPosition.X) ? true : false;

            if (stop)
            {
                current.Position = initialPosition;
                current.State = CharacterState.Stand;
                UnitAnimationEnded = true;
            }
            else
            {
                current.Position += modifier;
            }
        }

        private void attackAction(GameTime gameTime, CombatAction action)
        {
            var current = action.Current;
            animateImpact(gameTime, action);
            counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (counter >= attackTime && battle.AbilityManager.CastCompleted && impactAnimations.Completed)
            {
                current.State = CharacterState.Retreat;
            }
        }

        public void Next()
        {
            UnitAbilityAnimationStarted = false;
            UnitAnimationEnded = false;
        }
        
        public void Update(GameTime gameTime)
        {
            if (battle.State == BattleState.Acting)
            {
                if (units.Actions.Count > 0 && !UnitAnimationEnded)
                {
                    var action = units.Actions[0];

                    units.Current = action.Current;
                    units.Target = action.Target;

                    switch (action.Current.State)
                    {
                        case CharacterState.Stand:
                            standAction(gameTime, action);
                            break;

                        case CharacterState.CastTarget:
                            castAction(gameTime, action);
                            break;

                        case CharacterState.CastSelf:
                            castAction(gameTime, action);
                            break;

                        case CharacterState.Charge:
                            chargeAction(gameTime, action);
                            break;

                        case CharacterState.Retreat:
                            retreatAction(gameTime, action);
                            break;

                        case CharacterState.Attack:
                            attackAction(gameTime, action);
                            break;
                    }
                }
            }
        }
    }
}
