using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.InterfaceMenu;

namespace DarkEnergy.Scenes.World
{
    public abstract class WorldScene : GameSystem, IScene, ISaveable
    {
        private TexturedElement background;

        public TexturedElement CharacterButton { get; protected set; }
        public TexturedElement InventoryButton { get; protected set; }
        public TexturedElement CodexButton { get; protected set; }
        public ExitMenu ExitMenu { get; protected set; }

        public WorldScene()
        {
            background = new TexturedElement(1280, 72, HorizontalAlignment.Center, 0, VerticalAlignment.Bottom, 2) { Parent = this, Path = @"Interface\World\Background.dds" };
            CharacterButton = new TexturedElement(120, 120, HorizontalAlignment.Center, 0, VerticalAlignment.Bottom, -10f) { Parent = this, Path = @"Interface\World\CharacterIcon.dds" };
            InventoryButton = new TexturedElement(100, 100, HorizontalAlignment.Center, -150, VerticalAlignment.Bottom, -20f) { Parent = this, Path = @"Interface\World\InventoryIcon.dds" };
            CodexButton = new TexturedElement(100, 100, HorizontalAlignment.Center, 150, VerticalAlignment.Bottom, -20f) { Parent = this, Path = @"Interface\World\CodexIcon.dds" };
            ExitMenu = new ExitMenu();
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            CharacterButton.Initialize();
            InventoryButton.Initialize();
            CodexButton.Initialize();
            ExitMenu.Initialize();
            SaveData();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            CharacterButton.LoadContent(contentManager);
            InventoryButton.LoadContent(contentManager);
            CodexButton.LoadContent(contentManager);
            ExitMenu.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            ExitMenu.Update(gameTime);

            TouchManager.OnTap(CharacterButton, () => SceneManager.Play(new Menu.CharacterMenu(this)));
            TouchManager.OnTap(InventoryButton, () => SceneManager.Play(new Menu.InventoryMenu(this)));
            TouchManager.OnTap(CodexButton, () => SceneManager.Play(new Menu.CodexMenu(this)));
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            CharacterButton.Draw(renderer);
            InventoryButton.Draw(renderer);
            CodexButton.Draw(renderer);
            ExitMenu.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            CharacterButton.UnloadContent(contentManager);
            InventoryButton.UnloadContent(contentManager);
            CodexButton.UnloadContent(contentManager);
            ExitMenu.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        public abstract void SaveData();

        public virtual void OnBackKeyPress()
        {
            ExitMenu.Toggle();
        }
    }
}
