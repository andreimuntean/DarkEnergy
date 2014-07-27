using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat
{
    // Character index from top to bottom:
    // Left: 1, 0, 2
    // Right: 1, 0, 2
    // Trust me, I'm an engineer.

    public class Units : GameSystem
    {
        private Battle battle;
        private const float leftDistance = 220;
        private const float rightDistance = 300;
        private const float topDistance = 450;
        private const float distanceBetweenLeftUnits = 115;
        private const float distanceBetweenRightUnits = 150;

        public event EventHandler PositionChanged;
        public event EventHandler CurrentChanged;
        public event EventHandler TargetChanged;

        public List<Vector2> StartingPositions
        {
            get
            {
                var left = leftDistance;
                var right = Screen.NativeResolution.X - rightDistance;
                var top = topDistance;
                return new List<Vector2>(6)
                {
                    new Vector2(left + 2 * distanceBetweenLeftUnits - (GroupA.Count > 0 ? GroupA[0].Width : 0), top + 100 - (GroupA.Count > 0 ? GroupA[0].Height : 0)),
                    new Vector2(left + distanceBetweenLeftUnits - (GroupA.Count > 1 ? GroupA[1].Width : 0), top - (GroupA.Count > 1 ? GroupA[1].Height : 0)),
                    new Vector2(left - (GroupA.Count > 2 ? GroupA[2].Width : 0), top + 150 - (GroupA.Count > 2 ? GroupA[2].Height : 0)),
                    new Vector2(right - 2 *distanceBetweenRightUnits, top + 100 - (GroupB.Count > 0 ? GroupB[0].Height : 0)),
                    new Vector2(right - distanceBetweenRightUnits, top - (GroupB.Count > 1 ? GroupB[1].Height : 0)),
                    new Vector2(right, top + 150 - (GroupB.Count > 2 ? GroupB[2].Height : 0))
                };
            }
        }

        public List<Character> GroupA { get; protected set; }
        public List<Character> GroupB { get; protected set; }
        public List<CombatAction> Actions { get; protected set; }
        public bool CanSelectUnits { get; set; }

        private Character current;
        public Character Current
        {
            get { return current; }
            set
            {
                if (current == null || current != value)
                {
                    current = value;
                    OnCurrentChanged(EventArgs.Empty);
                }
            }
        }

        private Character target;
        public Character Target
        {
            get { return target; }
            set
            {
                if (target == null || target != value)
                {
                    target = value;
                    OnTargetChanged(EventArgs.Empty);
                }
            }
        }

        public int CurrentIndex { get { return CurrentGroup.IndexOf(Current); } }

        public int TargetIndex { get { return TargetGroup.IndexOf(Target); } }

        public List<Character> CurrentGroup { get { return GroupOf(Current); } }

        public List<Character> TargetGroup { get { return GroupOf(Target); } }

        public List<Character> All
        {
            get
            {
                var allUnits = new List<Character>(GroupA);
                allUnits.AddRange(GroupB);
                return allUnits;
            }
        }

        public List<Character> Alive
        {
            get
            {
                return new List<Character>(All.Where(unit => unit.Alive));
            }
        }

        public Character FirstAlive(List<Character> group, Character defaultReturn = null)
        {
            foreach (var character in group)
            {
                if (character.Alive)
                {
                    return character;
                }
            }

            return defaultReturn;
        }

        public int CountAlive(List<Character> group)
        {
            int count = 0;
            foreach (var character in group)
            {
                if (character.Alive) ++count;
            }
            return count;
        }

        public Units(Battle battle, List<Character> groupA, List<Character> groupB)
        {
            Parent = this.battle = battle;
            GroupA = groupA;
            GroupB = groupB;

            GroupA.ForEach(character => character.PositionChanged += Units_PositionChanged);
            GroupB.ForEach(character => character.PositionChanged += Units_PositionChanged);

            Actions = new List<CombatAction>(6);
        }

        public List<Character> GroupOf(Character character)
        {
            if (character == null)
            {
                return null;
            }
            else
            {
                return GroupA.Contains(character) ? GroupA : GroupB;
            }
        }

        public void InitializeSelections()
        {
            Current = FirstAlive(GroupA, Current);
            Target = FirstAlive(GroupB, Target);
        }

        public void EnqueueAbility(Ability ability)
        {
            if (battle.State == BattleState.Waiting)
            {
                if (Current.Level < ability.RequiredLevel)
                {
                    PhoneEffectsManager.Play(PhoneEffect.Vibration);
                    return;
                }

                if (Current.DarkEnergy < ability.DarkEnergyCost)
                {
                    PhoneEffectsManager.Play(PhoneEffect.Vibration);
                    return;
                }

                // If an ability that targets the caster is used but an enemy
                // is targeted instead: target the caster.
                if (ability.TargetRestrictions.Self && !TargetGroup.Contains(Current))
                {
                    target = current;
                }
                else if (!ability.TargetRestrictions.Validate(Current, Target))
                {
                    PhoneEffectsManager.Play(PhoneEffect.Vibration);
                    return;
                }

                Actions.Add(new CombatAction(ability, Current, Target));

                if (CurrentIndex + 1 < CurrentGroup.Count)
                {
                    SelectNextCharacter();
                }
            }
        }

        public void ComputeAttackOrder()
        {
            Action<List<Character>> wrapUp = group =>
            {
                // Looks up the characters that did not choose an ability.
                group.ForEach(character =>
                {
                    if (character.Alive)
                    {
                        for (var i = 0; i < Actions.Count; ++i)
                        {
                            if (Actions[i].Current == character)
                            {
                                return;
                            }
                        }

                        // Generates an action for every such character.
                        if (character is Enemy) Actions.Add((character as Enemy).ComputeAction(this));
                    }
                });
            };

            wrapUp(GroupA);
            wrapUp(GroupB);
            Actions.OrderByDescending(action => action.Current.Total.CalculateSpeed());
        }

        public void SelectNextCharacter()
        {
            if (CurrentIndex + 1 < CurrentGroup.Count)
            {
                Character current = null;

                // Select next alive unit.
                for (int i = CurrentIndex + 1; i < CurrentGroup.Count; ++i)
                {
                    if (CurrentGroup[i].Alive)
                    {
                        current = CurrentGroup[i];
                        break;
                    }
                }

                if (current != null) Current = current;
                
                // Select first alive enemy.
                Target = FirstAlive(GroupB, Target);
            }
        }

        public void SelectPreviousCharacter()
        {
            if (CurrentIndex - 1 >= 0)
            {
                Character current = null;

                // Select previous alive unit.
                for (int i = CurrentIndex - 1; i >= 0; --i)
                {
                    if (CurrentGroup[i].Alive)
                    {
                        current = CurrentGroup[i];
                        break;
                    }
                }

                if (current != null) Current = current;

                // Select first alive enemy.
                Target = FirstAlive(GroupB, Target);
            }
        }

        public void ResetPositions()
        {
            var positions = StartingPositions;

            foreach (var unit in GroupA)
            {
                unit.Position = positions[GroupA.IndexOf(unit)];
            }

            foreach (var unit in GroupB)
            {
                unit.Position = positions[3 + GroupB.IndexOf(unit)];
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            GroupA.ForEach(character => { character.Initialize(); });
            GroupB.ForEach(character => { character.Initialize(); });

            ResetPositions();
            CanSelectUnits = true;

            InitializeSelections();

            Actions.Clear();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            GroupA.ForEach(character => character.LoadContent(contentManager));
            GroupB.ForEach(character => character.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            GroupA.ForEach(character =>
            {
                character.Update(gameTime);
                if (CanSelectUnits && GroupA.Count > 1 && Target != character)
                {
                    TouchManager.OnTap(character, () => Target = character);
                }
            });

            GroupB.ForEach(character =>
            {
                character.Update(gameTime);
                if (CanSelectUnits && (GroupA.Count > 1 || GroupB.Count > 1) && Target != character)
                {
                    TouchManager.OnTap(character, () => Target = character);
                }
            });
        }

        public override void Draw(Renderer renderer)
        {
            var drawOrder = new List<TexturedElement>(GroupA);
            var drawAbilityVisualBack = battle.AreaEffects.AbilityVisual != null;
            var drawAbilityVisualFront = drawAbilityVisualBack && battle.AreaEffects.AbilityVisual.DrawAboveUnits;
            
            drawOrder.AddRange(GroupB);

            if (drawAbilityVisualBack)
                drawOrder.AddRange(battle.AreaEffects.AbilityVisual.Components);

            drawOrder = drawOrder.OrderBy(component => component.Y + component.Height).ToList();

            if (drawAbilityVisualFront)
                drawOrder.AddRange(battle.AreaEffects.AbilityVisual.Components);

            drawOrder.ForEach(component => component.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            GroupA.ForEach(character => character.UnloadContent(contentManager));
            GroupB.ForEach(character => character.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void Units_PositionChanged(object sender, EventArgs e)
        {
            OnPositionChanged(EventArgs.Empty);
        }

        protected void OnPositionChanged(EventArgs e)
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, e);
            }
        }

        protected void OnCurrentChanged(EventArgs e)
        {
            if (CurrentChanged != null)
            {
                CurrentChanged(this, e);
            }
        }

        protected void OnTargetChanged(EventArgs e)
        {
            if (TargetChanged != null)
            {
                TargetChanged(this, e);
            }
        }
        #endregion
    }
}
