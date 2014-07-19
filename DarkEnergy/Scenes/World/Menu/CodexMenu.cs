using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes.World.Menu
{
    public class CodexMenu : GameSystem, IScene
    {
        public GameSystem DisplayedPage { get; protected set; }
        public WorldScene WorldScene { get; protected set; }

        public CalligraphedImage QuestsPageButton { get; protected set; }
        public CalligraphedImage AchievementsPageButton { get; protected set; }

        public CodexMenu(WorldScene worldScene, int selectedPage = 1, params object[] args)
        {
            WorldScene = worldScene;

            switch (selectedPage)
            {
                case 1: DisplayedPage = new Codex.QuestsPage() { Parent = this };
                    break;
                case 2: DisplayedPage = new Codex.AchievementsPage() { Parent = this };
                    break;
            }

            QuestsPageButton = new CalligraphedImage(640, 150, Vector2.Zero, Resources.Strings.CodexMenu_Quests, FontStyle.CenturyGothic32, Color.White, new Vector2(0, -20)) { Parent = this, Path = @"Interface\World\Menu\WideButton.dds" };
            AchievementsPageButton = new CalligraphedImage(640, 150, new Vector2(640, 0), Resources.Strings.CodexMenu_Achievements, FontStyle.CenturyGothic32, Color.White, new Vector2(0, -20)) { Parent = this, Path = @"Interface\World\Menu\WideButton.dds" };
        }

        public override void Initialize()
        {
            base.Initialize();
            DisplayedPage.Initialize();
            QuestsPageButton.Initialize();
            AchievementsPageButton.Initialize();

            QuestsPageButton.TouchBoundaries = new RectangleF(0, 0, 0, -40);
            AchievementsPageButton.TouchBoundaries = new RectangleF(0, 0, 0, -40);

            if (DisplayedPage is Codex.QuestsPage)
            {
                QuestsPageButton.Layer = 1;
            }
            else if (DisplayedPage is Codex.AchievementsPage)
            {
                AchievementsPageButton.Layer = 1;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            DisplayedPage.LoadContent(contentManager);
            QuestsPageButton.LoadContent(contentManager);
            AchievementsPageButton.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            int goTo = 0;

            DisplayedPage.Update(gameTime);

            TouchManager.OnTap(QuestsPageButton, () => goTo = 1);
            TouchManager.OnTap(AchievementsPageButton, () => goTo = 2);

            if (goTo != 0)
            {
                SceneManager.Play(new CodexMenu(WorldScene, goTo));
            }
        }

        public override void Draw(Renderer renderer)
        {
            DisplayedPage.Draw(renderer);
            QuestsPageButton.Draw(renderer);
            AchievementsPageButton.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            DisplayedPage.UnloadContent(contentManager);
            QuestsPageButton.UnloadContent(contentManager);
            AchievementsPageButton.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        public void OnBackKeyPress()
        {
            SceneManager.Play(WorldScene);
        }
    }
}
