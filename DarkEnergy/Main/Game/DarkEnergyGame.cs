using System;
using SharpDX;
using SharpDX.Toolkit;

namespace DarkEnergy
{
    public sealed class DarkEnergyGame : Game
    {
        public Screen Screen { get; private set; }

        public GameSystem Scene { get; set; }

        public NotificationManager NotificationManager { get; private set; }

        public Renderer Renderer { get; private set; }
        
        public FPSCounter UpdatePerformance { get; private set; }
        
        public FPSCounter DrawPerformance { get; private set; }

        public bool IsPaused { get; private set; }

        public DarkEnergyGame()
        {
            SettingsManager.Load();
            DrawPerformance = new FPSCounter();
            UpdatePerformance = new FPSCounter();
            Renderer = new Renderer(this);
            NotificationManager = new NotificationManager();
            GameManager.Construct();
        }

        public void SetScreen(Screen screen)
        {
            this.Screen = screen;
            Renderer.SetResolution(screen.Resolution);
        }

        public void Pause()
        {
            IsPaused = true;
            SuppressDraw();
            while (Renderer.IsRendering)
            {
                // Wait for the Renderer to finish rendering.
                System.Threading.Thread.Sleep(1);
            }
        }

        public void Resume()
        {
            Renderer.IsActivated = true;
            ResetElapsedTime();
            IsPaused = false;
        }
        
        protected override void Initialize()
        {
            Window.Title = Resources.Strings.Title;
            Content.RootDirectory = "Assets";

            IsFixedTimeStep = true;
            IsPaused = false;
            TargetElapsedTime = TimeSpan.FromSeconds((SettingsManager.FrameRateLimit <= 0)? 0.001 : 1 / SettingsManager.FrameRateLimit);

            Renderer.Initialize(this);

            SceneManager.Play(new Scenes.MainMenu());
            if (SettingsManager.DebugMenuEnabled) NotificationManager.Add(new Notifications.DebugMenu());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Scene.LoadContent(Content);
            NotificationManager.LoadContent(Content);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsPaused)
            {
                UpdatePerformance.Tick();

                NotificationManager.Update(gameTime);
                Scene.Update(gameTime);

                TouchManager.Refresh();

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!IsPaused)
            {
                if (Renderer.IsActivated)
                {
                    Renderer.Initialize(this);
                }

                DrawPerformance.Tick();

                Renderer.Begin();

                Scene.Draw(Renderer);
                NotificationManager.Draw(Renderer);
                
                Renderer.End();

                base.Draw(gameTime);
            }
        }

        protected override void UnloadContent()
        {
            Scene.UnloadContent(Content);
            NotificationManager.UnloadContent(Content);
            base.UnloadContent();
        }
    }
}
