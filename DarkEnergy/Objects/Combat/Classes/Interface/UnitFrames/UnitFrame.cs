using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Interface;

namespace DarkEnergy.Combat.Interface
{
    public class UnitFrame : TexturedElement
    {
        private CalligraphedImage levelIcon;
        private TexturedElement elementIcon;
        private Text name;
        private ValueBar health;
        private ValueBar darkEnergy;

        public string Name
        {
            get { return name.String; }
            set
            {
                name.String = value;
                AdjustElements(null, EventArgs.Empty);
            }
        }

        private Character source;
        public Character Source
        {
            get { return source; }
            set
            {
                source = value;
                Refresh();
            }
        }

        public UnitFrame() : base(380, 100)
        {
            Path = @"Interface\World\Combat\UnitFramesBackground.dds";

            levelIcon = new CalligraphedImage(54, 54, "1", FontStyle.Calibri24, Color.White, new Vector2(1, 0)) { Parent = this, Path = @"Interface\World\Combat\LevelIcon.dds" };
            elementIcon = new TexturedElement(54, 54) { Parent = this, Path = @"Interface\Character\Elements.dds" };
            name = new Text(FontStyle.Calibri24) { String = "Unknown", Parent = this };
            health = new ValueBar(BarSize.Small, BarColor.Green, Quantity.One) { Parent = this };
            darkEnergy = new ValueBar(BarSize.Small, BarColor.Purple, Quantity.One) { Parent = this };

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }
        
        public void Refresh()
        {
            if (source != null)
            {
                levelIcon.String = source.Level.ToString();
                elementIcon.Frame = (int)source.DefensiveElement;
                Name = source.Name;
                health.SetValueTo(source.Health);
                darkEnergy.SetValueTo(source.DarkEnergy);
            }

            AdjustElements(null, EventArgs.Empty);
        }

        public override void Initialize()
        {
            base.Initialize();
            levelIcon.Initialize();
            elementIcon.Initialize();
            name.Initialize();
            health.Initialize();
            darkEnergy.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            levelIcon.LoadContent(contentManager);
            elementIcon.LoadContent(contentManager);
            name.LoadContent(contentManager);
            health.LoadContent(contentManager);
            darkEnergy.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            levelIcon.Draw(renderer);
            elementIcon.Draw(renderer);
            health.Draw(renderer);
            darkEnergy.Draw(renderer);
            name.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            levelIcon.UnloadContent(contentManager);
            elementIcon.UnloadContent(contentManager);
            name.UnloadContent(contentManager);
            health.UnloadContent(contentManager);
            darkEnergy.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            elementIcon.Position = new Vector2(X + Width + 10 - elementIcon.Width, Y - 10);
            levelIcon.Position = new Vector2(X - 10, Y - 10);
            name.Position = new Vector2(X + (Width - name.Width) / 2, Y);
            health.Position = new Vector2(X + 5, Y + 45);
            darkEnergy.Position = new Vector2(X + 5, Y + 75);
        }
    }
}
