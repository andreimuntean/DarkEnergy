using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Combat;

namespace DarkEnergy.Abilities.Visual
{
    public class NoVisual : AbilityVisual
    {
        public NoVisual(CombatAction action) : base(action)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!AnimationEnded)
            {
                UseAbility();
                AnimationEnded = true;
            }
        }
    }
}
