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
    public enum ItemFilter { All, Weapons, Armors, Misc }

    public class ItemListPanel : GameSystem
    {
        public event EventHandler SelectionChanged;

        private Region inventoryRegion;
        private List<CalligraphedImage> filterButtons;

        private ItemFilter filter;
        public ItemFilter Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                LoadSlots();
            }
        }

        private List<IItem> items;
        public List<IItem> Items
        {
            get { return items; }
            set
            {
                items = value;
                LoadSlots();
            }
        }

        public List<ItemSlot> Slots { get; protected set; }

        public Vector2 ItemSlotsTotalSize { get { return new Vector2(600, 70 + 65 * (Slots.Count - 1)); } }
        public float VerticalScroll { get; set; }

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

        private IItem selectedItem;
        public IItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnSelectionChanged(EventArgs.Empty);
            }
        }

        public ItemListPanel(List<IItem> items)
        {
            this.items = items;
            Position = new Vector2(620, 120);

            filterButtons = new List<CalligraphedImage>()
            {
                new CalligraphedImage(150, 72, Position + new Vector2(8, 8), Resources.Strings.InventoryFilters_All, FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\InventoryFilterButton.dds" },
                new CalligraphedImage(150, 72, Position + new Vector2(163, 8), Resources.Strings.InventoryFilters_Weapons, FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\InventoryFilterButton.dds" },
                new CalligraphedImage(150, 72, Position + new Vector2(317, 8), Resources.Strings.InventoryFilters_Armors, FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\InventoryFilterButton.dds" },
                new CalligraphedImage(150, 72, Position + new Vector2(472, 8), Resources.Strings.InventoryFilters_Misc, FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\InventoryFilterButton.dds" }
            };

            Loaded += AdjustElements;
        }

        protected Vector2 GetSlotPosition(int index)
        {
            return Position + new Vector2(15, VerticalScroll + 85 + 65 * index);
        }

        protected void RefreshViewport()
        {
            if (VerticalScroll < -(ItemSlotsTotalSize.Y - 370)) VerticalScroll = -(ItemSlotsTotalSize.Y - 370);
            if (VerticalScroll > 0) VerticalScroll = 0;

            for (int i = 0; i < Slots.Count; ++i)
            {
                Slots[i].Position = GetSlotPosition(i);
            }
        }

        protected void Scroll(float value)
        {
            VerticalScroll += value;
            RefreshViewport();
        }

        public void LoadSlots()
        {
            Slots = new List<ItemSlot>();

            foreach (var item in items)
            {
                switch (Filter)
                {
                    case ItemFilter.Weapons: if (!(item is Weapon)) continue; break;
                    case ItemFilter.Armors: if (!(item is EquippableItem) || (item is EquippableItem && item is Weapon)) continue; break;
                    case ItemFilter.Misc: if (!(item is GenericItem)) continue; break;
                }

                var slot = new ItemSlot(item) { Parent = this, Position = GetSlotPosition(Slots.Count) };
                Slots.Add(slot);
            }

            Slots.ForEach(slot => { slot.Initialize(); slot.LoadContent(App.Game.Content); });
            RefreshViewport();
            AdjustElements(this, EventArgs.Empty);
        }

        public void LoadSlots(List<IItem> items)
        {
            Items = items;
        }

        /// <summary>
        /// Refreshes the toggle status of the item.
        /// </summary>
        public void RefreshToggleStatus()
        {
            foreach (var slot in Slots)
            {
                slot.Refresh();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            filterButtons.ForEach(button => button.Initialize());
            Filter = ItemFilter.All;
            VerticalScroll = 0;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            filterButtons.ForEach(button => button.LoadContent(contentManager));
            Slots.ForEach(slot => slot.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < filterButtons.Count; ++i)
            {
                var button = filterButtons[i];
                var thisFilter = (ItemFilter)i;
                
                TouchManager.OnTap(button, () => Filter = thisFilter);

                button.Layer = (thisFilter == Filter) ? 1 : 0;
            }

            if (Slots.Count > 0)
            {
                Slots.ForEach(slot =>
                {
                    TouchManager.OnRelease(slot, () => SelectedItem = slot.Item);
                });

                TouchManager.OnDrag(inventoryRegion, () =>
                {
                    Scroll(TouchManager.Delta.Y);
                    AdjustElements(inventoryRegion, EventArgs.Empty);
                });
            }
        }

        public void DrawItems(Renderer renderer)
        {
            Slots.ForEach(slot => slot.Draw(renderer));
        }

        public override void Draw(Renderer renderer)
        {
            filterButtons.ForEach(button => button.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            filterButtons.ForEach(button => button.UnloadContent(contentManager));
            Slots.ForEach(slot => slot.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void AdjustElements(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                inventoryRegion = new Region(Position + new Vector2(10, 80), new Vector2(610, 370));

                // Readjusting every element while scrolling would be redundant.
                if (sender != inventoryRegion)
                {
                    filterButtons[0].Position = Position + new Vector2(8, 8);
                    filterButtons[1].Position = Position + new Vector2(163, 8);
                    filterButtons[2].Position = Position + new Vector2(317, 8);
                    filterButtons[3].Position = Position + new Vector2(472, 8);
                }
            }
        }

        protected void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, e);
            }
        }
        #endregion
    }
}
