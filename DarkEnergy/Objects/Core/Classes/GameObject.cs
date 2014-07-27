using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    public enum HorizontalAlignment { None, Left, Center, Right }
    public enum VerticalAlignment { None, Top, Middle, Bottom }

    public abstract class GameObject : IGame
    {
        public event EventHandler Loaded;
        public event EventHandler PositionChanged;
        public event EventHandler SizeChanged;

        public IGame Parent { get; set; }

        public bool IsColorIntensityIndependent { get; set; }
        public bool IsOpacityIndependent { get; set; }
        public bool IsRotationIndependent { get; set; }
        public bool IsScaleIndependent { get; set; }
        public bool IsVisibilityIndependent { get; set; }

        public bool IsLoaded { get; private set; }

        private bool visible;
        public bool Visible
        {
            get
            {
                if (!IsVisibilityIndependent && Parent != null && !Parent.Visible)
                {
                    return false;
                }
                return visible;
            } 
            set
            {
                visible = value;
            }
        }

        public HorizontalAlignment HorizontalAlignment { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }

        protected float opacity;
        public float Opacity
        {
            get
            {
                if (!IsOpacityIndependent && Parent != null)
                {
                    return opacity * Parent.Opacity;
                }
                return opacity;
            }
            set
            {
                if (value < 0.0f)
                {
                    opacity = 0.0f;
                }
                else if (value > 1.0f)
                {
                    opacity = 1.0f;
                }
                else
                {
                    opacity = value;
                }
            }
        }

        protected Color3 colorIntensity;
        public Color3 ColorIntensity
        {
            get
            {
                if (!IsColorIntensityIndependent && Parent != null)
                {
                    return colorIntensity * Parent.ColorIntensity;
                }
                return colorIntensity;
            }
            set
            {
                var red = value.Red;
                var green = value.Green;
                var blue = value.Blue;

                if (red > 1) red = 1;
                else if (red < 0) red = 0;
                if (green > 1) green = 1;
                else if (green < 0) green = 0;
                if (blue > 1) blue = 1;
                else if (blue < 0) blue = 0;

                colorIntensity = new Color3(red, green, blue);
            }
        }

        protected float rotation;
        public float Rotation
        {
            get
            {
                if (!IsRotationIndependent && Parent != null)
                {
                    float result = rotation + Parent.Rotation;
                    return (float)(result - System.Math.Truncate(result));
                }
                return rotation;
            }
            set
            {
                rotation = (float)(value - System.Math.Truncate(value));
            }
        }

        private Vector2 scale;
        public virtual Vector2 Scale
        {
            get
            {
                if (!IsScaleIndependent && Parent != null)
                {
                    return scale * Parent.Scale;
                }
                return scale;
            }
            set
            {
                if (value.X < 0.0f) value.X = 0.0f;
                if (value.Y < 0.0f) value.Y = 0.0f;
                scale = value;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        private float absoluteX;
        private float x
        {
            get { return absoluteX; }
            set { absoluteX = value; OnPositionChanged(EventArgs.Empty); }
        }
        public virtual float X
        {
            get
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left: return 0 + x;
                    case HorizontalAlignment.Center: return (Screen.NativeResolution.X - Width) / 2 + x;
                    case HorizontalAlignment.Right: return Screen.NativeResolution.X - Width + x;
                }
                return x;
            }
        }

        private float absoluteY;
        private float y
        {
            get { return absoluteY; }
            set { absoluteY = value; OnPositionChanged(EventArgs.Empty); }
        }
        public virtual float Y
        {
            get
            {
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top: return 0 + y;
                    case VerticalAlignment.Middle: return (Screen.NativeResolution.Y - Height) / 2 + y;
                    case VerticalAlignment.Bottom: return Screen.NativeResolution.Y - Height + y;
                }
                return y;
            }
        }

        public Rectangle PositionRectangle { get { return new Rectangle((int)X, (int)Y, (int)Width, (int)Height); } }

        /// <summary>
        /// Gets or sets the offset of this GameObject on the screen. If the object is not horizontally (or vertically) aligned, the X (or Y) offset coordinate cannot be changed and will return 0.
        /// Use the Position property instead.
        /// </summary>
        public Vector2 Offset
        {
            get
            {
                float offsetX = (HorizontalAlignment != HorizontalAlignment.None)? x : 0;
                float offsetY = (VerticalAlignment != VerticalAlignment.None)? y : 0;
                return new Vector2(offsetX, offsetY);
            }
            set
            {
                if (HorizontalAlignment != HorizontalAlignment.None) x = value.X;
                if (VerticalAlignment != VerticalAlignment.None) y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the position of this GameObject on the screen. If the object is horizontally (or vertically) aligned, the X (or Y) coordinate cannot be changed and will return 0.
        /// Use the Offset property instead.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                float X = (HorizontalAlignment == HorizontalAlignment.None)? this.x : 0;
                float Y = (VerticalAlignment == VerticalAlignment.None)? this.y : 0;
                return new Vector2(X, Y);
            }
            set
            {
                if (HorizontalAlignment == HorizontalAlignment.None) x = value.X;
                if (VerticalAlignment == VerticalAlignment.None) y = value.Y;
            }
        }

        public virtual float Height { get { return 0; } }

        public virtual float Width { get { return 0; } }

        public virtual Vector2 Dimensions { get { return new Vector2(Width, Height); } }

        public GameObject() { }

        public GameObject(Vector2 position)
        {
            absoluteX = position.X;
            absoluteY = position.Y;
        }

        public GameObject(float x, float y)
        {
            absoluteX = x;
            absoluteY = y;
        }

        public GameObject(HorizontalAlignment horizontalAlignment, float horizontalOffset, float y)
        {
            this.HorizontalAlignment = horizontalAlignment;
            absoluteX = horizontalOffset;
            absoluteY = y;
        }

        public GameObject(float x, VerticalAlignment verticalAlignment, float verticalOffset)
        {
            this.VerticalAlignment = verticalAlignment;
            absoluteX = x;
            absoluteY = verticalOffset;
        }

        public GameObject(HorizontalAlignment horizontalAlignment, float horizontalOffset, VerticalAlignment verticalAlignment, float verticalOffset)
        {
            this.HorizontalAlignment = horizontalAlignment;
            this.VerticalAlignment = verticalAlignment;
            absoluteX = horizontalOffset;
            absoluteY = verticalOffset;
        }

        public virtual void Initialize()
        {
            colorIntensity = Color3.White;
            opacity = 1.0f;
            rotation = 0.0f;
            scale = Vector2.One;
            visible = true;
            IsColorIntensityIndependent = false;
            IsOpacityIndependent = false;
            IsRotationIndependent = false;
            IsScaleIndependent = false;
            IsVisibilityIndependent = false;
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            IsLoaded = true;
            OnLoaded(EventArgs.Empty);
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(Renderer renderer);

        public virtual void UnloadContent(ContentManager contentManager)
        {
            IsLoaded = false;
        }

        #region Events
        protected virtual void OnLoaded(EventArgs e)
        {
            if (Loaded != null)
            {
                Loaded(this, e);
            }
        }

        protected virtual void OnPositionChanged(EventArgs e)
        {
            // "If the event is listening and content is loaded ..."
            if (PositionChanged != null && IsLoaded)
            {
                PositionChanged(this, e);
            }
        }

        protected virtual void OnSizeChanged(EventArgs e)
        {
            // "If the event is listening and content is loaded ..."
            if (SizeChanged != null && IsLoaded)
            {
                SizeChanged(this, e);
            }
        }
        #endregion
    }
}
