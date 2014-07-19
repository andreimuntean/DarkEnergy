using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace DarkEnergy
{
    public class TexturedElement : GameObject, ITappable
    {
        /// <summary>
        /// Contains the texture resources of the object.
        /// </summary>
        public Texture Texture { get; protected set; }

        public bool Tapped { get { return Visible && TouchManager.IsTapped && TouchArea.Contains(TouchManager.Position); } }

        private double animationTimeElapsed;
        public float AnimationRate { get; protected set; }

        private int absoluteHeight;
        private int height
        {
            get { return absoluteHeight; }
            set { absoluteHeight = value; OnSizeChanged(EventArgs.Empty); }
        }
        public override float Height { get { return height * Scale.Y; } }

        private int absoluteWidth;
        private int width
        {
            get { return absoluteWidth; }
            set { absoluteWidth = value; OnSizeChanged(EventArgs.Empty); }
        }
        public override float Width { get { return width * Scale.X; } }

        public int Frame { get; set; }

        public int FrameCount { get; set; }

        public int Layer { get; set; }

        public virtual RectangleF TouchArea
        {
            get
            {
                float x = PositionRectangle.Left - TouchBoundaries.Left;
                float y = PositionRectangle.Top - TouchBoundaries.Top;
                float width = PositionRectangle.Right + TouchBoundaries.Width - x;
                float height = PositionRectangle.Bottom + TouchBoundaries.Height - y;
                return new RectangleF(x, y, width, height);
            }
        }

        /// <summary>
        /// Extends the touch area.
        /// </summary>
        public RectangleF TouchBoundaries { get; set; }

        public Rectangle SpriteRectangle
        {
            get
            {
                float a = width * Frame;
                float b = height * Layer;
                return new Rectangle((int)a, (int)b, (int)width, (int)height);
            }
        }

        public string Path { get; set; }

        public TexturedElement(int width, int height) : base() { this.width = width; this.height = height; }

        public TexturedElement(int width, int height, Vector2 position) : base(position) { this.width = width; this.height = height; }

        public TexturedElement(int width, int height, float x, float y) : base(x, y) { this.width = width; this.height = height; }

        public TexturedElement(int width, int height, HorizontalAlignment horizontalAlignment, float horizontalOffset, float y) : base(horizontalAlignment, horizontalOffset, y) { this.width = width; this.height = height; }

        public TexturedElement(int width, int height, float x, VerticalAlignment verticalAlignment, float verticalOffset) : base(x, verticalAlignment, verticalOffset) { this.width = width; this.height = height; }

        public TexturedElement(int width, int height, HorizontalAlignment horizontalAlignment, float horizontalOffset, VerticalAlignment verticalAlignment, float verticalOffset) : base(horizontalAlignment, horizontalOffset, verticalAlignment, verticalOffset) { this.width = width; this.height = height; }

        protected void ChangeNativeSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return TouchArea.Contains(point);
        }

        public override void Initialize()
        {
            base.Initialize();
            Frame = 0;
            FrameCount = 1;
            Layer = 0;
            ColorIntensity = new Color3(1, 1, 1);
            animationTimeElapsed = 0.0d;
            AnimationRate = 0.0f;
            TouchBoundaries = RectangleF.Empty;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>(Path);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (AnimationRate > 0.0f)
            {
                animationTimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                while (animationTimeElapsed > 1.0f / AnimationRate)
                {
                    Frame = (Frame + 1) % (FrameCount);
                    animationTimeElapsed -= 1.0f / AnimationRate; 
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            if (Visible == true && Scale.X > 0.0f && Scale.Y > 0.0f) renderer.Draw(this);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload<Texture>(Path);
            base.UnloadContent(contentManager);
        }
    }
}
