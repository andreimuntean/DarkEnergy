using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat.Interface
{
    public class AbilityFrames : GameSystem
    {
        private Battle battle;
        private TexturedElement leftButton, rightButton;

        public AbilitySetView CurrentSet { get; private set; }
        public List<AbilitySetView> AbilitySets { get; private set; }

        public int SetIndex { get { return AbilitySets.IndexOf(CurrentSet); } }
        public int SetCount { get { return AbilitySets.Count; } }

        public AbilityFrames(Battle battle)
        {
            this.battle = battle;
            leftButton = new TexturedElement(120, 120) { Parent = this, Path = @"Interface\ArrowButtonsDark.dds" };
            rightButton = new TexturedElement(120, 120) { Parent = this, Path = @"Interface\ArrowButtonsDark.dds" };
            battle.Units.CurrentChanged += Units_CurrentChanged;
        }

        public override void Initialize()
        {
            base.Initialize();
            leftButton.Initialize();
            rightButton.Initialize();

            leftButton.Frame = 0;
            rightButton.Frame = 1;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            leftButton.LoadContent(contentManager);
            rightButton.LoadContent(contentManager);
            if (AbilitySets != null)
            {
                AbilitySets.ForEach(set => set.LoadContent(contentManager));
                leftButton.Visible = rightButton.Visible = AbilitySets.Count > 1;
            }
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (AbilitySets != null)
            {
                TouchManager.OnTap(rightButton, () =>
                {
                    CurrentSet = (SetIndex + 1 == SetCount) ? AbilitySets[0] : AbilitySets[SetIndex + 1];
                    AdjustElements(null, EventArgs.Empty);
                });

                TouchManager.OnTap(leftButton, () =>
                {
                    CurrentSet = (SetIndex == 0) ? AbilitySets[SetCount - 1] : AbilitySets[SetIndex - 1];
                    AdjustElements(null, EventArgs.Empty);
                });

                CurrentSet.Update(gameTime);
            }
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            leftButton.Draw(renderer);
            rightButton.Draw(renderer);
            if (CurrentSet != null) CurrentSet.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            leftButton.UnloadContent(contentManager);
            rightButton.UnloadContent(contentManager);
            if (AbilitySets != null) AbilitySets.ForEach(set => set.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }

        #region Events
        private void Units_CurrentChanged(object sender, EventArgs e)
        {
            if (battle.State == BattleState.Waiting)
            {
                var contentManager = App.Game.Content;

                // The previous ability sets are not being discarded because their size
                // is highly negligible. If the UnloadContent method is called, then it
                // will dispose not only of the textures but also of the fonts, meaning
                // that the entire interface would have to be reloaded.

                AbilitySets = new List<AbilitySetView>();

                var abilitySets = battle.Units.Current.AbilitySets;
                abilitySets.ForEach(set =>
                {
                    var setView = new AbilitySetView(battle, set) { Parent = this };
                    setView.Initialize();
                    setView.LoadContent(contentManager);
                    AbilitySets.Add(setView);
                });

                CurrentSet = AbilitySets[0];
                AdjustElements(null, EventArgs.Empty);
            }
        }

        private void AdjustElements(object sender, EventArgs e)
        {
            if (CurrentSet != null)
            {
                var padding = 50f;
                
                var x = CurrentSet.Position.X - leftButton.Width - padding;
                var y = CurrentSet.Position.Y + (CurrentSet.Height - leftButton.Height) / 2;
                leftButton.Position = new Vector2(x, y);

                x = CurrentSet.Position.X + CurrentSet.Width + padding;
                y = CurrentSet.Position.Y + (CurrentSet.Height - leftButton.Height) / 2;
                rightButton.Position = new Vector2(x, y);
            }
        }
        #endregion
    }
}
