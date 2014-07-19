using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Notifications
{
    public class DebugMenu : GameSystem, INotification
    {
        private Text gameVersion;
        private Debug_Menu.FPSCounter fpsCounter;
        private Debug_Menu.PointerPosition mousePosition;

        public int Importance { get { return 2; } }
        
        public DebugMenu()
        {
            gameVersion = new Text(FontStyle.Consolas18, HorizontalAlignment.Center, 0, VerticalAlignment.Bottom, 0) { Parent = this, String = "Dark Energy " + Resources.Strings.Version };
            fpsCounter = new Debug_Menu.FPSCounter() { Parent = this };
            mousePosition = new Debug_Menu.PointerPosition() { Parent = this };
        }

        public override void Initialize()
        {
            gameVersion.Initialize();
            fpsCounter.Initialize();
            mousePosition.Initialize();
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            gameVersion.LoadContent(contentManager);
            fpsCounter.LoadContent(contentManager);
            mousePosition.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            gameVersion.Update(gameTime);
            fpsCounter.Update(gameTime);
            mousePosition.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            gameVersion.Draw(renderer);
            fpsCounter.Draw(renderer);
            mousePosition.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            gameVersion.UnloadContent(contentManager);
            fpsCounter.UnloadContent(contentManager);
            mousePosition.UnloadContent(contentManager);
        }
    }
}
