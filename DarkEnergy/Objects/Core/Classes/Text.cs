using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace DarkEnergy
{
    static class FontStyle
    {
        public static string Calibri16 { get { return "Calibri16"; } }
        public static string Calibri18 { get { return "Calibri18"; } }
        public static string Calibri20 { get { return "Calibri20"; } }
        public static string Calibri24 { get { return "Calibri24"; } }
        public static string Calibri28 { get { return "Calibri28"; } }
        public static string Calibri30 { get { return "Calibri30"; } }
        public static string Calibri32 { get { return "Calibri32"; } }
        public static string CenturyGothic16 { get { return "CenturyGothic16"; } }
        public static string CenturyGothic20 { get { return "CenturyGothic20"; } }
        public static string CenturyGothic24 { get { return "CenturyGothic24"; } }
        public static string CenturyGothic28 { get { return "CenturyGothic28"; } }
        public static string CenturyGothic32 { get { return "CenturyGothic32"; } }
        public static string CenturyGothic40 { get { return "CenturyGothic40"; } }
        public static string Consolas18 { get { return "Consolas18"; } }
        public static string SegoeWP08 { get { return "SegoeWP08"; } }
        public static string SegoeWP16 { get { return "SegoeWP16"; } }
        public static string SegoeWP24 { get { return "SegoeWP24"; } }
        public static string SegoeWP28 { get { return "SegoeWP28"; } }
        public static string SegoeWP32 { get { return "SegoeWP32"; } }
        public static string SegoeWP40 { get { return "SegoeWP40"; } }
        public static string SegoeWP44 { get { return "SegoeWP44"; } }
        public static string SegoeWP64 { get { return "SegoeWP64"; } }
    }

    public class Text : GameObject
    {
        public SpriteFont Font { get; private set; }

        private string text;
        public string String
        {
            get { return text; }
            set
            {
                if (IsLoaded && MaxWidth > 0) // If a width limit is specified.
                {
                    #region Separate the string into lines.
                    List<string> lines = new List<string>() { "" };
                    var result = value.Replace(@"\n", "\n");
                    var words = result.Split(' ');

                    for (var i = 0; i < words.Length; ++i)
                    {
                        var pixelWidth = MeasureString(lines[lines.Count - 1] + words[i] + " ").X;

                        if (pixelWidth >= MaxWidth)
                        {
                            lines.Add("");
                        }

                        lines[lines.Count - 1] += words[i] + " ";
                    }

                    result = "";

                    foreach (var x in lines)
                    {
                        result += x.TrimEnd() + "\n";
                    }

                    text = result.TrimEnd('\n');
                    #endregion
                }
                else
                {
                    text = value;
                }

                OnSizeChanged(EventArgs.Empty);
            }
        }
        
        public Color Color { get; set; }

        public string Style { get; private set; }

        public override Vector2 Dimensions
        {
            get
            {
                return MeasureString(String);
            }
        }

        public override float Height { get { return Dimensions.Y; } }

        public override float Width { get { return Dimensions.X; } }

        public float MaxWidth { get; set; }

        public Text(string style) : base()
        {
            Color = Color.White;
            Style = style;
            text = "";
        }

        public Text(string style, Vector2 position) : base(position)
        {
            Color = Color.White;
            Style = style;
            text = "";
        }

        public Text(string style, float x, float y) : base(x, y)
        {
            Color = Color.White;
            Style = style;
            text = "";
        }

        public Text(string style, HorizontalAlignment horizontalAlignment, float horizontalOffset, float y) : base(horizontalAlignment, horizontalOffset, y)
        {
            Color = Color.White;
            Style = style;
            text = "";
        }

        public Text(string style, float x, VerticalAlignment verticalAlignment, float verticalOffset) : base(x, verticalAlignment, verticalOffset)
        {
            Color = Color.White;
            Style = style;
            text = "";
        }

        public Text(string style, HorizontalAlignment horizontalAlignment, float horizontalOffset, VerticalAlignment verticalAlignment, float verticalOffset) : base(horizontalAlignment, horizontalOffset, verticalAlignment, verticalOffset)
        {
            Color = Color.White;
            Style = style;
            text = "";
        }

        public Vector2 MeasureString(string value)
        {
            if (IsLoaded)
            {
                return Font.MeasureString(value) * Scale.X;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Font = contentManager.Load<SpriteFont>("Fonts/" + Style);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(Renderer renderer)
        {
            if (Visible == true && Scale.X > 0.0f && Scale.Y > 0.0f) renderer.Draw(this);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload<SpriteFont>("Fonts/" + Style);
            base.UnloadContent(contentManager);
        }
    }
}
