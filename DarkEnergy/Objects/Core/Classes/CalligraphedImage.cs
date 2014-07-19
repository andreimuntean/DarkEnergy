using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    public class CalligraphedImage : TexturedElement
    {
        private Text text;

        public Color FontColor { get { return text.Color; } set { text.Color = value; } }
        public string FontStyle { get { return text.Style; } }
        public string String { get { return text.String; } set { text.String = value; } }
        public Vector2 TextOffset { get; private set; }

        #region Constructors
        public CalligraphedImage(int textureWidth, int textureHeight) : base(textureWidth, textureHeight)
        {
            this.text = new Text(DarkEnergy.FontStyle.SegoeWP32, HorizontalAlignment.None, 0, VerticalAlignment.None, 0);
            this.text.Parent = this;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, Vector2 position) : base(textureWidth, textureHeight, position)
        {
            this.text = new Text(DarkEnergy.FontStyle.SegoeWP32, HorizontalAlignment.None, 0, VerticalAlignment.None, 0);
            this.text.Parent = this;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, string text, string fontStyle) : base(textureWidth, textureHeight)
        {
            this.text = new Text(fontStyle, HorizontalAlignment.None, 0, VerticalAlignment.None, 0) { Color = Color.White, String = text };
            this.text.Parent = this;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, string text, string fontStyle, Color fontColor, Vector2 textOffset) : base(textureWidth, textureHeight)
        {
            this.text = new Text(fontStyle, HorizontalAlignment.None, 0, VerticalAlignment.None, 0) { Color = fontColor, String = text };
            this.text.Parent = this;
            this.TextOffset = textOffset;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, Vector2 position, string text, string fontStyle) : base(textureWidth, textureHeight, position)
        {
            this.text = new Text(fontStyle, HorizontalAlignment.None, 0, VerticalAlignment.None, 0) { Color = Color.White, String = text };
            this.text.Parent = this;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, Vector2 position, string text, string fontStyle, Color fontColor, Vector2 textOffset) : base(textureWidth, textureHeight, position)
        {
            this.text = new Text(fontStyle, HorizontalAlignment.None, 0, VerticalAlignment.None, 0) { Color = fontColor, String = text };
            this.text.Parent = this;
            this.TextOffset = textOffset;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, float x, VerticalAlignment verticalAlignment, float verticalOffset, string text, string fontStyle, Color fontColor, Vector2 textOffset) : base(textureWidth, textureHeight, x, verticalAlignment, verticalOffset)
        {
            this.text = new Text(fontStyle, HorizontalAlignment.None, 0, VerticalAlignment.None, 0) { Color = fontColor, String = text };
            this.text.Parent = this;
            this.TextOffset = textOffset;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, HorizontalAlignment horizontalAlignment, float horizontalOffset, float y, string text, string fontStyle, Color fontColor, Vector2 textOffset) : base(textureWidth, textureHeight, horizontalAlignment, horizontalOffset, y)
        {
            this.text = new Text(fontStyle, HorizontalAlignment.None, 0, VerticalAlignment.None, 0) { Color = fontColor, String = text };
            this.text.Parent = this;
            this.TextOffset = textOffset;
            assignEvents();
        }

        public CalligraphedImage(int textureWidth, int textureHeight, HorizontalAlignment horizontalAlignment, float horizontalOffset, VerticalAlignment verticalAlignment, float verticalOffset, string text, string fontStyle, Color fontColor, Vector2 textOffset) : base(textureWidth, textureHeight, horizontalAlignment, horizontalOffset, verticalAlignment, verticalOffset)
        {
            this.text = new Text(fontStyle, HorizontalAlignment.None, 0, VerticalAlignment.None, 0) { Color = fontColor, String = text };
            this.text.Parent = this;
            this.TextOffset = textOffset;
            assignEvents();
        }
        #endregion

        private void assignEvents()
        {
            Loaded += Readjust;
            PositionChanged += Readjust;
            text.SizeChanged += Readjust;
            SizeChanged += Readjust;
        }

        public Text GetText()
        {
            return text;
        }

        public override void Initialize()
        {
            text.Initialize();
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            text.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            text.Update(gameTime);
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

        #region Events
        private void Readjust(object sender, EventArgs e)
        {
            Vector2 center = PositionRectangle.Center;
            text.Position = new Vector2(center.X - text.Width / 2 + TextOffset.X, center.Y - text.Height / 2 + TextOffset.Y);
        }
        #endregion
    }
}
