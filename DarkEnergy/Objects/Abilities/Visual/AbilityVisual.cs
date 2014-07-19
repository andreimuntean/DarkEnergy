using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Combat;

namespace DarkEnergy.Abilities.Visual
{
    public class AbilityVisual : GameSystem
    {
        public static AbilityVisual FromAction(CombatAction action)
        {
            switch (action.Ability.VisualId)
            {
                case 10000: return new NoVisual(action);
                case 10002: return new Frenzy(action);
                case 10003: return new Infest(action);
                case 10004: return new VenomSpit(action);
                case 10005: return new Firebolt(action);
                default: return null;
            }
        }

        private CombatAction action;

        private bool animationEnded;
        public bool AnimationEnded { get { return animationEnded; } protected set { animationEnded = value; Visible = !animationEnded; } }
        public bool AbilityUsed { get; protected set; }

        public bool DrawAboveUnits { get; protected set; }

        public virtual List<TexturedElement> Components { get; protected set; }

        public AbilityVisual(CombatAction action)
        {
            this.action = action;
            AbilityUsed = false;
            AnimationEnded = false;
            Components = new List<TexturedElement>();
        }

        public void UseAbility()
        {
            action.Use();
            AbilityUsed = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            Components.ForEach(component => component.Initialize());
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Components.ForEach(component => component.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            Components.ForEach(component => component.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }
    }
}
