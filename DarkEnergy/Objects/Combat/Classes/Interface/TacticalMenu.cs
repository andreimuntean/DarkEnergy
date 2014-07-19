using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat.Interface
{
    public class TacticalMenu : GameSystem
    {
        private Battle battle;

        public AbilityFrames AbilityFrames { get; protected set; }
        public UnitFrames UnitFrames { get; protected set; }

        public TacticalMenu(Battle battle)
        {
            Parent = this.battle = battle;
            AbilityFrames = new AbilityFrames(battle) { Parent = this };
            UnitFrames = new UnitFrames(battle) { Parent = this };
        }

        public override void Initialize()
        {
            base.Initialize();
            AbilityFrames.Initialize();
            UnitFrames.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            AbilityFrames.LoadContent(contentManager);
            UnitFrames.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            AbilityFrames.Update(gameTime);
            UnitFrames.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            AbilityFrames.Draw(renderer);
            UnitFrames.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            AbilityFrames.UnloadContent(contentManager);
            UnitFrames.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
