using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Combat;

namespace DarkEnergy.Abilities.Visual
{
    public sealed class Firebolt : AbilityVisual
    {
        private float direction;
        private const double moveTime = 0.4;
        private TexturedElement firebolt;
        private Vector2 initialDistance;
        private Vector2 start;
        private Vector2 end;

        public Firebolt(CombatAction action) : base(action)
        {
            var current = action.Current;
            var target = action.Target;

            if (current.X < target.X)
            {
                direction = 1 / (float)moveTime;
                start = new Vector2(current.X + current.Width, current.Y + (current.Height - 52) / 2);
                end = new Vector2(target.X - 155, target.Y + (target.Height - 52) / 2);
            }
            else
            {
                direction = -1 / (float)moveTime;
                start = new Vector2(current.X - 155, current.Y + (current.Height - 52) / 2);
                end = new Vector2(target.X + target.Width, target.Y + (target.Height - 52) / 2);
            }

            initialDistance = (start - end) / direction;

            firebolt = new TexturedElement(155, 52, start) { Parent = this, Path = @"Ability\Effects\Firebolt.dds" };
            Components.Add(firebolt);
        }

        public override void Update(GameTime gameTime)
        {
            if (!AnimationEnded)
            {
                var modifier = (float)(gameTime.ElapsedGameTime.TotalSeconds * direction) * initialDistance;
                var stop = (direction > 0 && start.X + modifier.X >= end.X) ? true : (direction < 0 && start.X + modifier.X <= end.X) ? true : false;

                if (stop)
                {
                    firebolt.Position = end;
                    UseAbility();
                    AnimationEnded = true;
                }
                else
                {
                    firebolt.Position += modifier;
                }
            }
        }
    }
}
