using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Inventory;
using DarkEnergy.Inventory.Slots;
using DarkEnergy.Scenes.World.Menu.Inventory.Inventory;

namespace DarkEnergy.Scenes.World.Menu.Inventory
{
    public class InventoryPage : GameSystem
    {
        private TexturedElement background;
        private Text message;

        private int messageStatus;
        private double timeElapsed;
        private const double displayTime = 2;

        public ItemViewer Viewer { get; protected set; }
        public ItemController Controller { get; protected set; }
        public ItemListPanel ItemListPanel { get; protected set; }
        public InventoryData InventoryData { get; protected set; }

        public IItem SelectedItem { get; protected set; }

        public InventoryPage()
        {
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\InventoryBackground.dds" };
            message = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 0, VerticalAlignment.Bottom, -20) { Parent = this };
            Viewer = new ItemViewer() { Parent = this };
            Controller = new ItemController() { Parent = this };
            ItemListPanel = new ItemListPanel(GameManager.Inventory.Items) { Parent = this };
            InventoryData = new InventoryData() { Parent = this };

            Controller.ButtonTapped += Controller_ButtonTapped;
            ItemListPanel.SelectionChanged += ItemListPanel_SelectionChanged;
        }

        protected void Use()
        {
            DisplayMessage(Resources.Strings.InventoryMenu_MessageUse.Replace("{I}", SelectedItem.Name));
        }

        protected void Toggle()
        {
            if (!GameManager.Inventory.Equipment.ToList().Contains(SelectedItem as EquippableItem))
            {
                DisplayMessage(Resources.Strings.InventoryMenu_MessageEquip.Replace("{I}", SelectedItem.Name));
                GameManager.Inventory.Equip(GameManager.Inventory.Equipment, SelectedItem as EquippableItem);
                ItemListPanel.RefreshToggleStatus();
                Viewer.Refresh();
            }
            else
            {
                if (SelectedItem is Weapon || SelectedItem is Chest || SelectedItem is Legs || SelectedItem is Feet)
                {
                    DisplayMessage(Resources.Strings.InventoryMenu_MessageUnequipFailure);
                    PhoneEffectsManager.Play(PhoneEffect.Vibration);
                }
                else
                {
                    DisplayMessage(Resources.Strings.InventoryMenu_MessageUnequipSuccess.Replace("{S}", SelectedItem.GetType().Name));
                    GameManager.Inventory.Equipment.Unequip(SelectedItem.GetType());
                    ItemListPanel.RefreshToggleStatus();
                    Viewer.Refresh();
                }
            }
        }

        protected void Delete()
        {
            if (GameManager.Inventory.Remove(SelectedItem))
            {
                DisplayMessage(Resources.Strings.InventoryMenu_MessageDeleteSuccess.Replace("{I}", SelectedItem.Name + (SelectedItem.Count > 1 ? " x " + SelectedItem.Count : "")));
                ItemListPanel.LoadSlots();
                Deselect();
            }
            else
            {
                DisplayMessage(Resources.Strings.InventoryMenu_MessageDeleteFailure);
                PhoneEffectsManager.Play(PhoneEffect.Vibration);
            }
        }

        public void DisplayMessage(string value)
        {
            timeElapsed = 0;
            messageStatus = 1;
            message.Opacity = 0;
            message.String = value;
            message.Visible = true;
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
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            message.Initialize();
            Controller.Initialize();
            Viewer.Initialize();
            ItemListPanel.Initialize();
            InventoryData.Initialize();

            messageStatus = 0;
            message.Visible = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            message.LoadContent(contentManager);
            Controller.LoadContent(contentManager);
            Viewer.LoadContent(contentManager);
            ItemListPanel.LoadContent(contentManager);
            InventoryData.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            message.Update(gameTime);
            Controller.Update(gameTime);
            Viewer.Update(gameTime);
            ItemListPanel.Update(gameTime);
            InventoryData.Update(gameTime);

            if (messageStatus > 0)
            {
                if (messageStatus == 1)
                {
                    message.Opacity += (float)(gameTime.ElapsedGameTime.TotalSeconds * 3);
                    if (message.Opacity == 1)
                    {
                        messageStatus = 2;
                    }
                }
                else if (messageStatus == 2)
                {
                    timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeElapsed >= displayTime)
                    {
                        messageStatus = 3;
                    }
                }
                else if (messageStatus == 3)
                {
                    message.Opacity -= (float)(gameTime.ElapsedGameTime.TotalSeconds * 3);
                    if (message.Opacity == 0)
                    {
                        message.Visible = false;
                        messageStatus = 0;
                    }
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            ItemListPanel.DrawItems(renderer);
            background.Draw(renderer);
            message.Draw(renderer);
            Controller.Draw(renderer);
            Viewer.Draw(renderer);
            ItemListPanel.Draw(renderer);
            InventoryData.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            message.UnloadContent(contentManager);
            Controller.UnloadContent(contentManager);
            Viewer.UnloadContent(contentManager);
            ItemListPanel.UnloadContent(contentManager);
            InventoryData.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void ItemListPanel_SelectionChanged(object sender, EventArgs e)
        {
            var item = ItemListPanel.SelectedItem;

            if (item != SelectedItem)
            {
                Select(item);
            }
            else
            {
                Deselect();
            }
        }

        protected void Controller_ButtonTapped(object sender, EventArgs e)
        {
            var action = sender as ItemAction?;

            if (action == ItemAction.Use)
            {
                Use();
            }
            else if (action == ItemAction.Toggle)
            {
                Toggle();
            }
            else if (action == ItemAction.Delete)
            {
                Delete();
            }

            Controller.Refresh();
        }
        #endregion
    }
}
