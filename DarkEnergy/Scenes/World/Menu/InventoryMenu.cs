using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes.World.Menu
{
    public class InventoryMenu : GameSystem, IScene
    {
        public GameSystem DisplayedPage { get; protected set; }
        public WorldScene WorldScene { get; protected set; }

        public CalligraphedImage InventoryPageButton { get; protected set; }
        public CalligraphedImage EquipmentPageButton { get; protected set; }

        public InventoryMenu(WorldScene worldScene, int selectedPage = 1, params object[] args)
        {
            WorldScene = worldScene;

            switch (selectedPage)
            {
                case 1: DisplayedPage = new Inventory.InventoryPage() { Parent = this };
                    break;
                case 2: DisplayedPage = new Inventory.EquipmentPage() { Parent = this };
                    break;
            }

            InventoryPageButton = new CalligraphedImage(640, 150, Vector2.Zero, Resources.Strings.InventoryMenu_Inventory, FontStyle.CenturyGothic32, Color.White, new Vector2(0, -20)) { Parent = this, Path = @"Interface\World\Menu\WideButton.dds" };
            EquipmentPageButton = new CalligraphedImage(640, 150, new Vector2(640, 0), Resources.Strings.InventoryMenu_Equipment, FontStyle.CenturyGothic32, Color.White, new Vector2(0, -20)) { Parent = this, Path = @"Interface\World\Menu\WideButton.dds" };
        }

        public override void Initialize()
        {
            base.Initialize();
            DisplayedPage.Initialize();
            InventoryPageButton.Initialize();
            EquipmentPageButton.Initialize();

            InventoryPageButton.TouchBoundaries = new RectangleF(0, 0, 0, -40);
            EquipmentPageButton.TouchBoundaries = new RectangleF(0, 0, 0, -40);

            if (DisplayedPage is Inventory.InventoryPage)
            {
                InventoryPageButton.Layer = 1;
            }
            else if (DisplayedPage is Inventory.EquipmentPage)
            {
                EquipmentPageButton.Layer = 1;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            DisplayedPage.LoadContent(contentManager);
            InventoryPageButton.LoadContent(contentManager);
            EquipmentPageButton.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            int goTo = 0;

            DisplayedPage.Update(gameTime);

            TouchManager.OnTap(InventoryPageButton, () => goTo = 1);
            TouchManager.OnTap(EquipmentPageButton, () => goTo = 2);

            if (goTo != 0)
            {
                SceneManager.Play(new InventoryMenu(WorldScene, goTo));
            }
        }

        public override void Draw(Renderer renderer)
        {
            DisplayedPage.Draw(renderer);
            InventoryPageButton.Draw(renderer);
            EquipmentPageButton.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            DisplayedPage.UnloadContent(contentManager);
            InventoryPageButton.UnloadContent(contentManager);
            EquipmentPageButton.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        public void OnBackKeyPress()
        {
            SceneManager.Play(WorldScene);
        }
    }
}
