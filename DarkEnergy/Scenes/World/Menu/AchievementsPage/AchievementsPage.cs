using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes.World.Menu.Codex
{
    public class AchievementsPage : GameSystem
    {
        private TexturedElement background;
        private Text unavailableMessage;

        public AchievementsPage()
        {
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\AchievementsBackground.dds" };
            unavailableMessage = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 0, 145) { Parent = this, String = Resources.Strings.NoAchievementsMessage };
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            unavailableMessage.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            unavailableMessage.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            unavailableMessage.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            unavailableMessage.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
