using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Windows.UI.Core;
using SharpDX;

namespace DarkEnergy
{
    public partial class Screen : PhoneApplicationPage
    {
        public static Vector2 NativeResolution { get { return new Vector2(1280, 720); } }

        public bool IsTapped { get { return TouchManager.IsTapped; } }
        public Vector2 Scaling { get; private set; }
        public Vector2 Resolution { get; private set; }

        public Screen()
        {
            InitializeComponent();
            Initialize();
            App.Game.SetScreen(this);
            App.Game.Run(Surface);
        }

        public void Initialize()
        {
            switch (App.Current.Host.Content.ScaleFactor)
            {
                case 100: Resolution = new Vector2(800, 480);
                    break;

                case 150: Resolution = new Vector2(1280, 720);
                    break;

                case 160: Resolution = new Vector2(1200, 675);
                    Surface.Height = 450;
                    break;
            }

            SplashScreen.Visibility = Visibility.Visible;
            Scaling = new Vector2(Resolution.X / NativeResolution.X, Resolution.Y / NativeResolution.Y);
        }

        private void Surface_Loaded(object sender, RoutedEventArgs e)
        {
            SplashScreen.Visibility = Visibility.Collapsed;
        }

        private void Surface_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            double widthScalingFactor = Surface.ActualWidth / Resolution.X;
            double heightScalingFactor = Surface.ActualHeight / Resolution.Y;
            
            float x = (float)(e.ManipulationOrigin.X / widthScalingFactor);
            float y = (float)(e.ManipulationOrigin.Y / heightScalingFactor);

            TouchManager.Tap(new Vector2(x, y));
        }

        private void Surface_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            double widthScalingFactor = Surface.ActualWidth / Resolution.X;
            double heightScalingFactor = Surface.ActualHeight / Resolution.Y;

            float x = (float)(e.DeltaManipulation.Translation.X / widthScalingFactor);
            float y = (float)(e.DeltaManipulation.Translation.Y / heightScalingFactor);

            TouchManager.Drag(new Vector2(x, y));
        }

        private void Surface_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            float x = (float)(e.TotalManipulation.Translation.X);
            float y = (float)(e.TotalManipulation.Translation.Y);

            TouchManager.Release(new Vector2(x, y));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            SceneManager.GoBack();
        }
    }
}