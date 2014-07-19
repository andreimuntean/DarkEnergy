using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes.Main_Menu
{
    public class SelectionMenu : GameSystem
    {
        private bool menuCyclingContinues;
        private bool selectionChanged;
        private bool isFadingOut;
        private byte defaultSelection;
        private CalligraphedImage[] items;

        public Color FontColor { get; private set; }
        public string FontStyle { get; private set; }
        public float Speed { get; set; }
        public float Expansion { get; set; }
        public Vector2 Offset { get; set; }
        public byte SelectedOption { get; private set; }
        public bool GoToSelectedOption { get; private set; }

        public SelectionMenu(string[] options, byte defaultSelection)
        {
            if (options.Length < 1 || (defaultSelection < 0 || defaultSelection >= options.Length)) throw new ArgumentException();

            FontColor = Color.White;
            FontStyle = DarkEnergy.FontStyle.SegoeWP32;
            Speed = 1.46f;
            Expansion = 1.0f;
            Offset = Vector2.Zero;
            SelectedOption = this.defaultSelection = defaultSelection;
            GoToSelectedOption = false;
            isFadingOut = false;

            items = new CalligraphedImage[options.Length];
            for (byte i = 0; i < options.Length; ++i)
            {
                items[i] = new CalligraphedImage(528, 87, Vector2.Zero, options[i], FontStyle, FontColor, new Vector2(-2, -2)) { Path = "Interface/HighlightPurple.dds", Parent = this };
            }
        }

        #region Secondary Methods
        private float calculateX(float y)
        {
            float y2 = (float)Math.Pow(y - Offset.Y, 2);
            return (-1.0f / 960) * y2 + Offset.X;
        }

        private Vector2 calculateScale(float x)
        {
            float values = ((x - Offset.X) + 320.0f) / 320.0f;
            return new Vector2(values, values);
        }

        private float calculateOpacity(float x)
        {
            float x3 = (float)Math.Pow(x - Offset.X, 3);
            return (x3 + 80000) / 80000;
        }

        private void generateYCoordinates(ref float[] y)
        {
            float distance = 160f;
            float temp = -distance;
            y[SelectedOption] = 0.0f;
            for (int i = SelectedOption - 1; i >= 0; --i)
            {
                y[i] = temp;
                temp -= distance;
            }

            temp = distance;
            for (int i = SelectedOption + 1; i < y.Length; ++i)
            {
                y[i] = temp;
                temp += distance;
            }

            for (int i = 0; i < y.Length; ++i)
            {
                y[i] += Offset.Y;
            }
        }

        private void select(byte option)
        {
            foreach (var item in items) item.Layer = 0;
            items[option].Layer = 1;
            SelectedOption = option;
            selectionChanged = true;
        }

        private void move(float speed, ref float position, float target)
        {
            if (Speed > 0.0f) speed *= Speed;
            if (position < target) if (position + speed > target) position = target; else position += speed;
            else if (position > target) if (position - speed < target) position = target; else position -= speed;
        }

        private void cycleMenu(GameTime gameTime, ref float[] y, ref bool menuIsCycling)
        {
            float[] x = new float[y.Length];
            float[] target = new float[y.Length];
            float speed = (float)(900 * gameTime.ElapsedGameTime.TotalSeconds);

            generateYCoordinates(ref target);

            menuIsCycling = false;
            for (byte i = 0; i < x.Length; ++i)
            {
                move(speed, ref y[i], target[i]);
                x[i] = calculateX(y[i]);
                items[i].Scale = calculateScale(x[i]);
                items[i].Opacity = calculateOpacity(x[i]);
                if (y[i] != target[i]) menuIsCycling = true;
            }
        }

        private void touchEvents()
        {
            TouchManager.OnTap(() =>
            {
                float deviation = 60;
                float center = Screen.NativeResolution.Y / 2 + Offset.Y;
                
                if (TouchManager.Position.Y < center - deviation)
                {
                    // Scroll up.
                    if (SelectedOption > 0) select((byte)(SelectedOption - 1));
                }
                else if (TouchManager.Position.Y >= center + deviation)
                {
                    // Scroll down.
                    if (SelectedOption < items.Length - 1) select((byte)(SelectedOption + 1));
                }
                else
                {
                    TouchManager.Enabled = false;
                }
            });
        }

        private void fadeOut()
        {
            TouchManager.Enabled = false;
            isFadingOut = true;
        }
        #endregion

        public void Reactivate()
        {
            TouchManager.Enabled = true;
            isFadingOut = false;
            GoToSelectedOption = false;
            Opacity = 1;
        }

        public override void Initialize()
        {
            float[] x = new float[items.Length];
            float[] y = new float[items.Length];
            float highlightHeight = 0.0f;
            float highlightWidth = 0.0f;

            base.Initialize();
            generateYCoordinates(ref y);
            for (byte i = 0; i < items.Length; ++i)
            {
                x[i] = calculateX(y[i]);
                items[i].Initialize();
                items[i].HorizontalAlignment = HorizontalAlignment.Left;
                items[i].VerticalAlignment = VerticalAlignment.Middle;
                items[i].TouchBoundaries = new RectangleF(highlightWidth, highlightHeight, highlightWidth, highlightHeight);
                items[i].Offset = new Vector2(x[i] + 110.0f, y[i]);
                items[i].Scale = calculateScale(x[i]);
                items[i].Opacity = calculateOpacity(x[i]);
            }

            select(defaultSelection);
            TouchManager.Enabled = true;
            menuCyclingContinues = true;
            items[defaultSelection].Layer = 1;
            Visible = true;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            foreach (var item in items) item.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (TouchManager.Enabled)
            {
                touchEvents();
            }
            else if (isFadingOut == false)
            {
                fadeOut();
            }

            if (isFadingOut)
            {
                if (Opacity == 0)
                {
                    GoToSelectedOption = true;
                    isFadingOut = false;
                    TouchManager.Enabled = true;
                }
                else
                {
                    Opacity -= 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (selectionChanged)
            {
                float[] y = new float[items.Length];

                for (byte i = 0; i < items.Length; ++i)
                {
                    y[i] = items[i].Offset.Y;
                }

                cycleMenu(gameTime, ref y, ref menuCyclingContinues);

                if (menuCyclingContinues == false) selectionChanged = false;

                for (byte i = 0; i < items.Length; ++i)
                {
                    items[i].Offset = new Vector2(calculateX(y[i]), y[i]);
                }
            }
            foreach (var item in items) item.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            foreach (var item in items) item.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            foreach (var item in items) item.UnloadContent(contentManager);
        }
    }
}
