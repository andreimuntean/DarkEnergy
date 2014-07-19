using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Combat;

namespace DarkEnergy.Abilities.Visual
{
    public class VenomSpit : AbilityVisual
    {
        protected float direction;
        private const float moveTime = 0.1f;
        protected TexturedElement effect;
        protected Vector2 initialDistance;
        protected Vector2 start;
        protected Vector2 end;

        public VenomSpit(CombatAction action) : base(action)
        {
            var current = action.Current;
            var target = action.Target;

            if (current.X < target.X)
            {
                direction = 1;
                start = new Vector2(current.X + current.Width + 10, current.Y + (current.Height - 52) / 2);
                end = new Vector2(target.X + 200, target.Y + (target.Height - 52) / 2);
            }
            else
            {
                direction = -1;
                start = new Vector2(current.X - 10, current.Y + (current.Height - 52) / 2);
                end = new Vector2(target.X + target.Width - 200, target.Y + (target.Height - 52) / 2);
            }

            initialDistance = (end - start) * direction;

            effect = new TexturedElement(350, 50, start) { Parent = this, Path = @"Ability\Effects\VenomSpit.dds" };
            Components.Add(effect);
        }

        public override void Update(GameTime gameTime)
        {
            if (!AnimationEnded)
            {
                var modifier = (float)(gameTime.ElapsedGameTime.TotalSeconds * direction / moveTime) * initialDistance;
                var stop = (direction > 0 && effect.X + modifier.X >= end.X) ? true : (direction < 0 && effect.X + modifier.X <= end.X) ? true : false;

                if (stop)
                {
                    effect.Position = end;
                    UseAbility();
                    AnimationEnded = true;
                }
                else
                {
                    effect.Position += modifier;
                }
            }
        }
    }
}
