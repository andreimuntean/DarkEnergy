using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Scenes.World.Menu.Character.Abilities;

namespace DarkEnergy.Scenes.World.Menu.Character
{
    public class AbilitiesPage : GameSystem
    {
        private TexturedElement background;
        private Text abilitySetName;
        private TexturedElement leftButton, rightButton;
        private Text unspentPoints;

        public List<AbilityPanel> AbilityPanels { get; protected set; }

        public AbilitySet SelectedSet
        {
            get { return GameManager.Hero.AbilitySets[SelectedSetIndex]; }
        }

        public string SelectedSetName
        {
            get { return SelectedSet.Name; }
        }

        private int selectedSetIndex;
        public int SelectedSetIndex
        {
            get { return selectedSetIndex; }
            set
            {
                selectedSetIndex = value;
                if (IsLoaded) { LoadAbilities(); }
            }
        }

        public AbilitiesPage(int setIndex)
        {
            SelectedSetIndex = setIndex;
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\AbilitiesBackground.dds" };
            abilitySetName = new Text(FontStyle.CenturyGothic40, HorizontalAlignment.Center, 0, 160) { Parent = this };
            leftButton = new TexturedElement(120, 120, 40, 130) { Parent = this, Path = @"Interface\ArrowButtons.dds" };
            rightButton = new TexturedElement(120, 120, 1120, 130) { Parent = this, Path = @"Interface\ArrowButtons.dds" };
            unspentPoints = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 0, VerticalAlignment.Bottom, -20) { Parent = this, String = Resources.Strings.RemainingMasteryPoints.Replace("$", GameManager.Hero.MasteryPoints.ToString()) };
        }

        public void LoadAbilities()
        {
            var contentManager = App.Game.Content;

            if (AbilityPanels != null)
            {
                foreach (var panel in AbilityPanels)
                {
                    panel.UnloadContent(contentManager);
                }
            }

            AbilityPanels = new List<AbilityPanel>();

            for (int i = 0; i < SelectedSet.Abilities.Count; ++i)
            {
                var panel = new AbilityPanel(SelectedSet, i, SelectedSetIndex) { Parent = this };
                panel.Initialize();
                panel.Visible = false;
                panel.LoadContent(contentManager);

                AbilityPanels.Add(panel);
            }

            float padding = 20;
            float containerWidth = AbilityPanels.Count * (280 + padding) - padding;
            float x = (Screen.NativeResolution.X - containerWidth) / 2;

            for (int i = 0; i < AbilityPanels.Count; ++i)
            {
                AbilityPanels[i].Position = new Vector2(x + i * (280 + padding), 290);
                AbilityPanels[i].Visible = true;
            }

            abilitySetName.String = SelectedSetName;
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            abilitySetName.Initialize();
            leftButton.Initialize();
            rightButton.Initialize();
            unspentPoints.Initialize();

            leftButton.Frame = 0;
            rightButton.Frame = 1;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            abilitySetName.LoadContent(contentManager);
            leftButton.LoadContent(contentManager);
            rightButton.LoadContent(contentManager);
            unspentPoints.LoadContent(contentManager);

            LoadAbilities();

            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            TouchManager.OnTap(leftButton, () =>
            {
                if (SelectedSetIndex > 0)
                {
                    SelectedSetIndex -= 1;
                }
                else SelectedSetIndex = GameManager.Hero.AbilitySets.Count - 1;
            });

            TouchManager.OnTap(rightButton, () =>
            {
                if (SelectedSetIndex < GameManager.Hero.AbilitySets.Count - 1)
                {
                    SelectedSetIndex += 1;
                }
                else SelectedSetIndex = 0;
            });

            AbilityPanels.ForEach(panel => panel.Update(gameTime));
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            abilitySetName.Draw(renderer);
            leftButton.Draw(renderer);
            rightButton.Draw(renderer);
            unspentPoints.Draw(renderer);
            AbilityPanels.ForEach(panel => panel.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            abilitySetName.UnloadContent(contentManager);
            leftButton.UnloadContent(contentManager);
            rightButton.UnloadContent(contentManager);
            unspentPoints.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
