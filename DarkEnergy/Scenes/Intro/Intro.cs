using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes
{
    public class Intro : GameSystem, IScene
    {
        private Text message;
        private bool isChangingScene;
        private double timeWaited;

        public Intro()
        {
            message = new Text(FontStyle.SegoeWP32, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0);
        }

        public override void Initialize()
        {
            base.Initialize();
            message.Initialize();

            message.Opacity = 0;
            message.String = Resources.Strings.Intro_Message;

            timeWaited = 0;
            isChangingScene = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            message.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isChangingScene)
            {
                if (timeWaited < 1.3)
                {
                    timeWaited += gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (message.Opacity < 1.0f)
                {
                    message.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                TouchManager.OnTap(() => isChangingScene = true);
            }
            else if (message.Opacity > 0.0f)
            {
                message.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                SceneManager.Play(new Scenes.AcolyteDialogue());
            }
        }

        public override void Draw(Renderer renderer)
        {
            message.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            message.LoadContent(contentManager);
        }

        public void OnBackKeyPress()
        {
            SceneManager.Play(new MainMenu());
        }
    }
}
