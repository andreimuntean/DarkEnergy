using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    public interface IGame
    {
        event EventHandler Loaded;

        bool IsLoaded { get; }
        bool Visible { get; set; }
        Color3 ColorIntensity { get; set; }
        float Opacity { get; set; }
        float Rotation { get; set; }
        IGame Parent { get; set; }
        Vector2 Scale { get; set; }

        void Initialize();
        void LoadContent(ContentManager contentManager);
        void Update(GameTime gameTime);
        void Draw(Renderer renderer);
        void UnloadContent(ContentManager contentManager);
    }
}
