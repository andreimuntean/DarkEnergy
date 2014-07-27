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
    public class ItemViewer : TexturedElement
    {
        private Text name, description, slot;
        private Hero characterPreview;
        private List<CalligraphedImage> buttons;

        public IItem Item { get; protected set; }
        public int ButtonCount { get; protected set; }
        public bool ValueDisplayed { get; set; }

        private int selectedView;
        public int SelectedView
        {
            get { return selectedView; }
            set
            {
                selectedView = value;

                if (SelectedView == 0)
                {
                    description.Visible = true;
                    slot.Visible = true;
                    characterPreview.Visible = false;
                    buttons[0].Layer = 1;
                    buttons[1].Layer = 0;
                }
                else if (SelectedView == 1)
                {
                    description.Visible = false;
                    slot.Visible = false;
                    characterPreview.Visible = true;
                    buttons[0].Layer = 0;
                    buttons[1].Layer = 1;
                }
            }
        }

        public ItemViewer() : base(530, 430, new Vector2(30, 120))
        {
            Path = @"Interface\World\Menu\ItemViewerBackground.dds";

            name = new Text(FontStyle.CenturyGothic24) { Parent = this };
            description = new Text(FontStyle.Calibri20) { Parent = this, MaxWidth = 470 };
            slot = new Text(FontStyle.Calibri20) { Parent = this, Color = new Color(0.9f, 0.8f, 1.0f, 1.0f) };
            characterPreview = new Hero(equipment: DarkEnergy.Inventory.Equipment.FromIdList(GameManager.Inventory.Equipment.GetIdList())) { Parent = this };

            buttons = new List<CalligraphedImage>()
            {
                new CalligraphedImage(250, 72, Vector2.Zero, "", FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\ItemViewerButton.dds" },
                new CalligraphedImage(250, 72, Vector2.Zero, "", FontStyle.CenturyGothic20) { Parent = this, Path = @"Interface\World\Menu\ItemViewerButton.dds" }
            };

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }

        public void Refresh()
        {
            Func<string, string> formatDescription = (text) =>
            {
                if (ValueDisplayed)
                {
                    return text;
                }
                else
                {
                    var start = text.IndexOf(Resources.Strings.Item_Value);
                    return text.Remove(start, text.Length - start);
                }
            };

            name.String = Item.Name;
            slot.String = Item.GetType().Name;
            
            if (Item is EquippableItem)
            {
                var equippableItem = Item as EquippableItem;
                var text = equippableItem.GetDescription(GameManager.Inventory.GetEquippedItem(equippableItem.Slot));
                description.String = formatDescription(text);
                
                ButtonCount = 2;
                buttons[0].String = Resources.Strings.InventoryMenu_Details;
                buttons[1].String = Resources.Strings.InventoryMenu_View;

                #region Loads the character equipment for the item preview screen.
                characterPreview.Equipment = DarkEnergy.Inventory.Equipment.FromIdList(GameManager.Inventory.Equipment.GetIdList());
                GameManager.Inventory.Equip(characterPreview.Equipment, equippableItem);
                characterPreview.Equipment.Parent = characterPreview;
                characterPreview.Equipment.Initialize();
                characterPreview.Equipment.LoadContent(App.Game.Content);
                #endregion
            }
            else
            {
                var text = Item.GetDescription();
                description.String = formatDescription(text);

                if (Item is GenericItem)
                {
                    ButtonCount = 1;
                    buttons[0].String = Resources.Strings.InventoryMenu_Details;
                }
                else ButtonCount = 0;
            }

            // Resets the view.
            if (SelectedView >= ButtonCount) SelectedView = 0;

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
            SelectedView = 0;
            Visible = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            name.Initialize();
            description.Initialize();
            slot.Initialize();
            characterPreview.Initialize();
            buttons.ForEach(button => button.Initialize());
            characterPreview.Scale = new Vector2(0.65f);
            SelectedView = 0;
            ValueDisplayed = true;
            Visible = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            name.LoadContent(contentManager);
            description.LoadContent(contentManager);
            slot.LoadContent(contentManager);
            characterPreview.LoadContent(contentManager);
            buttons.ForEach(button => button.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            TouchManager.OnTap(buttons[0], () => { if (SelectedView != 0) SelectedView = 0; });
            TouchManager.OnTap(buttons[1], () => { if (SelectedView != 1) SelectedView = 1; });
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            name.Draw(renderer);
            description.Draw(renderer);
            slot.Draw(renderer);
            characterPreview.Draw(renderer);
            buttons.ForEach(button => button.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            name.UnloadContent(contentManager);
            description.UnloadContent(contentManager);
            slot.UnloadContent(contentManager);
            characterPreview.UnloadContent(contentManager);
            buttons.ForEach(button => button.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void AdjustElements(object sender, EventArgs e)
        {
            name.Position = Position + new Vector2((Width - name.Width) / 2, (70 - name.Height) / 2);
            description.Position = Position + new Vector2(30, 100);
            slot.Position = Position + new Vector2(Width - slot.Width - 30, 100);
            characterPreview.Position = Position + new Vector2((530 - characterPreview.Width) / 2, 100);
            
            if (ButtonCount == 1)
            {
                buttons[0].Position = Position + new Vector2(135, 348);
            }
            else if (ButtonCount == 2)
            {
                buttons[0].Position = Position + new Vector2(10, 348);
                buttons[1].Position = Position + new Vector2(270, 348);
            }
        }
        #endregion
    }
}
