using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes.Character_Selection_Menu
{
    public class CharacterSlot : TexturedElement
    {
        public bool IsEmpty { get { return (name.String == "" && status.String == ""); } }
        private Text emptyMessage;
        private Text name;
        private Text status;

        public string Name
        {
            get { return name.String; }
            set
            {
                name.String = value;
                if (name.IsLoaded) CharacterSlot_AdjustElements(null, EventArgs.Empty);
            }
        }

        public string Status
        {
            get { return status.String; }
            set
            {
                status.String = value;
                if (status.IsLoaded) CharacterSlot_AdjustElements(null, EventArgs.Empty);
            }
        }

        public CharacterSlot() : base(800, 90)
        {
            emptyMessage = new Text(FontStyle.SegoeWP32) { Parent = this, String = Resources.Strings.EmptyCharacterSlot };
            name = new Text(FontStyle.SegoeWP32) { Parent = this, String = "" };
            status = new Text(FontStyle.SegoeWP32) { Parent = this, String = "" };
            Path = "Interface/CharacterSlot.dds";
            Loaded += CharacterSlot_AdjustElements;
            PositionChanged += CharacterSlot_AdjustElements;
        }

        public void Clear()
        {
            Name = "";
            Status = "";
        }

        public override void Initialize()
        {
            base.Initialize();
            emptyMessage.Initialize();
            name.Initialize();
            status.Initialize();
            Clear();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            emptyMessage.LoadContent(contentManager);
            name.LoadContent(contentManager);
            status.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            emptyMessage.Visible = IsEmpty;
            name.Visible = !IsEmpty;
            status.Visible = !IsEmpty;
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            emptyMessage.Draw(renderer);
            name.Draw(renderer);
            status.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            emptyMessage.UnloadContent(contentManager);
            name.UnloadContent(contentManager);
            status.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        private void CharacterSlot_AdjustElements(object sender, EventArgs e)
        {
            emptyMessage.Position = new Vector2(PositionRectangle.Center.X - emptyMessage.Width / 2, Y + 15);
            name.Position = new Vector2(PositionRectangle.Left + 80, Y + 15);
            status.Position = new Vector2(PositionRectangle.Right - status.Width - 80, Y + 15);
        }
        #endregion
    }

    public class CharacterList : GameSystem, ILoadable
    {
        private List<CharacterSlot> characterSlots;

        public event EventHandler SelectedIndexChanged;

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnSelectedIndexChanged(EventArgs.Empty);
            }
        }

        public CharacterSlot SelectedSlot { get { return characterSlots[SelectedIndex]; } }

        public bool IsSelectedSlotEmpty { get { return characterSlots[SelectedIndex].IsEmpty; } }
        
        public Vector2 Offset { get; set; }

        public CharacterList()
        {
            Offset = Vector2.Zero;
            characterSlots = new List<CharacterSlot>();
            for (byte i = 1; i <= 4; ++i)
            {
                characterSlots.Add(new CharacterSlot() { Parent = this, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Middle });
            }
        }

        public void DeleteCharacter()
        {
            DataStorageManager.DeleteCharacter(selectedIndex + 1);
            DataStorageManager.Flush();
            LoadData();
        }

        public override void Initialize()
        {
            base.Initialize();
            for (byte i = 0; i <= 3; ++i)
            {
                characterSlots[i].Initialize();
                characterSlots[i].Offset = new Vector2(Offset.X, Offset.Y - 170 + i * 108);
            }
            characterSlots[SelectedIndex].Layer = 1;
        }

        public void LoadData()
        {
            for (byte i = 1; i <= 4; ++i)
            {
                string name = DataStorageManager.Load<string>("HeroName", i);
                if (name != default(string))
                {
                    string status = "Level " + DataStorageManager.Load<int>("HeroLevel", i).ToString();
                    characterSlots[i - 1].Name = name;
                    characterSlots[i - 1].Status = status;
                }
                else
                {
                    characterSlots[i - 1].Clear();
                    // break;
                }
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            foreach (var characterSlot in characterSlots)
            {
                characterSlot.LoadContent(contentManager);
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (byte i = 0; i <= 3; ++i)
            {
                if (characterSlots[i].Tapped)
                {
                    SelectedIndex = i;
                    foreach (var characterSlot in characterSlots)
                    {
                        characterSlot.Layer = 0;
                    }
                    characterSlots[i].Layer = 1;
                }
                characterSlots[i].Update(gameTime);
            }
        }

        public override void Draw(Renderer graphicsDeviceManager)
        {
            foreach (var characterSlot in characterSlots)
            {
                characterSlot.Draw(graphicsDeviceManager);
            }
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            foreach (var characterSlot in characterSlots)
            {
                characterSlot.UnloadContent(contentManager);
            }
        }

        #region Events
        protected void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, e);
            }
        }
        #endregion
    }
}
