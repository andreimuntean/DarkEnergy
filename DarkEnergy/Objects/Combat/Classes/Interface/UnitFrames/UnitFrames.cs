using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat.Interface
{
    public class UnitFrames : GameSystem
    {
        private Battle battle;
        private UnitFrame leftUnit;
        private UnitFrame rightUnit;

        public UnitFrames(Battle battle)
        {
            this.battle = battle;
            leftUnit = new UnitFrame() { Parent = this, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Offset = new Vector2(-277, 12) };
            rightUnit = new UnitFrame() { Parent = this, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top, Offset = new Vector2(277, 12) };

            battle.Units.CurrentChanged += Units_Changed;
            battle.Units.TargetChanged += Units_Changed;
        }

        public void Refresh()
        {
            leftUnit.Refresh();
            rightUnit.Refresh();
        }

        public override void Initialize()
        {
            base.Initialize();
            leftUnit.Initialize();
            rightUnit.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            leftUnit.LoadContent(contentManager);
            rightUnit.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            leftUnit.Draw(renderer);
            rightUnit.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            leftUnit.UnloadContent(contentManager);
            rightUnit.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        private void Units_Changed(object sender, EventArgs e)
        {
            var current = battle.Units.Current;
            var target = battle.Units.Target;
            
            if (!(current is Enemy) && !(target is Enemy))
            {
                leftUnit.Source = current;
                rightUnit.Source = target;
            }
            else if (!(current is Enemy) && target is Enemy)
            {
                leftUnit.Source = current;
                rightUnit.Source = target;
            }
            else if (current is Enemy && !(target is Enemy))
            {
                leftUnit.Source = target;
                rightUnit.Source = current;
            }
            else if (current is Enemy && target is Enemy)
            {
                leftUnit.Source = target;
                rightUnit.Source = current;
            }
        }
        #endregion
    }
}
