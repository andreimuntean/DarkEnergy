using SharpDX;
using SharpDX.Toolkit;

namespace DarkEnergy.Notifications.Debug_Menu
{
    public class FPSCounter : Text
    {
        public FPSCounter() : base(FontStyle.Consolas18, HorizontalAlignment.Left, 0, VerticalAlignment.Bottom, 0) { }

        public override void Update(GameTime gameTime)
        {
            String = "FPS: " + App.Game.UpdatePerformance.FrameRate.ToString("F2") + " (" + App.Game.DrawPerformance.FrameRate.ToString("F2") + ")";
        }
    }

    public class PointerPosition : Text
    {
        public PointerPosition() : base(FontStyle.Consolas18, HorizontalAlignment.Right, 0, VerticalAlignment.Bottom, 0) { }

        public override void Update(GameTime gameTime)
        {
            String = TouchManager.Position.ToString();
        }
    }
}
