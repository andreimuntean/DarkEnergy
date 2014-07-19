using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.World.Objects;

namespace DarkEnergy.Scenes.World.Westhill
{
    public class GeneralStore : WorldScene
    {
        protected TexturedElement background;

        public GeneralStore()
        {
            background = new TexturedElement(1280, 720, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this, Path = @"World\Scenes\Westhill\GeneralStore.dds" };
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            base.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        public override void SaveData()
        {
            DataStorageManager.SaveCharacterLocation<GeneralStore>();
        }

        public override void OnBackKeyPress()
        {
            SceneManager.Play(new Village());
        }
    }
}
