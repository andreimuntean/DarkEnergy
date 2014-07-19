using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    internal class LoadingScreen : GameSystem, INotification
    {
        private TexturedElement background;
        private TexturedElement title;
        private Text text;
        private Action action;
        private double elapsedTime;
        private bool isLoading;
        private bool drawnOnce;

        // The minimum number of seconds for which the loading screen will be visible.
        private const float minimumLoadTime = 0.5f;

        public int Importance { get { return 1; } }

        public LoadingScreen(Action action, string text)
        {
            this.action = action;
            this.text = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 120) { Parent = this, String = text };
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\DefaultBackground.dds" };
            title = new TexturedElement(540, 88, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this, Path = @"Interface\Title.dds" };
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            title.Initialize();
            text.Initialize();
            elapsedTime = 0;
            isLoading = false;
            drawnOnce = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            title.LoadContent(contentManager);
            text.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (drawnOnce && !isLoading)
            {
                isLoading = true;
                TouchManager.Enabled = false;
                action();
            }

            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= minimumLoadTime)
            {
                TouchManager.Enabled = true;
                NotificationManager.Remove(this);
            }
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            title.Draw(renderer);
            text.Draw(renderer);
            drawnOnce = true;
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            title.UnloadContent(contentManager);
            text.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }

    static class LoadingScreenManager
    {
        public static void Show(Action action, string text)
        {
            if (!NotificationManager.ContainsItemOfType(typeof(LoadingScreen)))
            {
                NotificationManager.Add(new LoadingScreen(action, text));
            }
        }
    }
}
