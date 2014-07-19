using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    public class Keyboard : TexturedElement
    {
        protected bool fadingOut;
        protected List<CalligraphedImage> keys;
        protected TexturedElement backspace, enter;
        protected Text text;
        protected Text target;

        public bool IsUpperCase { get; protected set; }
        public string Message { get { return text.String; } protected set { target.String = text.String = value; } }
        public int Length { get { return Message.Length; } }
        public new Vector2 Offset { get { return base.Offset; } protected set { base.Offset = value; } }

        public Keyboard() : base(1280, 485, HorizontalAlignment.Center, 0, VerticalAlignment.Bottom, 0)
        {
            fadingOut = false;
            Path = "Interface/KeyboardBackground.dds";
            
            var keyValues = new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
            "A", "S", "D", "F", "G", "H", "J", "K", "L",
            "Z", "X", "C", "V", "B", "N", "M" };

            keys = new List<CalligraphedImage>();

            foreach (var value in keyValues)
            {
                keys.Add(new CalligraphedImage(117, 115, value, FontStyle.SegoeWP32, Color.White, new Vector2(-2, -2)) { Path = "Interface/KeyboardButton.dds", Parent = this });
            }

            text = new Text(FontStyle.SegoeWP32) { Parent = this, Color = Color.Black };
            backspace = new TexturedElement(244, 105) { Parent = this, Path = "Interface/KeyboardBackspace.dds" };
            enter = new TexturedElement(244, 115) { Parent = this, Path = "Interface/KeyboardEnter.dds" };

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
            SizeChanged += AdjustElements;
        }

        public void Hide()
        {
            fadingOut = true;
        }

        public void Show(Text target)
        {
            this.target = target;
            fadingOut = false;
            Visible = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            foreach (var button in keys) button.Initialize();
            text.Initialize();
            backspace.Initialize();
            enter.Initialize();

            Offset = new Vector2(0, Height);
            Visible = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            foreach (var button in keys) button.LoadContent(contentManager);
            text.LoadContent(contentManager);
            backspace.LoadContent(contentManager);
            enter.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                if (fadingOut)
                {
                    float modifier = (float)gameTime.ElapsedGameTime.TotalSeconds * Height * 7;

                    if (Offset.Y + modifier < Height) Offset = new Vector2(0, Offset.Y + modifier);
                    else
                    {
                        Offset = new Vector2(0, Height);
                        Visible = false;
                    }

                    return;
                }
                else if (Offset.Y > 0)
                {
                    float modifier = (float)gameTime.ElapsedGameTime.TotalSeconds * Height * 7;

                    if (Offset.Y - modifier > 0) Offset = new Vector2(0, Offset.Y - modifier);
                    else Offset = Vector2.Zero;
                }

                TouchManager.OnTap(() =>
                {
                    if (this.Tapped == false)
                    {
                        Hide();
                    }
                    else
                    {
                        foreach (var button in keys)
                        {
                            if (button.Tapped)
                            {
                                if (Message.Length < SettingsManager.NameLengthLimit)
                                {
                                    Message += button.String;
                                }
                                break;
                            }
                        }

                        if (backspace.Tapped && Message.Length > 0)
                        {
                            Message = Message.Substring(0, Message.Length - 1);
                        }
                        else if (enter.Tapped)
                        {
                            Hide();
                        }

                        if (Message.Length == 0)
                        {
                            foreach (var key in keys)
                            {
                                key.String = key.String.ToUpper();
                            }
                            IsUpperCase = true;
                        }
                        else
                        {
                            foreach (var key in keys)
                            {
                                key.String = key.String.ToLower();
                            }
                            IsUpperCase = false;
                        }
                    }
                });
            }
        }

        public override void Draw(Renderer renderer)
        {
            if (Visible)
            {
                base.Draw(renderer);
                text.Draw(renderer);
                foreach (var button in keys) button.Draw(renderer);
                backspace.Draw(renderer);
                enter.Draw(renderer);
            }
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            text.UnloadContent(contentManager);
            backspace.UnloadContent(contentManager);
            enter.UnloadContent(contentManager);
            foreach (var button in keys) button.UnloadContent(contentManager);
        }

        private void AdjustElements(object sender, EventArgs e)
        {
            Vector2 origin = PositionRectangle.TopLeft;

            text.Position = origin + new Vector2(35, 28);

            for (var i = 0; i < keys.Count; ++i)
            {
                float x, y;

                if (i < 10) // First row.
                {
                    x = 10 + i * 127;
                    y = 120;
                }
                else if (i < 19) // Second row.
                {
                    x = 73.5f + ((i - 10) % 9) * 127;
                    y = 240;
                }
                else // Third row.
                {
                    x = 137 + ((i - 19) % 7) * 127;
                    y = 360;
                }

                keys[i].Position = origin + new Vector2(x, y);
            }

            backspace.Position = origin + new Vector2(1026, 5);
            enter.Position = origin + new Vector2(1026, 360);
        }
    }
}
