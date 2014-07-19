using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Inventory;
using DarkEnergy.Inventory.Slots;

namespace DarkEnergy.Scenes.World.Menu.Inventory.Inventory
{
    public class ItemSlot : TexturedElement
    {
        private Text text;
        private Text slot;
        private TexturedElement elementIcon;
        public IItem Item { get; protected set; }

        protected string GetItemSlot(IItem item)
        {
            if (item is GenericItem)
            {
                return Resources.Strings.ItemMisc;
            }
            else if (item is EquippableItem)
            {
                return EquippableItem.GetName((item as EquippableItem).Slot);
            }
            else return "";
        }

        public ItemSlot(IItem item) : base(600, 60)
        {
            Item = item;
            Path = @"Interface\World\Menu\InventoryItem.dds";
            text = new Text(FontStyle.Calibri24) { Parent = this };
            slot = new Text(FontStyle.Calibri20) { Parent = this, String = GetItemSlot(item), Color = new Color(0.9f, 0.8f, 1.0f, 1.0f) };
            elementIcon = new TexturedElement(54, 54) { Parent = this, Path = @"Interface\Character\Elements.dds" };
            
            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }

        public void Refresh()
        {
            text.String = (Item.Count > 1) ? (Item.Name + " x " + Item.Count) : Item.Name;
            
            if (Item is EquippableItem && GameManager.Inventory.IsEquipped(Item as EquippableItem))
            {
                text.String += " " + Resources.Strings.Item_Equipped;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            text.Initialize();
            slot.Initialize();
            elementIcon.Initialize();

            if (!(Item is Weapon || Item is Relic))
            {
                elementIcon.Visible = false;
            }
            else
            {
                if (Item is Weapon) elementIcon.Frame = (int)(Item as Weapon).Element;
                else if (Item is Relic) elementIcon.Frame = (int)(Item as Relic).Element;
            }

            Refresh();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            text.LoadContent(contentManager);
            slot.LoadContent(contentManager);
            elementIcon.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            text.Draw(renderer);
            slot.Draw(renderer);
            elementIcon.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            text.UnloadContent(contentManager);
            slot.UnloadContent(contentManager);
            elementIcon.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void AdjustElements(object sender, EventArgs e)
        {
            if (Item is Weapon || Item is Relic)
            {
                text.Position = new Vector2(X + 65, Y + 11);
                elementIcon.Position = new Vector2(X + 3, Y + 3);
            }
            else
            {
                text.Position = new Vector2(X + 15, Y + 11);
            }

            slot.Position = new Vector2(X + Width - slot.Width - 15, Y + 13);
        }
        #endregion
    }
}
