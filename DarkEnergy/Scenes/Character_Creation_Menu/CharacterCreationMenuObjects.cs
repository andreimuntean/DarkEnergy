using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.Character_Creation_Menu
{
    public class Selector : TexturedElement
    {
        private Text header;
        private Text body;
        private TexturedElement left;
        private TexturedElement right;

        public byte SelectionIndex { get; set; }
        public byte SelectionCount { get; set; }
        public string Header { get { return header.String; } set { header.String = value; } }
        public string Body { get { return body.String; } }

        public Selector() : base(400, 100)
        {
            Path = "Interface/BackgroundPurple.dds";
            header = new Text(FontStyle.SegoeWP32) { Parent = this, String = "" };
            body = new Text(FontStyle.SegoeWP24) { Parent = this, String = "" };
            left = new TexturedElement(71, 90) { Parent = this, Path = "Interface/ButtonLeft.dds" };
            right = new TexturedElement(71, 90) { Parent = this, Path = "Interface/ButtonRight.dds" };
            SelectionIndex = 1;
            Loaded += Readjust;
            PositionChanged += Readjust;
            body.SizeChanged += Readjust;
        }

        public override void Initialize()
        {
            base.Initialize();
            header.Initialize();
            body.Initialize();
            left.Initialize();
            right.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            header.LoadContent(contentManager);
            body.LoadContent(contentManager);
            left.LoadContent(contentManager);
            right.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TouchManager.OnTap(right, () => SelectionIndex = (SelectionIndex < SelectionCount) ? (byte)(SelectionIndex + 1) : (byte)1);
            TouchManager.OnTap(left, () => SelectionIndex = (SelectionIndex > 1) ? (byte)(SelectionIndex - 1) : (byte)SelectionCount);

            body.String = SelectionIndex.ToString() + " / " + SelectionCount.ToString();
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            header.Draw(renderer);
            body.Draw(renderer);
            left.Draw(renderer);
            right.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            header.UnloadContent(contentManager);
            body.UnloadContent(contentManager);
            left.UnloadContent(contentManager);
            right.UnloadContent(contentManager);
        }

        #region Events
        private void Readjust(object sender, System.EventArgs e)
        {
            left.Position = new Vector2(PositionRectangle.Left + 8, PositionRectangle.Center.Y - left.Height / 2);
            right.Position = new Vector2(PositionRectangle.Right - right.Width - 8, PositionRectangle.Center.Y - right.Height / 2);
            header.Position = new Vector2(PositionRectangle.Center.X - header.Width / 2, PositionRectangle.Center.Y - header.Height / 2 - 16);
            body.Position = new Vector2(PositionRectangle.Center.X - body.Width / 2, PositionRectangle.Center.Y - body.Height / 2 + 25);
        }
        #endregion
    }

    public class GenderSelector : TexturedElement
    {
        public event EventHandler SelectionChanged;

        private Text header;
        private Text body;
        private TexturedElement female;
        private TexturedElement male;

        private Gender selection;
        public Gender Selection
        {
            get { return selection; }
            set
            {
                selection = value;
                switch (selection)
                {
                    case Gender.Male: female.Layer = 0;
                        male.Layer = 1;
                        body.String = Resources.Strings.Character_Male;
                        break;
                    case Gender.Female: female.Layer = 1;
                        male.Layer = 0;
                        body.String = Resources.Strings.Character_Female;
                        break;
                }
            }
        }

        public string Header { get { return header.String; } set { header.String = value; } }
        public string Body { get { return body.String; } }

        public GenderSelector() : base(400, 119)
        {
            Path = "Interface/BackgroundWidePurple.dds";
            male = new TexturedElement(112, 112) { Parent = this, Path = "Interface/ButtonMale.dds" };
            female = new TexturedElement(112, 112) { Parent = this, Path = "Interface/ButtonFemale.dds" };
            header = new Text(FontStyle.SegoeWP32) { Parent = this, String = Resources.Strings.Character_Gender };
            body = new Text(FontStyle.SegoeWP24) { Parent = this };
            Loaded += Readjust;
            PositionChanged += Readjust;
            body.SizeChanged += Readjust;
        }

        public override void Initialize()
        {
            base.Initialize();
            header.Initialize();
            body.Initialize();
            female.Initialize();
            male.Initialize();
            Selection = Gender.Male;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            header.LoadContent(contentManager);
            body.LoadContent(contentManager);
            female.LoadContent(contentManager);
            male.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Selection == Gender.Male)
            {
                TouchManager.OnTap(female, () => { Selection = Gender.Female; OnSelectionChanged(EventArgs.Empty); });
            }
            else
            {
                TouchManager.OnTap(male, () => { Selection = Gender.Male; OnSelectionChanged(EventArgs.Empty); });
            }
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            header.Draw(renderer);
            body.Draw(renderer);
            male.Draw(renderer);
            female.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            header.UnloadContent(contentManager);
            body.UnloadContent(contentManager);
            male.UnloadContent(contentManager);
            female.UnloadContent(contentManager);
        }

        #region Events
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            // "If the event is listening and content is loaded ..."
            if (SelectionChanged != null && IsLoaded)
            {
                SelectionChanged(this, e);
            }
        }

        private void Readjust(object sender, System.EventArgs e)
        {
            male.Position = new Vector2(PositionRectangle.Left + 8, PositionRectangle.Center.Y - male.Height / 2);
            female.Position = new Vector2(PositionRectangle.Right - female.Width - 8, PositionRectangle.Center.Y - female.Height / 2);
            header.Position = new Vector2(PositionRectangle.Center.X - header.Width / 2, PositionRectangle.Center.Y - header.Height / 2 - 16);
            body.Position = new Vector2(PositionRectangle.Center.X - body.Width / 2, PositionRectangle.Center.Y - body.Height / 2 + 25);
        }
        #endregion
    }

    public class CharacterFeaturesPanel : TexturedElement
    {
        private Selector skin;
        private Selector face;
        private Selector hair;
        private GenderSelector gender;

        protected List<byte>[] SavedSelections;
        protected const byte femaleSkinOptions = 10;
        protected const byte femaleFaceOptions = 4;
        protected const byte femaleHairOptions = 4;
        protected const byte maleSkinOptions = 10;
        protected const byte maleFaceOptions = 11;
        protected const byte maleHairOptions = 6;

        public byte Skin { get { return skin.SelectionIndex; } }
        public byte Face { get { return face.SelectionIndex; } }
        public byte Hair { get { return hair.SelectionIndex; } }
        public Gender Gender { get { return gender.Selection; } }

        public CharacterFeaturesPanel() : base(432, 523)
        {
            Path = "Interface/BackgroundPanel.dds";
            skin = new Selector() { Parent = this, SelectionCount = maleSkinOptions, Header = Resources.Strings.Character_Skin };
            face = new Selector() { Parent = this, SelectionCount = maleFaceOptions, Header = Resources.Strings.Character_Face };
            hair = new Selector() { Parent = this, SelectionCount = maleHairOptions, Header = Resources.Strings.Character_Hair };
            gender = new GenderSelector() { Parent = this, Selection = Gender.Male };
            Loaded += Readjust;
            PositionChanged += Readjust;
            gender.SelectionChanged += GenderSelectionChanged;
        }

        public override void Initialize()
        {
            base.Initialize();

            SavedSelections = new List<byte>[]
            {
                new List<byte>() { 1, 1, 1 },
                new List<byte>() { 1, 1, 1 }
            };            

            skin.Initialize();
            face.Initialize();
            hair.Initialize();
            gender.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            skin.LoadContent(contentManager);
            face.LoadContent(contentManager);
            hair.LoadContent(contentManager);
            gender.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            skin.Update(gameTime);
            face.Update(gameTime);
            hair.Update(gameTime);
            gender.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            skin.Draw(renderer);
            face.Draw(renderer);
            hair.Draw(renderer);
            gender.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            skin.UnloadContent(contentManager);
            face.UnloadContent(contentManager);
            hair.UnloadContent(contentManager);
            gender.UnloadContent(contentManager);
        }

        #region Events
        protected void GenderSelectionChanged(object sender, EventArgs e)
        {
            int current = (int)Gender;
            int previous = Math.Abs(current - 1);

            SavedSelections[current][0] = skin.SelectionIndex;
            SavedSelections[current][1] = face.SelectionIndex;
            SavedSelections[current][2] = hair.SelectionIndex;
            
            skin.SelectionIndex = SavedSelections[previous][0];
            face.SelectionIndex = SavedSelections[previous][1];
            hair.SelectionIndex = SavedSelections[previous][2];
     
            if (current == 0)
            {
                skin.SelectionCount = maleSkinOptions;
                face.SelectionCount = maleFaceOptions;
                hair.SelectionCount = maleHairOptions;
            }
            else
            {
                skin.SelectionCount = femaleSkinOptions;
                face.SelectionCount = femaleFaceOptions;
                hair.SelectionCount = femaleHairOptions;
            }
        }

        private void Readjust(object sender, System.EventArgs e)
        {
            float x = PositionRectangle.Center.X - skin.Width / 2;
            gender.Position = new Vector2(PositionRectangle.Center.X - gender.Width / 2, PositionRectangle.Top + 24);
            skin.Position = new Vector2(x, gender.Y + gender.Height + 24);
            face.Position = new Vector2(x, skin.Y + skin.Height + 24);
            hair.Position = new Vector2(x, face.Y + face.Height + 24);
        }
        #endregion
    }

    public class CharacterDisplayPanel : TexturedElement
    {
        public CalligraphedImage Name { get; private set; }
        public Hero Character { get; private set; }

        public CharacterDisplayPanel() : base(432, 523)
        {
            Path = "Interface/BackgroundPanel.dds";
            Name = new CalligraphedImage(408, 77, "", FontStyle.SegoeWP32, Color.White, Vector2.Zero) { Parent = this, Path = "Interface/TextBackground.dds" };
            Character = new Hero();
            Loaded += ReadjustElements;
            PositionChanged += ReadjustElements;
        }

        public void Refresh()
        {
            ReadjustElements(null, EventArgs.Empty);
        }

        public override void Initialize()
        {
            base.Initialize();
            Name.Initialize();
            Character.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Name.LoadContent(contentManager);
            Character.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            Character.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            Character.Draw(renderer);
            Name.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            Name.UnloadContent(contentManager);
            Character.UnloadContent(contentManager);
        }

        #region Events
        private void ReadjustElements(object sender, EventArgs e)
        {
            Name.Position = new Vector2(PositionRectangle.Center.X - Name.Width / 2, PositionRectangle.Top + 16);
            Character.Position = new Vector2(PositionRectangle.Center.X - Character.Width / 2, PositionRectangle.Bottom - Character.Height - 48);
        }
        #endregion
    }
}
