using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;

namespace DarkEnergy.Combat.Interface
{
    public class AbilitySetView : GameSystem
    {
        private Battle battle;
        private List<AbilityButton> abilityButtons;

        public float Padding { get; protected set; }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                
                for (var i = 0; i < Count; ++i)
                {
                    abilityButtons[i].Position = new Vector2(position.X + i * (abilityButtons[i].Width + Padding), position.Y);
                }
            }
        }

        public float Height
        {
            get
            {
                return Count > 1 ? abilityButtons[0].Height : 0;
            }
        }

        public float Width
        {
            get
            {
                return Count > 1 ? abilityButtons[Count - 1].Width + (Count - 1) * (abilityButtons[Count - 1].Width + Padding) : 0;
            }
        }

        public int Count { get { return abilityButtons.Count; } }

        public AbilitySetView(Battle battle, AbilitySet abilitySet)
        {
            this.battle = battle;
            Padding = 50;
            abilityButtons = new List<AbilityButton>();
            abilitySet.Abilities.ForEach(ability => abilityButtons.Add(new AbilityButton(battle, ability, abilitySet.Template) { Parent = this }));
            Loaded += AbilitySetView_Loaded;
        }

        public override void Initialize()
        {
            base.Initialize();
            abilityButtons.ForEach(button => button.Initialize());
        }

        public override void LoadContent(ContentManager contentManager)
        {
            abilityButtons.ForEach(button => button.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            abilityButtons.ForEach(button => button.Update(gameTime));
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            abilityButtons.ForEach(button => button.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            abilityButtons.ForEach(button => button.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }

        protected void AbilitySetView_Loaded(object sender, EventArgs e)
        {
            Position = new Vector2((Screen.NativeResolution.X - Width) / 2, Screen.NativeResolution.Y - Height - 4);
        }
    }
}
