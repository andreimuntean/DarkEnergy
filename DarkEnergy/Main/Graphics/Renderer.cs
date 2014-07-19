using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace DarkEnergy
{
    public class Renderer
    {
        private GraphicsDevice graphicsDevice;
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;

        public bool IsActivated { get; set; }

        public bool IsRendering { get; private set; }

        public bool IsVerticalSynchronizationEnabled
        {
            get { return graphicsDeviceManager.SynchronizeWithVerticalRetrace; }
            set { graphicsDeviceManager.SynchronizeWithVerticalRetrace = value; }
        }

        public Vector2 Scaling { get; protected set; }

        public Renderer(DarkEnergyGame game)
        {
            graphicsDeviceManager = new GraphicsDeviceManager(game);
        }

        public void SetResolution(Vector2 resolution)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = (int)resolution.X;
            graphicsDeviceManager.PreferredBackBufferHeight = (int)resolution.Y;
            Scaling = new Vector2(resolution.X / Screen.NativeResolution.X, resolution.Y / Screen.NativeResolution.Y);
        }

        public void Initialize(DarkEnergyGame game)
        {
            this.graphicsDevice = game.GraphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);
            IsVerticalSynchronizationEnabled = true;
            IsActivated = false;
        }

        public void Begin()
        {
            IsRendering = true;

            try
            {
                graphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, graphicsDevice.BlendStates.NonPremultiplied);
            }
            catch
            {
                ExceptionManager.Log("Some super rare glitch occurred while starting to draw. 'Twas handled though.");
            }
        }

        public void Draw(TexturedElement entity)
        {
            if (entity.Visible)
            {
                try
                {
                    // Parses the data to the right format.
                    int x = (int)(entity.X * Scaling.X);
                    int y = (int)(entity.Y * Scaling.Y);
                    int width = (int)(entity.Width * Scaling.X);
                    int height = (int)(entity.Height * Scaling.Y);
                    var color = new Color(entity.ColorIntensity.Red, entity.ColorIntensity.Green, entity.ColorIntensity.Blue, entity.Opacity);

                    spriteBatch.Draw(entity.Texture, new Rectangle(x, y, width, height), entity.SpriteRectangle, color, (float)(entity.Rotation * 2 * System.Math.PI), Vector2.Zero, SpriteEffects.None, 0);
                }
                catch
                {
                    // Just. In. Case.
                    // There is an extremely rare chance that a null asset will be
                    // drawn and cause an error. Extremely rare errors typically occur
                    // while the product is being presented in front of an audience.
                    // Not on my watch.
                    ExceptionManager.Log("Some super rare glitch occurred while drawing an entity. 'Twas handled though.");
                }
            }
        }

        public void Draw(Text text)
        {
            if (text.Visible)
            {
                Rectangle positionRectangle = text.PositionRectangle;
                try
                {
                    // Parses the data to the right format.
                    int x = (int)(text.X * Scaling.X);
                    int y = (int)(text.Y * Scaling.Y);
                    Vector2 scale = text.Scale * Scaling.Y;
                    
                    spriteBatch.DrawString(text.Font, text.String, new Vector2(x, y), new Color(text.Color.R, text.Color.G, text.Color.B, (byte)(text.Opacity * 255)), (float)(text.Rotation * 2 * System.Math.PI), Vector2.Zero, scale, SpriteEffects.None, 1);
                }
                catch
                {
                    // Just. In. Case.
                    // There is an extremely rare chance that a null asset will be
                    // drawn and cause an error. Extremely rare errors typically occur
                    // while the product is being presented in front of an audience.
                    // Not on my watch.
                    ExceptionManager.Log("Some super rare glitch occurred while drawing some text. 'Twas handled though.");
                }
            }
        }

        public void End()
        {
            try
            {
                spriteBatch.End();
            }
            catch
            {
                ExceptionManager.Log("Some super rare glitch occurred while finishing the drawing. 'Twas handled though.");
            }

            IsRendering = false;
        }

        public void UnloadContent()
        {
            graphicsDeviceManager.Dispose();
            spriteBatch.Dispose();
        }
    }
}
