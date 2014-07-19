using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.World.Objects;

namespace DarkEnergy.Scenes.World.Westhill
{
    public class Lake : WorldScene
    {
        private Region portal;
        protected TexturedElement background;

        public Lake()
        {
            background = new TexturedElement(1280, 720, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this, Path = @"World\Scenes\Westhill\Lake.dds" };
            portal = new Region(490, 200, 300, 300);
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

            TouchManager.OnTap(portal, () =>
            {
                var location = RandomManager.GetInt(1, 8);
                var enemies = new EnemyList(10000, 10001, 10002, 10003, 10004, 10005, 10006);
                enemies.SetLevelList(GameManager.Hero.Level / 5 + 1, (int)(GameManager.Hero.Level * 1.2f + 4));
                CombatSceneManager.Engage(this, location, enemies.GetList());
            });
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
            DataStorageManager.SaveCharacterLocation<Lake>();
        }

        public override void OnBackKeyPress()
        {
            SceneManager.Play(new Village());
        }
    }
}
