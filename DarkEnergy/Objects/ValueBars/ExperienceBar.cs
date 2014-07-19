using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Interface
{
    public class ExperienceBar : ValueBar
    {
        protected CalligraphedImage left, center, right;

        public ExperienceBar() : base(BarSize.Large, BarColor.Purple, GameManager.Hero.Experience)
        {
            left = new CalligraphedImage(230, 20, new Vector2(X, Y - 25), Resources.Strings.Level + ' ' + GameManager.Hero.Level.ToString(), FontStyle.Calibri28, SharpDX.Color.White, Vector2.Zero) { Parent = this, Path = @"Interface\ValueBar\TextShadow.dds" };
            center = new CalligraphedImage(230, 20, new Vector2(X + 285, Y - 25), Resources.Strings.Experience, FontStyle.Calibri28, SharpDX.Color.White, Vector2.Zero) { Parent = this, Path = @"Interface\ValueBar\TextShadow.dds" };
            right = new CalligraphedImage(230, 20, new Vector2(X + 570, Y - 25), Resources.Strings.Level + ' ' + (GameManager.Hero.Level + 1).ToString(), FontStyle.Calibri28, SharpDX.Color.White, Vector2.Zero) { Parent = this, Path = @"Interface\ValueBar\TextShadow.dds" };
        }

        public override void Initialize()
        {
            base.Initialize();
            left.Initialize();
            center.Initialize();
            right.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            left.LoadContent(contentManager);
            center.LoadContent(contentManager);
            right.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            left.Draw(renderer);
            center.Draw(renderer);
            right.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            left.UnloadContent(contentManager);
            center.UnloadContent(contentManager);
            right.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        protected override void ValueBar_PositionChanged(object sender, System.EventArgs e)
        {
            base.ValueBar_PositionChanged(sender, e);
            left.Position = new Vector2(X - 50, Y - 38);
            center.Position = new Vector2(X + 285, Y - 38);
            right.Position = new Vector2(X + 610, Y - 38);
        }
    }
}
