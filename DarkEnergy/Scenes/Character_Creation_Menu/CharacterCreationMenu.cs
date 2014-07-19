using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Inventory.Slots;

namespace DarkEnergy.Scenes
{
    public class CharacterCreationMenu : GameSystem, IScene
    {
        private TexturedElement background;
        private Keyboard keyboard;
        private MessageBox alert;
        private CalligraphedImage header;
        private CalligraphedImage createCharacterButton;
        private Character_Creation_Menu.CharacterFeaturesPanel featuresPanel;
        private Character_Creation_Menu.CharacterDisplayPanel displayPanel;

        public int CharacterId { get; set; }

        public CharacterCreationMenu()
        {
            GameManager.InitializeInventory();
            background = new TexturedElement(1280, 720, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this, Path = @"Interface\TitlelessScreen.dds" };
            header = new CalligraphedImage(1000, 40, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, -305, Resources.Strings.CharacterCreation, FontStyle.SegoeWP32, Color.White, new Vector2(-4, -4)) { Parent = this, Path = "Interface/HeaderBackground.dds" };
            featuresPanel = new Character_Creation_Menu.CharacterFeaturesPanel() { Parent = this, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Middle, Offset = new Vector2(-32, 0) };
            displayPanel = new Character_Creation_Menu.CharacterDisplayPanel() { Parent = this, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Middle, Offset = new Vector2(32, 0) };
            createCharacterButton = new CalligraphedImage(316, 80, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 305, Resources.Strings.CreateCharacter, FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Parent = this, Path = "Interface/DefaultButton.dds" };
            keyboard = new Keyboard();
            alert = new MessageBox();
        }

        private void createCharacter()
        {
            var id = CharacterId;
            var name = displayPanel.Name.String;
            var gender = featuresPanel.Gender;
            var skin = featuresPanel.Skin;
            var face = featuresPanel.Face;
            var hair = featuresPanel.Hair;

            GameManager.CreateCharacter(id, name, gender, skin, face, hair);
        }

        public override void Initialize()
        {
            base.Initialize();
            keyboard.Initialize();
            alert.Initialize();
            background.Initialize();
            header.Initialize();
            createCharacterButton.Initialize();
            featuresPanel.Initialize();
            displayPanel.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            keyboard.LoadContent(contentManager);
            alert.LoadContent(contentManager);
            background.LoadContent(contentManager);
            header.LoadContent(contentManager);
            createCharacterButton.LoadContent(contentManager);
            featuresPanel.LoadContent(contentManager);
            displayPanel.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update(gameTime);
            alert.Update(gameTime);
            background.Update(gameTime);
            header.Update(gameTime);
            createCharacterButton.Update(gameTime);
            featuresPanel.Update(gameTime);
            displayPanel.Update(gameTime);

            bool appearanceChanged = false;

            if (displayPanel.Character.Features.Gender != featuresPanel.Gender)
            {
                displayPanel.Character.Features.Gender = featuresPanel.Gender;
                displayPanel.Refresh();
                appearanceChanged = true;
            }

            if (displayPanel.Character.Features.Skin != featuresPanel.Skin)
            {
                displayPanel.Character.Features.Skin = featuresPanel.Skin;
                appearanceChanged = true;
            }

            if (displayPanel.Character.Features.Face != featuresPanel.Face)
            {
                displayPanel.Character.Features.Face = featuresPanel.Face;
                appearanceChanged = true;
            }

            if (displayPanel.Character.Features.Hair != featuresPanel.Hair)
            {
                displayPanel.Character.Features.Hair = featuresPanel.Hair;
                appearanceChanged = true;
            }

            if (appearanceChanged)
            {
                var contentManager = App.Game.Content;

                displayPanel.Character.UnloadContent(contentManager);
                displayPanel.Character.Features.Refresh();
                displayPanel.Character.LoadContent(contentManager);
            }

            TouchManager.OnTap(createCharacterButton, () =>
            {
                if (displayPanel.Name.String.Length > 0)
                {
                    createCharacter();
                    SceneManager.Play(new CharacterSelectionMenu());
                }
                else
                {
                    alert.Show(Resources.Strings.CharacterNameAlert, new string[] { Resources.Strings.OK });
                }
            });
            
            TouchManager.OnTap(displayPanel.Name, () => keyboard.Show(displayPanel.Name.GetText()));

            if (alert.Result == Resources.Strings.OK)
            {
                alert.Hide();
            }
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            createCharacterButton.Draw(renderer);
            featuresPanel.Draw(renderer);
            displayPanel.Draw(renderer);
            header.Draw(renderer);
            alert.Draw(renderer);
            keyboard.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            keyboard.UnloadContent(contentManager);
            alert.UnloadContent(contentManager);
            background.UnloadContent(contentManager);
            header.UnloadContent(contentManager);
            createCharacterButton.UnloadContent(contentManager);
            featuresPanel.UnloadContent(contentManager);
            displayPanel.UnloadContent(contentManager);
        }

        public void OnBackKeyPress()
        {
            if (keyboard.Visible)
            {
                keyboard.Hide();
            }
            else if (alert.Visible)
            {
                alert.Hide();
            }
            else
            {
                SceneManager.Play(new CharacterSelectionMenu());
            }
        }
    }
}
