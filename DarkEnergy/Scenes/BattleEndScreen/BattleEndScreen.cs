using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Combat;
using DarkEnergy.Inventory;
using DarkEnergy.Scenes.Battle_End_Screen;

namespace DarkEnergy.Scenes
{
    public class BattleEndScreen : GameSystem, IScene
    {
        private Battle battle;
        private TexturedElement background;
        private List<TexturedElement> statusList;
        private List<TexturedElement> rewardsList;
        private float notificationOpacity = 0;

        protected int view;
        protected int View
        {
            get { return view; }
            set
            {
                view = value;
                notificationOpacity = 0;
                OnViewChanged();
            }
        }

        public BattleEndScreen(Battle battle)
        {
            this.battle = battle;
            background = new TexturedElement(1280, 720, HorizontalAlignment.Center, 0, VerticalAlignment.Middle, 0) { Parent = this, Path = @"Interface\DefaultBackground.dds" };
            statusList = new List<TexturedElement>();
            rewardsList = new List<TexturedElement>();

            if (battle.Victory)
            {
                var reward = battle.RewardManager.GetReward().Value;

                statusList.Add(new SimpleNotification(FontStyle.Calibri24, Resources.Strings.BattleEndWon));

                rewardsList.Add(new SimpleNotification(FontStyle.SegoeWP32, Resources.Strings.BattleEndRewards));
                
                rewardsList.Add(new IconNotification(FontStyle.Calibri30, reward.Experience.ToString(), @"Interface\Character\ExperienceIcon.dds"));
                
                if (reward.Currency > 0)
                    rewardsList.Add(new IconNotification(FontStyle.Calibri30, reward.Currency.ToString(), @"Interface\Character\CurrencyIcon.dds"));
                
                if (reward.DarkCrystals > 0)
                    rewardsList.Add(new IconNotification(FontStyle.Calibri30, reward.DarkCrystals.ToString(), @"Interface\Character\DarkCrystalsIcon.dds"));

                foreach (var id in reward.ItemIdList)
                {
                    var item = DataManager.Load<IItem>(id);
                    rewardsList.Add(new SimpleNotification(FontStyle.Calibri24, item.Name));
                }

                if (reward.Experience > GameManager.Hero.Experience)
                    rewardsList.Add(new SimpleNotification(FontStyle.Calibri24, Resources.Strings.BattleEndLevelUp));
                
                rewardsList.Add(new SimpleNotification(FontStyle.Calibri24, Resources.Strings.TapToContinue));
            }
            else
            {
                statusList.Add(new SimpleNotification(FontStyle.Calibri24, Resources.Strings.BattleEndLost));
            }

            statusList.Add(new SimpleNotification(FontStyle.Calibri24, Resources.Strings.TapToContinue));
            Loaded += BattleEndScreen_Loaded;
        }

        protected void Hide()
        {
            notificationOpacity = 0;
            statusList.ForEach(notification => notification.Opacity = 0);
            rewardsList.ForEach(notification => notification.Opacity = 0);
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            statusList.ForEach(notification => notification.Initialize());
            rewardsList.ForEach(notification => notification.Initialize());
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            statusList.ForEach(notification => notification.LoadContent(contentManager));
            rewardsList.ForEach(notification => notification.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (notificationOpacity < 1)
            {
                notificationOpacity += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (View == 0)
                {
                    statusList.ForEach(notification => notification.Opacity = notificationOpacity);
                }
                else if (View == 1)
                {
                    rewardsList.ForEach(notification => notification.Opacity = notificationOpacity);
                }
            }
            
            if (notificationOpacity > 0.25f)
            {
                TouchManager.OnTap(() => View += 1);
            }
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            statusList.ForEach(notification => notification.Draw(renderer));
            rewardsList.ForEach(notification => notification.Draw(renderer));   
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            statusList.ForEach(notification => notification.UnloadContent(contentManager));
            rewardsList.ForEach(notification => notification.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }

        public void OnBackKeyPress()
        {
            View += 1;
        }

        protected void BattleEndScreen_Loaded(object sender, System.EventArgs e)
        {
            var width = Screen.NativeResolution.X;
            var height = Screen.NativeResolution.Y;
            var padding = 20;

            foreach (var notification in statusList)
            {
                var count = statusList.Count;
                var index = statusList.IndexOf(notification);
                var totalHeight = count * notification.Height + (count - 1) * padding;

                notification.Position = new Vector2((width - notification.Width) / 2, (height - totalHeight) / 2 + index * (notification.Height + padding));
            }

            foreach (var notification in rewardsList)
            {
                var count = rewardsList.Count;
                var index = rewardsList.IndexOf(notification);
                var totalHeight = count * notification.Height + (count - 1) * padding;

                notification.Position = new Vector2((width - notification.Width) / 2, (height - totalHeight) / 2 + index * (notification.Height + padding));
            }

            if (rewardsList.Count > 0)
            {
                var first = rewardsList[0];
                var last = rewardsList[rewardsList.Count - 1];

                first.Position = new Vector2(first.X, first.Y - 40);
                last.Position = new Vector2(last.X, last.Y + 40);
            }

            View = 0;
            Hide();
        }

        protected void OnViewChanged()
        {
            if (View == 0)
            {
                statusList.ForEach(notification => notification.Visible = true);
                rewardsList.ForEach(notification => notification.Visible = false);
            }
            else if (View == 1 && rewardsList.Count > 0)
            {
                statusList.ForEach(notification => notification.Visible = false);
                rewardsList.ForEach(notification => notification.Visible = true);
            }
            else
            {
                LoadingScreenManager.Show(() => SceneManager.Play(battle.GetWorldScene()), Resources.Strings.LoadingWorld);
            }
        }
    }
}