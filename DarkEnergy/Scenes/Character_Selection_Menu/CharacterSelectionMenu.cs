using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes
{
    public class CharacterSelectionMenu : GameSystem, IScene, ILoadable
    {
        private Character_Selection_Menu.CharacterList characterList;
        private TexturedElement background;
        private CalligraphedImage header;
        private CalligraphedImage createCharacterButton;
        private CalligraphedImage deleteCharacterButton;
        private CalligraphedImage playButton;
        private MessageBox confirmation;

        public CharacterSelectionMenu()
        {
            background = new TexturedElement(1280, 720, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this, Path = @"Interface\TitlelessScreen.dds" };
            header = new CalligraphedImage(1000, 40, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, -305, Resources.Strings.CharacterList, FontStyle.SegoeWP32, Color.White, new Vector2(-4, -4)) { Path = "Interface/HeaderBackground.dds", Parent = this };
            createCharacterButton = new CalligraphedImage(316, 80, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 305, Resources.Strings.CreateCharacter, FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Path = "Interface/DefaultButton.dds", Parent = this };
            deleteCharacterButton = new CalligraphedImage(316, 80, HorizontalAlignment.Center, -240, VerticalAlignment.Middle, 305, Resources.Strings.DeleteCharacter, FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Path = "Interface/DefaultButton.dds", Parent = this };
            playButton = new CalligraphedImage(316, 80, HorizontalAlignment.Center, 240, VerticalAlignment.Middle, 305, Resources.Strings.Play, FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Path = "Interface/DefaultButton.dds", Parent = this };
            characterList = new Character_Selection_Menu.CharacterList() { Parent = this, Offset = new Vector2(1, 0) };
            characterList.SelectedIndexChanged += ShowButtons;
            confirmation = new MessageBox();
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            characterList.Initialize();
            header.Initialize();
            createCharacterButton.Initialize();
            deleteCharacterButton.Initialize();
            playButton.Initialize();
            confirmation.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            characterList.LoadContent(contentManager);
            header.LoadContent(contentManager);
            createCharacterButton.LoadContent(contentManager);
            deleteCharacterButton.LoadContent(contentManager);
            playButton.LoadContent(contentManager);
            confirmation.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            confirmation.Update(gameTime);

            TouchManager.OnTap(createCharacterButton, () => SceneManager.Play(new CharacterCreationMenu() { CharacterId = characterList.SelectedIndex + 1 }));
            TouchManager.OnTap(deleteCharacterButton, () => confirmation.Show(Resources.Strings.CharacterDeletionConfirmation + "\n" + characterList.SelectedSlot.Name + "?", new string[] { Resources.Strings.No, Resources.Strings.Yes }));
            TouchManager.OnTap(playButton, () => GameManager.Start(characterList.SelectedIndex + 1));

            if (confirmation.Result == Resources.Strings.Yes)
            {
                characterList.DeleteCharacter();
                ShowButtons(null, EventArgs.Empty);
                confirmation.Hide();
            }
            else if (confirmation.Result == Resources.Strings.No)
            {
                confirmation.Hide();
            }

            characterList.Update(gameTime);
            header.Update(gameTime);
            createCharacterButton.Update(gameTime);
            deleteCharacterButton.Update(gameTime);
            playButton.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            characterList.Draw(renderer);
            header.Draw(renderer);
            createCharacterButton.Draw(renderer);
            deleteCharacterButton.Draw(renderer);
            playButton.Draw(renderer);
            confirmation.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            characterList.UnloadContent(contentManager);
            header.UnloadContent(contentManager);
            createCharacterButton.UnloadContent(contentManager);
            deleteCharacterButton.UnloadContent(contentManager);
            playButton.UnloadContent(contentManager);
            confirmation.UnloadContent(contentManager);
        }

        public void LoadData()
        {
            characterList.LoadData();
            ShowButtons(null, EventArgs.Empty);
        }

        public void OnBackKeyPress()
        {
            if (confirmation.Visible)
            {
                confirmation.Hide();
            }
            else
            {
                SceneManager.Play(new MainMenu());
            }
        }

        #region Events
        private void ShowButtons(object sender, EventArgs e)
        {
            deleteCharacterButton.Visible = !characterList.IsSelectedSlotEmpty;
            createCharacterButton.Visible = characterList.IsSelectedSlotEmpty;
            playButton.Visible = !characterList.IsSelectedSlotEmpty;
        }
        #endregion
    }
}
