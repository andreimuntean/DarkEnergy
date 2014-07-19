using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Notifications.SaveGameIcon
{
    internal class SavingIcon : TexturedElement, INotification
    {
        private bool fadeOut, fadeIn;

        public int Importance { get { return 3; } }

        public SavingIcon() : base(40, 25, HorizontalAlignment.Right, -32, VerticalAlignment.Bottom, -32)
        {
            Path = "Interface/LoadingIcon.dds";
        }

        public void FadeOut()
        {
            fadeOut = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            AnimationRate = 60.0f;
            Scale = new Vector2(2, 2);
            FrameCount = 99;
            Opacity = 0.0f;

            fadeIn = true;
            fadeOut = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (fadeIn)
            {
                if (Opacity < 1)
                {
                    Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    fadeIn = false;
                }
            }
            else if (fadeOut)
            {
                if (Opacity > 0)
                {
                    Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    NotificationManager.Remove(this);
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
        }
    }
}
