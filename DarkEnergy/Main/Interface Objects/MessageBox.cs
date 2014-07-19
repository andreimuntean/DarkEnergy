using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    public class MessageBox : TexturedElement
    {
        protected int itemCount;
        protected List<CalligraphedImage> buttons;
        protected Text message;

        public string Message { get { return message.String; } set { message.String = value; } }

        public string Result { get; set; }

        public MessageBox() : base(920, 520, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0)
        {
            Path = @"Interface\MessageBox.dds";
            message = new Text(FontStyle.SegoeWP32, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this };
            buttons = new List<CalligraphedImage>();

            for (var i = 0; i < 2; ++i)
            {
                buttons.Add(new CalligraphedImage(316, 80, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0, "", FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Path = "Interface/DefaultButton.dds", Parent = this });
            }

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
            SizeChanged += AdjustElements;
        }

        public void Hide()
        {
            Result = "";
            Visible = false;
        }

        public void Show(string body, string[] items)
        {
            Message = body;
            itemCount = items.Length;
            AdjustElements(null, EventArgs.Empty);

            for (var i = 0; i < buttons.Count; ++i)
            {
                if (i < itemCount)
                {
                    buttons[i].String = items[i];
                    buttons[i].Visible = true;
                }
                else buttons[i].Visible = false;
            }

            Visible = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            foreach (var button in buttons) button.Initialize();
            message.Initialize();
            Hide();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            foreach (var button in buttons) button.LoadContent(contentManager);
            message.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                foreach (var button in buttons)
                {
                    if (button.Tapped)
                    {
                        Result = button.String;
                    }
                }

                if (Result == "")
                {
                    TouchManager.OnTap(() => PhoneEffectsManager.Play(PhoneEffect.Vibration));
                }

                // Disables touch events for other objects for as long as
                // this MessageBox is shown.
                TouchManager.StopHandlingTap();
            }
        }

        public override void Draw(Renderer renderer)
        {
            if (Visible)
            {
                base.Draw(renderer);
                message.Draw(renderer);
                foreach (var button in buttons) button.Draw(renderer);
            }
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            message.UnloadContent(contentManager);
            foreach (var button in buttons) button.UnloadContent(contentManager);
        }

        private void AdjustElements(object sender, EventArgs e)
        {
            var y = PositionRectangle.Bottom - PositionRectangle.Center.Y - buttons[0].Height / 2 - 28.8f;

            if (itemCount == 1)
            {
                buttons[0].Offset = new Vector2(0, y);
            }
            else if (itemCount == 2)
            {
                float x = PositionRectangle.Center.X - PositionRectangle.Left;
                float modifier = buttons[0].Width / 2 + 28.8f;
                x -= modifier;
                
                buttons[0].Offset = new Vector2(-x, y);
                buttons[1].Offset = new Vector2(x, y);
            }
        }
    }
}
