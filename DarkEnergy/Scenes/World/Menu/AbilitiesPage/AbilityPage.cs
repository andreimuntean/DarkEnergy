using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.Abilities
{
    public class AbilityPage : GameSystem
    {
        private TexturedElement background;
        private TexturedElement upgradeAbility;
        private Text unspentPoints, cannotUpgrade;

        public Ability Ability { get; protected set; }
        public int SetIndex { get; protected set; }
        public List<AbilityWidePanel> AbilityWidePanels { get; protected set; }

        public AbilityPage(Ability ability, int setIndex)
        {
            Ability = ability;
            SetIndex = setIndex;

            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\AbilityBackground.dds" };
            upgradeAbility = new CalligraphedImage(316, 80, HorizontalAlignment.Center, 0, 575, Resources.Strings.UpgradeAbility, FontStyle.Calibri30, Color.White, Vector2.Zero) { Parent = this, Path = @"Interface\DefaultButton.dds" };
            unspentPoints = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 0, VerticalAlignment.Bottom, -20) { Parent = this };
            cannotUpgrade = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 0, 595) { Parent = this, String = Resources.Strings.CannotUpgradeAbility };
        }

        public void Refresh()
        {
            unspentPoints.String = Resources.Strings.RemainingMasteryPoints.Replace("$", GameManager.Hero.MasteryPoints.ToString());

            var contentManager = App.Game.Content;

            if (AbilityWidePanels != null)
            {
                foreach (var widePanel in AbilityWidePanels)
                {
                    widePanel.UnloadContent(contentManager);
                }
            }

            AbilityWidePanels = new List<AbilityWidePanel>();

            var panel = new AbilityWidePanel(Ability, GameManager.Hero.AbilitySets[SetIndex].Template) { Parent = this };
            panel.Initialize();
            panel.Visible = false;
            panel.LoadContent(contentManager);
            AbilityWidePanels.Add(panel);

            if (Ability.Rank < Ability.HighestRank)
            {
                panel = new AbilityWidePanel(Ability.GetNextRank(), GameManager.Hero.AbilitySets[SetIndex].Template, false) { Parent = this };
                panel.Initialize();
                panel.Visible = false;
                panel.LoadContent(contentManager);
                AbilityWidePanels.Add(panel);

                upgradeAbility.Visible = true;
                cannotUpgrade.Visible = false;
            }
            else
            {
                upgradeAbility.Visible = false;
                cannotUpgrade.Visible = true;
            }

            float padding = 10;
            float containerWidth = AbilityWidePanels.Count * (595 + padding) - padding;
            float x = (Screen.NativeResolution.X - containerWidth) / 2;

            for (int i = 0; i < AbilityWidePanels.Count; ++i)
            {
                AbilityWidePanels[i].Position = new Vector2(x + i * (595 + padding), 130);
                AbilityWidePanels[i].Visible = true;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            upgradeAbility.Initialize();
            cannotUpgrade.Initialize();
            unspentPoints.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            upgradeAbility.LoadContent(contentManager);
            cannotUpgrade.LoadContent(contentManager);
            unspentPoints.LoadContent(contentManager);

            Refresh();
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            TouchManager.OnTap(upgradeAbility, () =>
            {
                var locked = (AbilityWidePanels.Count > 1) ? AbilityWidePanels[1].Locked : false;
                if (!locked && Ability.Rank < Ability.HighestRank && GameManager.Hero.MasteryPoints > 0)
                {
                    Ability.IncreaseRank();
                    GameManager.Hero.MasteryPoints -= 1;
                    Refresh();
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            upgradeAbility.Draw(renderer);
            cannotUpgrade.Draw(renderer);
            unspentPoints.Draw(renderer);
            AbilityWidePanels.ForEach(panel => panel.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            upgradeAbility.UnloadContent(contentManager);
            cannotUpgrade.UnloadContent(contentManager);
            unspentPoints.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
