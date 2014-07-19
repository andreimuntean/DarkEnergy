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
    public enum ItemAction { Use, Toggle, Delete }

    public class ItemController : TexturedElement
    {
        public event EventHandler ButtonTapped;

        private List<CalligraphedImage> buttons;

        public IItem Item { get; protected set; }
        public int ButtonCount { get; protected set; }

        public ItemController() : base(530, 80, new Vector2(30, 560))
        {
            Path = @"Interface\World\Menu\ItemControllerBackground.dds";

            buttons = new List<CalligraphedImage>()
            {
                new CalligraphedImage(237, 65, Vector2.Zero, "", FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\ItemControllerButton.dds" },
                new CalligraphedImage(237, 65, Vector2.Zero, "", FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\ItemControllerButton.dds" }
            };

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }

        public void Refresh()
        {
            if (Item is GenericItem)
            {
                ButtonCount = 1;
                buttons[0].String = Resources.Strings.Item_Delete;
            }
            else if (Item is EquippableItem)
            {
                ButtonCount = 2;
                buttons[0].String = (GameManager.Inventory.Equipment.ToList().Contains(Item as EquippableItem) ? Resources.Strings.Item_Unequip : Resources.Strings.Item_Equip);
                buttons[1].String = Resources.Strings.Item_Delete;
            }
            else ButtonCount = 0;

            AdjustElements(null, EventArgs.Empty);
        }

        public void Select(IItem item)
        {
            Item = item;
            Refresh();
            Visible = true;
        }

        public void Deselect()
        {
            Item = null;
            Visible = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            buttons.ForEach(button => button.Initialize());
            Visible = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            buttons.ForEach(button => button.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (Item is GenericItem)
            {
                TouchManager.OnTap(buttons[0], () => OnButtonTapped(ItemAction.Delete));
            }
            else if (Item is EquippableItem)
            {
                TouchManager.OnTap(buttons[0], () => OnButtonTapped(ItemAction.Toggle));
                TouchManager.OnTap(buttons[1], () => OnButtonTapped(ItemAction.Delete));
            }
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            buttons.ForEach(button => button.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            buttons.ForEach(button => button.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void OnButtonTapped(ItemAction? itemAction)
        {
            if (ButtonTapped != null)
            {
                ButtonTapped(itemAction, EventArgs.Empty);
            }
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            if (ButtonCount == 1)
            {
                buttons[0].Position = Position + new Vector2(146.5f, 7.5f);
            }
            else if (ButtonCount == 2)
            {
                buttons[0].Position = Position + new Vector2(15, 7.5f);
                buttons[1].Position = Position + new Vector2(278, 7.5f);
            }
        }
        #endregion
    }
}
