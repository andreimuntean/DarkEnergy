using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Inventory;
using DarkEnergy.Inventory.Slots;

namespace DarkEnergy.Scenes.World.Menu.Inventory.Inventory
{
    public class InventoryData : GameSystem
    {
        private Text capacity, coins, darkCrystals;

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                AdjustElements(null, EventArgs.Empty);
            }
        }

        public InventoryData()
        {
            Position = new Vector2(700, 585);
            darkCrystals = new Text(FontStyle.Calibri20) { Parent = this, String = GameManager.Inventory.DarkCrystals.ToString() };
            coins = new Text(FontStyle.Calibri20) { Parent = this, String = GameManager.Inventory.Coins.ToString() };
            capacity = new Text(FontStyle.Calibri20) { Parent = this, String = "" };

            Loaded += AdjustElements;
        }

        public override void Initialize()
        {
            base.Initialize();
            darkCrystals.Initialize();
            coins.Initialize();
            capacity.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            darkCrystals.LoadContent(contentManager);
            coins.LoadContent(contentManager);
            capacity.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            AdjustElements(null, EventArgs.Empty);
        }

        public override void Draw(Renderer renderer)
        {
            darkCrystals.Draw(renderer);
            coins.Draw(renderer);
            capacity.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            darkCrystals.UnloadContent(contentManager);
            coins.UnloadContent(contentManager);
            capacity.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void AdjustElements(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                darkCrystals.String = GameManager.Inventory.DarkCrystals.ToString();
                darkCrystals.Position = Position;
                coins.String = GameManager.Inventory.Coins.ToString();
                coins.Position = Position + new Vector2(470 - coins.Width, 0);
                capacity.String = GameManager.Inventory.Count + "/" + GameManager.Inventory.Capacity;
                capacity.Position = Position + new Vector2((470 - capacity.Width) / 2, 0);
            }
        }
        #endregion
    }
}
