using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Combat;

namespace DarkEnergy.Abilities.Visual
{
    public sealed class Frenzy : AbilityVisual
    {
        private const float fadeTime = 0.25f;
        private const float pausedTime = 0.5f;
        private const float totalMovement = 25f;
        private float elapsedTime;
        private TexturedElement effect;
        private Vector2 start;
        private Vector2 end;

        public Frenzy(CombatAction action) : base(action)
        {
            DrawAboveUnits = true;
            var current = action.Current;

            elapsedTime = 0.0f;
            start = current.Position + new Vector2((current.Width - 135) / 2.0f, -135);
            end = start + new Vector2(0, -totalMovement);

            effect = new TexturedElement(135, 135, start) { Parent = this, Path = @"Ability\Effects\Frenzy.dds" };
            Components.Add(effect);
        }

        public override void Initialize()
        {
            base.Initialize();
            effect.Opacity = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!AnimationEnded)
            {
                var modifier = (float)(gameTime.ElapsedGameTime.TotalSeconds);

                if (!AbilityUsed)
                {
                    modifier /= fadeTime;
                    var movement = -modifier * totalMovement;
                    var stop = effect.Y + movement <= end.Y;

                    if (stop)
                    {
                        effect.Position = end;
                        effect.Opacity = 1.0f;
                        UseAbility();
                    }
                    else
                    {
                        effect.Position += new Vector2(0, movement);
                        effect.Opacity += modifier;
                    }
                }
                else
                {
                    if (elapsedTime + modifier <= pausedTime)
                    {
                        elapsedTime += modifier;
                    }
                    else
                    {
                        effect.Opacity -= modifier / fadeTime;
                        if (effect.Opacity == 0.0f)
                        {
                            AnimationEnded = true;
                        }
                    }
                }
            }
        }
    }
}
