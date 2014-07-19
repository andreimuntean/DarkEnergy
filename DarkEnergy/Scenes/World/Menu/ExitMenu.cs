using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.InterfaceMenu
{
    public class ExitMenu : TexturedElement
    {
        protected TexturedElement exitButton, saveButton;

        public ExitMenu() : base(480, 280, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0)
        {
            Path = @"Interface\ExitMenu.dds";
            saveButton = new CalligraphedImage(440, 110, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, -65f, Resources.Strings.ExitMenu_Save, FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Parent = this, Path = @"Interface\MessageBoxButton.dds" };
            exitButton = new CalligraphedImage(440, 110, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 65f, Resources.Strings.ExitMenu_Exit, FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Parent = this, Path = @"Interface\MessageBoxButton.dds" };
        }

        public void Toggle()
        {
            Visible = !Visible;
        }

        public override void Initialize()
        {
            base.Initialize();
            saveButton.Initialize();
            exitButton.Initialize();
            Visible = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            saveButton.LoadContent(contentManager);
            exitButton.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                TouchManager.OnTap(() =>
                {
                    if (exitButton.Tapped || saveButton.Tapped)
                    {
                        GameManager.SaveGame();

                        if (exitButton.Tapped)
                        {
                            SceneManager.Play(new Scenes.MainMenu());
                        }
                    }

                    Toggle();
                });
            }
        }

        public override void Draw(Renderer renderer)
        {
            if (Visible)
            {
                base.Draw(renderer);
                saveButton.Draw(renderer);
                exitButton.Draw(renderer);
            }
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            saveButton.UnloadContent(contentManager);
            exitButton.UnloadContent(contentManager);
        }
    }
}
