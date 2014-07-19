using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes.World.Menu
{
    public class CharacterMenu : GameSystem, IScene
    {
        public GameSystem DisplayedPage { get; protected set; }
        public WorldScene WorldScene { get; protected set; }

        public CalligraphedImage GeneralPageButton { get; protected set; }
        public CalligraphedImage AttributesPageButton { get; protected set; }
        public CalligraphedImage AbilitiesPageButton { get; protected set; }

        public CharacterMenu(WorldScene worldScene, int selectedPage = 1, params object[] args)
        {
            WorldScene = worldScene;

            switch (selectedPage)
            {
                case 1: DisplayedPage = new Character.GeneralPage() { Parent = this };
                    break;
                case 2: DisplayedPage = new Character.AttributesPage() { Parent = this };
                    break;
                case 3: DisplayedPage = new Character.AbilitiesPage(args.Length > 0 ? (int)args[0] : 0) { Parent = this };
                    break;
                case 4: DisplayedPage = new Character.Abilities.AbilityPage(args[0] as Abilities.Ability, (int)args[1]) { Parent = this };
                    break;
            }

            GeneralPageButton = new CalligraphedImage(427, 150, Vector2.Zero, Resources.Strings.CharacterMenu_General, FontStyle.CenturyGothic32, Color.White, new Vector2(0, -20)) { Parent = this, Path = @"Interface\World\Menu\Button.dds" };
            AttributesPageButton = new CalligraphedImage(427, 150, new Vector2(427, 0), Resources.Strings.CharacterMenu_Attributes, FontStyle.CenturyGothic32, Color.White, new Vector2(0, -20)) { Parent = this, Path = @"Interface\World\Menu\Button.dds" };
            AbilitiesPageButton = new CalligraphedImage(427, 150, new Vector2(854, 0), Resources.Strings.CharacterMenu_Abilities, FontStyle.CenturyGothic32, Color.White, new Vector2(0, -20)) { Parent = this, Path = @"Interface\World\Menu\Button.dds" }; 
        }

        public override void Initialize()
        {
            base.Initialize();
            DisplayedPage.Initialize();
            GeneralPageButton.Initialize();
            AttributesPageButton.Initialize();
            AbilitiesPageButton.Initialize();

            GeneralPageButton.TouchBoundaries = new RectangleF(0, 0, 0, -40);
            AttributesPageButton.TouchBoundaries = new RectangleF(0, 0, 0, -40);
            AbilitiesPageButton.TouchBoundaries = new RectangleF(0, 0, 0, -40);


            if (DisplayedPage is Character.GeneralPage)
            {
                GeneralPageButton.Layer = 1;
            }
            else if (DisplayedPage is Character.AttributesPage)
            {
                AttributesPageButton.Layer = 1;
            }
            else if (DisplayedPage is Character.AbilitiesPage || DisplayedPage is Character.Abilities.AbilityPage)
            {
                AbilitiesPageButton.Layer = 1;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            DisplayedPage.LoadContent(contentManager);
            GeneralPageButton.LoadContent(contentManager);
            AttributesPageButton.LoadContent(contentManager);
            AbilitiesPageButton.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            int goTo = 0;

            DisplayedPage.Update(gameTime);

            TouchManager.OnTap(GeneralPageButton, () => goTo = 1);
            TouchManager.OnTap(AttributesPageButton, () => goTo = 2);
            TouchManager.OnTap(AbilitiesPageButton, () => goTo = 3);

            if (goTo != 0)
            {
                SceneManager.Play(new CharacterMenu(WorldScene, goTo));
            }
        }

        public override void Draw(Renderer renderer)
        {
            DisplayedPage.Draw(renderer);
            GeneralPageButton.Draw(renderer);
            AttributesPageButton.Draw(renderer);
            AbilitiesPageButton.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            DisplayedPage.UnloadContent(contentManager);
            GeneralPageButton.UnloadContent(contentManager);
            AttributesPageButton.UnloadContent(contentManager);
            AbilitiesPageButton.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        public void OnBackKeyPress()
        {
            if (DisplayedPage is Character.Abilities.AbilityPage)
            {
                SceneManager.Play(new CharacterMenu(WorldScene, 3, (DisplayedPage as Character.Abilities.AbilityPage).SetIndex));
            }
            else SceneManager.Play(WorldScene);
        }
    }
}
