using SharpDX;
using SharpDX.Toolkit;
using DarkEnergy.Abilities.Visual;

namespace DarkEnergy.Combat
{
    public class AbilityManager
    {
        private Battle battle;
        private Units units { get { return battle.Units; } }
        private AbilityVisual abilityVisual { get { return battle.AreaEffects.AbilityVisual; } }

        public bool IsCasting { get; private set; }
        public bool CastStarted { get; private set; }
        public bool CastCompleted { get; private set; }
        public bool CombatTextDisplayed { get; private set; }
        public bool EffectsApplied { get; private set; }

        public AbilityManager(Battle battle)
        {
            this.battle = battle;
        }

        public void Next()
        {
            IsCasting = false;
            CastStarted = false;
            CastCompleted = false;
            CombatTextDisplayed = false;
            EffectsApplied = false;
        }

        public void Tick()
        {
            foreach (var unit in units.Alive)
            {
                for (int i = 0; i < unit.ActiveEffects.Count; ++i)
                {
                    unit.ActiveEffects[i].Tick();
                    if (unit.ActiveEffects[i].Rounds <= 0)
                    {
                        unit.ActiveEffects.RemoveAt(i);
                    }
                }

                unit.DarkEnergy += (int)unit.Total.CalculateDarkEnergyGenerated();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (battle.AnimationManager.UnitAbilityAnimationStarted && !CastCompleted)
            {
                IsCasting = true;
            }

            if (IsCasting)
            {
                var action = units.Actions[0];

                if (!CastStarted)
                {
                    CastStarted = true;
                    battle.AreaEffects.AbilityVisual = AbilityVisual.FromAction(action);
                    abilityVisual.Initialize();
                    abilityVisual.LoadContent(App.Game.Content);
                }

                if (abilityVisual.AbilityUsed && !CombatTextDisplayed)
                {
                    EffectsApplied = true;

                    battle.AreaEffects.DisplayCombatText(GameManager.Combat.GetTurnData());
                    battle.TacticalMenu.UnitFrames.Refresh();

                    if (GameManager.Combat.AllAttacksMissed() == false)
                    {
                        PhoneEffectsManager.Play(PhoneEffect.SlightVibration);
                    }

                    CombatTextDisplayed = true;
                }

                if (abilityVisual.AnimationEnded && CombatTextDisplayed)
                {
                    abilityVisual.UnloadContent(App.Game.Content);
                    battle.AreaEffects.AbilityVisual = null;
                    CastCompleted = true;
                    IsCasting = false;
                }
            }
        }
    }
}
