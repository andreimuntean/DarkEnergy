using System;
using Microsoft.Phone.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes
{
    public class MainMenu : GameSystem, IScene
    {
        private TexturedElement background;
        private TexturedElement title;
        private Main_Menu.SelectionMenu mainSelectionMenu;

        public MainMenu()
        {
            string[] menuOptions = new string[] { Resources.Strings.MainMenu_Community, Resources.Strings.MainMenu_PlayGame, Resources.Strings.MainMenu_Feedback };

            background = new TexturedElement(1280, 720, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Path = @"Interface\TitleScreen.dds", Parent = this };
            title = new TexturedElement(540, 88, HorizontalAlignment.Center, 160, VerticalAlignment.Middle, -4) { Path = @"Interface\Title.dds", Parent = this };
            mainSelectionMenu = new Main_Menu.SelectionMenu(menuOptions, 1) { Offset = new Vector2(30, -9), Parent = this };
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            title.Initialize();
            mainSelectionMenu.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            title.LoadContent(contentManager);
            mainSelectionMenu.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            background.Update(gameTime);
            title.Update(gameTime);
            mainSelectionMenu.Update(gameTime);

            if (mainSelectionMenu.GoToSelectedOption)
            {
                switch (mainSelectionMenu.SelectedOption)
                {
                    case 0: WebBrowserTask webBrowserTask = new WebBrowserTask();
                        webBrowserTask.Uri = new Uri("https://www.facebook.com/pages/Dark-Energy/477456289033000", UriKind.Absolute);
                        webBrowserTask.Show();
                        mainSelectionMenu.Reactivate();
                        break;
                    case 1: SceneManager.Play(new CharacterSelectionMenu());
                        break;
                    case 2: MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                        marketplaceReviewTask.Show();
                        mainSelectionMenu.Reactivate();
                        break;
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            mainSelectionMenu.Draw(renderer);
            title.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            title.UnloadContent(contentManager);
            mainSelectionMenu.UnloadContent(contentManager);
        }

        public void OnBackKeyPress()
        {
            System.Windows.Application.Current.Terminate();
        }
    }
}
