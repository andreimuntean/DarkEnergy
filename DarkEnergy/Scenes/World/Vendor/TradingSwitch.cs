using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using System;

namespace DarkEnergy.Scenes.Trading
{
    public class TradingSwitch : GameSystem
    {
        public event EventHandler SelectionChanged;

        private TexturedElement left, right;
        private Text buy, sell;

        private InventoryAction status;
        public InventoryAction Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;

                    if (status == InventoryAction.Buying)
                    {
                        left.Opacity = 1;
                        right.Opacity = 0;
                    }
                    else if (status == InventoryAction.Selling)
                    {
                        left.Opacity = 0;
                        right.Opacity = 1;
                    }

                    OnSelectionChanged(EventArgs.Empty);
                }
            }
        }

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

        public TradingSwitch()
        {
            Position = new Vector2(632, 512);

            left = new TexturedElement(302, 68) { Parent = this, Path = @"Interface\World\Menu\VendorPlayerSwitch.dds" };
            right = new TexturedElement(302, 68) { Parent = this, Path = @"Interface\World\Menu\VendorPlayerSwitch.dds" };

            buy = new Text(FontStyle.CenturyGothic24) { Parent = this, String = Resources.Strings.TradingBuy };
            sell = new Text(FontStyle.CenturyGothic24) { Parent = this, String = Resources.Strings.TradingSell };

            Loaded += AdjustElements;
        }

        public override void Initialize()
        {
            base.Initialize();
            left.Initialize();
            right.Initialize();
            buy.Initialize();
            sell.Initialize();

            Status = InventoryAction.Buying;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            left.LoadContent(contentManager);
            right.LoadContent(contentManager);
            buy.LoadContent(contentManager);
            sell.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            TouchManager.OnTap(left, () => Status = InventoryAction.Buying);
            TouchManager.OnTap(right, () => Status = InventoryAction.Selling);
        }

        public override void Draw(Renderer renderer)
        {
            left.Draw(renderer);
            right.Draw(renderer);
            buy.Draw(renderer);
            sell.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            left.UnloadContent(contentManager);
            right.UnloadContent(contentManager);
            buy.UnloadContent(contentManager);
            sell.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void AdjustElements(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                left.Position = Position;
                right.Position = Position + new Vector2(304, 0);
                buy.Position = left.Position + new Vector2((left.Width - buy.Width) / 2, (left.Height - buy.Height) / 2);
                sell.Position = right.Position + new Vector2((right.Width - sell.Width) / 2, (right.Height - sell.Height) / 2);
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
