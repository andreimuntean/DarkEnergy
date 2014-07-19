using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes.Battle_End_Screen
{
    public class SimpleNotification : TexturedElement
    {
        private Text text;

        public string Text { get { return text.String; } set { text.String = value; } }

        public SimpleNotification(string style, string text) : base(700, 60)
        {
            this.text = new Text(style) { Parent = this, String = text };
            Path = @"Interface\World\Combat\EndScreen\TextBackground1.dds";

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            text.Position = new Vector2(X + (Width - text.Width) / 2, Y + (Height - text.Height) / 2);
        }

        public override void Initialize()
        {
            base.Initialize();
            text.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            text.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            text.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            text.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }

    public class IconNotification : TexturedElement
    {
        private Text text;
        private TexturedElement icon;

        public string Text { get { return text.String; } set { text.String = value; } }

        public IconNotification(string style, string text, string iconPath) : base(700, 70)
        {
            this.text = new Text(style) { Parent = this, String = text };
            icon = new TexturedElement(60, 60) { Parent = this, Path = iconPath };
            Path = @"Interface\World\Combat\EndScreen\TextBackground2.dds";

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            text.Position = new Vector2(X + 150 + (Width - text.Width) / 2, Y + (Height - text.Height) / 2);
            icon.Position = new Vector2(X - 150 + (Width - icon.Width) / 2, Y + (Height - icon.Height) / 2);
        }

        public override void Initialize()
        {
            base.Initialize();
            text.Initialize();
            icon.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            text.LoadContent(contentManager);
            icon.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            text.Draw(renderer);
            icon.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            text.UnloadContent(contentManager);
            icon.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
