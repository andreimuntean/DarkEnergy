using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.World.Objects;

namespace DarkEnergy.Scenes.World.Westhill
{
    public class Village : WorldScene
    {
        protected TexturedElement background;
        protected MapMarker forest;
        protected MapMarker acolyte;
        protected MapMarker generalStore;
        protected MapMarker portal;

        public Village()
        {
            background = new TexturedElement(1280, 720, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this, Path = @"World\Scenes\Westhill\Map.dds" };
            forest = new MapMarker(MarkerType.Combat) { Parent = this, Position = new Vector2(75, 430) };
            acolyte = new MapMarker(MarkerType.Quest) { Parent = this, Position = new Vector2(200, 40) };
            generalStore = new MapMarker(MarkerType.Vendor) { Parent = this, Position = new Vector2(445, 90) };
            portal = new MapMarker(MarkerType.Travel) { Parent = this, Position = new Vector2(1035, 90) };
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            forest.Initialize();
            acolyte.Initialize();
            generalStore.Initialize();
            portal.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            forest.LoadContent(contentManager);
            acolyte.LoadContent(contentManager);
            generalStore.LoadContent(contentManager);
            portal.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TouchManager.OnTap(acolyte, () => SceneManager.Play(new AcolyteDialogue()));

            TouchManager.OnTap(generalStore, () => SceneManager.Play(new Westhill.GeneralStore()));
            
            TouchManager.OnTap(portal, () => SceneManager.Play(new Westhill.Lake()));
            
            TouchManager.OnTap(forest, () =>
            {
                var heroLevel = GameManager.Hero.Level;
                var enemies = new EnemyList(10000, 10001, 10005);
                enemies.SetLevelList(1, heroLevel);

                enemies.SetCountList(1);
                if (heroLevel >= 2) enemies.Count.Add(2);
                if (heroLevel >= 7) enemies.Count.Add(3);

                CombatSceneManager.Engage(this, 8, enemies.GetList());
            });

            forest.Update(gameTime);
            acolyte.Update(gameTime);
            generalStore.Update(gameTime);
            portal.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            forest.Draw(renderer);
            acolyte.Draw(renderer);
            generalStore.Draw(renderer);
            portal.Draw(renderer);
            base.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            forest.UnloadContent(contentManager);
            acolyte.UnloadContent(contentManager);
            generalStore.UnloadContent(contentManager);
            portal.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        public override void SaveData()
        {
            DataStorageManager.SaveCharacterLocation<Village>();
        }
    }
}
