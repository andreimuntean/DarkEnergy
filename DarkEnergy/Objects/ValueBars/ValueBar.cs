using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Interface
{
    public enum BarColor { Green, Purple }
    public enum BarSize { Small, Large }

    public class ValueBar : TexturedElement
    {
        protected TexturedElement bar;
        protected TexturedElement foreground;
        protected Text text;
        protected Quantity value;

        public int Current { get { return value.Current; } set { this.value += value; OnValueChanged(); } }

        public int Maximum { get { return value.Maximum; } set { this.value = new Quantity(Current, value); OnValueChanged(); } }

        public BarColor Color { get; protected set; }

        public BarSize Size { get; protected set; }

        public ValueBar(BarSize size, BarColor color, Quantity quantity) : base(0, 0)
        {
            Size = size;
            Color = color;
            value = quantity;
            Path = @"Interface\ValueBar\" + size.ToString() + "Background.dds";

            switch (Size)
            {
                case BarSize.Small: ChangeNativeSize(370, 16);
                    bar = new TexturedElement(367, 14) { Parent = this, Path = @"Interface\ValueBar\" + size.ToString() + color.ToString() + "Bar.dds" };
                    foreground = new TexturedElement(366, 14) { Parent = this, Path = @"Interface\ValueBar\" + size.ToString() + "Foreground.dds" };
                    text = new Text(FontStyle.Calibri24) { Parent = this };
                    break;

                case BarSize.Large: ChangeNativeSize(800, 20);
                    bar = new TexturedElement(797, 18) { Parent = this, Path = @"Interface\ValueBar\" + size.ToString() + color.ToString() + "Bar.dds" };
                    foreground = new TexturedElement(796, 18) { Parent = this, Path = @"Interface\ValueBar\" + size.ToString() + "Foreground.dds" };
                    text = new Text(FontStyle.Calibri28) { Parent = this };
                    break;
            }

            Loaded += ValueBar_Loaded;
            PositionChanged += ValueBar_PositionChanged;
        }

        public void SetValueTo(Quantity value)
        {
            this.value = value;
            OnValueChanged();
        }

        public override void Initialize()
        {
            base.Initialize();
            bar.Initialize();
            foreground.Initialize();
            text.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            bar.LoadContent(contentManager);
            foreground.LoadContent(contentManager);
            text.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            bar.Draw(renderer);
            foreground.Draw(renderer);
            text.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            bar.UnloadContent(contentManager);
            foreground.UnloadContent(contentManager);
            text.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        protected void OnValueChanged()
        {
            if (bar != null && text != null)
            {
                bar.Scale = new Vector2((value.Maximum <= 0) ? 0 : (float)value.Current / (float)value.Maximum, 1);
                text.String = value.ToString();
                ValueBar_PositionChanged(null, System.EventArgs.Empty);
            }
        }

        protected void ValueBar_Loaded(object sender, EventArgs e)
        {
            OnValueChanged();
            ValueBar_PositionChanged(null, EventArgs.Empty);
        }

        protected virtual void ValueBar_PositionChanged(object sender, System.EventArgs e)
        {
            bar.Position = new Vector2(X + 1.5f, Y + 1);
            foreground.Position = new Vector2(X + 2, Y + 1);
            text.Position = new Vector2(X + (Width - text.Width) / 2, (text.Style == FontStyle.Calibri28)? Y - 15 : Y - 12);
        }
    }
}
