using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    public class GameSystem : IGame
    {
        public event EventHandler Loaded;
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
                if (green > 1) green = 1;
                if (blue > 1) blue = 1;

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
                    return new Vector2(scale.X * Parent.Scale.X, scale.Y * Parent.Scale.Y);
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

        public GameSystem() { Parent = null; }

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

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(Renderer renderer) { }

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

        protected virtual void OnSizeChanged(EventArgs e)
        {
            if (SizeChanged != null)
            {
                SizeChanged(this, e);
            }
        }
        #endregion
    }
}
