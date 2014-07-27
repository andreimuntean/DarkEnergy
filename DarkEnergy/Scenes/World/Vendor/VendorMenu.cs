using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Inventory;
using DarkEnergy.Inventory.Slots;
using DarkEnergy.Scenes.Trading;
using DarkEnergy.Scenes.World;
using DarkEnergy.Scenes.World.Menu.Inventory.Inventory;
using DarkEnergy.Trading;

namespace DarkEnergy.Scenes
{
    public enum InventoryAction { None, Buying, Selling }

    public class VendorMenu : GameSystem, IScene
    {
        private TexturedElement background;
        private Text cost;
        private WorldScene worldScene;
        private Vendor vendor;

        public ItemViewer Viewer { get; protected set; }
        public ItemController Controller { get; protected set; }
        public ItemListPanel ItemListPanel { get; protected set; }
        public InventoryData InventoryData { get; protected set; }
        public TradingSwitch TradingSwitch { get; protected set; }

        public IItem SelectedItem { get; protected set; }

        private InventoryAction status;
        public InventoryAction Status
        {
            get { return status; }
            set
            {
                status = value;
                Controller.Status = Status;

                if (Status == InventoryAction.Buying)
                {
                    ItemListPanel.LoadSlots(vendor.Items);
                }
                else if (Status == InventoryAction.Selling)
                {
                    ItemListPanel.LoadSlots(GameManager.Inventory.Items);
                }
            }
        }

        public VendorMenu(WorldScene currentScene, int id)
        {
            worldScene = currentScene;
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\VendorBackground.dds" };
            cost = new Text(FontStyle.Calibri24, 45, 557) { Parent = this };

            vendor = DataManager.Load<Vendor>(id);

            Viewer = new ItemViewer() { Parent = this, Position = new Vector2(30, 100) };
            Controller = new ItemController() { Parent = this, Position = new Vector2(30, 540) };
            ItemListPanel = new ItemListPanel(vendor.Items) { Parent = this, Position = new Vector2(620, 60) };
            InventoryData = new InventoryData() { Parent = this, Position = new Vector2(700, 605) };
            TradingSwitch = new TradingSwitch() { Parent = this };

            Controller.ButtonTapped += Controller_ButtonTapped;
            ItemListPanel.SelectionChanged += ItemListPanel_SelectionChanged;
            TradingSwitch.SelectionChanged += TradingSwitch_SelectionChanged;
        }

        public WorldScene GetWorldScene()
        {
            return worldScene;
        }

        protected void Buy()
        {
            if (CanBuy(SelectedItem))
            {
                if (GameManager.Inventory.CanAdd(SelectedItem, SelectedItem.Count))
                {
                    var price = PriceOf(SelectedItem);

                    GameManager.Inventory.Add(vendor.GetId(SelectedItem));

                    if (price.Currency == Currency.Coins)
                    {
                        GameManager.Inventory.Coins -= price.Value;
                    }
                    else if (price.Currency == Currency.DarkCrystals)
                    {
                        GameManager.Inventory.DarkCrystals -= price.Value;
                    }
                }
                else
                {
                    // This will likely be expanded in the future.
                    PhoneEffectsManager.Play(PhoneEffect.Vibration);
                }
            }
            else
            {
                // This will likely be expanded in the future.
                PhoneEffectsManager.Play(PhoneEffect.Vibration);
            }
        }

        protected void Sell()
        {
            if (GameManager.Inventory.Remove(SelectedItem))
            {
                GameManager.Inventory.Coins += PriceOf(SelectedItem, true).Value;
                ItemListPanel.LoadSlots();
                Deselect();
            }
            else
            {
                // This will likely be expanded in the future.
                PhoneEffectsManager.Play(PhoneEffect.Vibration);
            }
        }

        public Price PriceOf(IItem item, bool playerSellsIt = false)
        {
            return playerSellsIt ? new Price((int)Math.Round(item.Value * vendor.TradeModifier), Currency.Coins) : vendor.PriceOf(item);
        }

        public bool CanBuy(IItem item)
        {
            var price = PriceOf(item);

            if (price.Currency == Currency.Coins)
            {
                return price.Value <= GameManager.Inventory.Coins;
            }
            else if (price.Currency == Currency.DarkCrystals)
            {
                return price.Value <= GameManager.Inventory.DarkCrystals;
            }
            else return false;
        }

        public void Select(IItem item)
        {
            SelectedItem = item;
            Viewer.Select(item);
            Controller.Select(item);
        }

        public void Deselect()
        {
            SelectedItem = null;
            Viewer.Deselect();
            Controller.Deselect();
            cost.String = "";
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            cost.Initialize();
            Controller.Initialize();
            Viewer.Initialize();
            ItemListPanel.Initialize();
            InventoryData.Initialize();
            TradingSwitch.Initialize();

            Viewer.ValueDisplayed = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            cost.LoadContent(contentManager);
            Controller.LoadContent(contentManager);
            Viewer.LoadContent(contentManager);
            ItemListPanel.LoadContent(contentManager);
            InventoryData.LoadContent(contentManager);
            TradingSwitch.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            Controller.Update(gameTime);
            Viewer.Update(gameTime);
            TradingSwitch.Update(gameTime);
            ItemListPanel.Update(gameTime);
            InventoryData.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            ItemListPanel.DrawItems(renderer);
            background.Draw(renderer);
            Controller.Draw(renderer);
            cost.Draw(renderer);
            Viewer.Draw(renderer);
            ItemListPanel.Draw(renderer);
            InventoryData.Draw(renderer);
            TradingSwitch.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            cost.UnloadContent(contentManager);
            Controller.UnloadContent(contentManager);
            Viewer.UnloadContent(contentManager);
            ItemListPanel.UnloadContent(contentManager);
            TradingSwitch.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        public void OnBackKeyPress()
        {
            SceneManager.Play(worldScene);
        }

        protected void ItemListPanel_SelectionChanged(object sender, EventArgs e)
        {
            var item = ItemListPanel.SelectedItem;

            if (item != SelectedItem)
            {
                Select(item);

                var price = new Price();

                if (Status == InventoryAction.Buying)
                {
                    price = PriceOf(SelectedItem);
                }
                else if (Status == InventoryAction.Selling)
                {
                    price = PriceOf(SelectedItem, true);
                }

                cost.String = price.Value.ToString() + " ";

                if (price.Currency == Currency.Coins)
                {
                    cost.String += Resources.Strings.Currency_Coins;
                }
                else if (price.Currency == Currency.DarkCrystals)
                {
                    cost.String += Resources.Strings.Currency_DarkCrystals;
                }

                if (CanBuy(SelectedItem))
                {
                    cost.Color = new Color(255, 255, 255);
                }
                else cost.Color = new Color(255, 100, 100); 
            }
            else
            {
                Deselect();
            }

            cost.Position = new Vector2(45 + (237 - cost.Width) / 2, 557);
        }

        protected void TradingSwitch_SelectionChanged(object sender, EventArgs e)
        {
            Deselect();
            Status = TradingSwitch.Status;
        }

        protected void Controller_ButtonTapped(object sender, EventArgs e)
        {
            var action = sender as ItemAction?;

            if (action == ItemAction.Buy)
            {
                Buy();
            }
            else if (action == ItemAction.Sell)
            {
                Sell();
            }

            Controller.Refresh();
        }
        #endregion
    }
}
