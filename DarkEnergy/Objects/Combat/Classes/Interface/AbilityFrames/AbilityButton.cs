using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat.Interface
{
    public class AbilityButton : TexturedElement
    {
        private Battle battle;
        private TexturedElement icon, costBackground;
        private Text text;

        public new float Height { get { return 160; } }
        public string String { get { return text.String; } set { text.String = value; } }
        public Ability Ability { get; protected set; }
        public bool Enabled { get; protected set; }

        public AbilityButton(Battle battle, Ability ability, int templateId) : base(120, 120)
        {
            this.battle = battle;
            Ability = ability;
            Enabled = (Ability.RequiredLevel <= battle.Units.Current.Level);
            Path = @"Ability\Icons\Template" + templateId.ToString() + ".dds";
            
            icon = new TexturedElement(105, 105) { Parent = this, Path = @"Ability\Icons\" + (Enabled ? Ability.IconId.ToString() : "Locked") + ".dds" };
            costBackground = new TexturedElement(120, 52) { Parent = this, Path = @"Ability\Icons\CostBackground.dds" };

            var displayedText = Enabled ? Ability.DarkEnergyCost.ToString() + Resources.Strings.AbilityResource : Resources.Strings.AbilityDisabled;
            var color = (Enabled && battle.Units.Current != null && battle.Units.Current.DarkEnergy < Ability.DarkEnergyCost) ? Color.Red : Color.White;
            text = new Text(FontStyle.Calibri24) { Parent = this, Color = color, String = displayedText };

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }

        public override void Initialize()
        {
            base.Initialize();
            icon.Initialize();
            costBackground.Initialize();
            text.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            icon.LoadContent(contentManager);
            costBackground.LoadContent(contentManager);
            text.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                TouchManager.OnTap(this, () => battle.Units.EnqueueAbility(Ability));
            }
        }

        public override void Draw(Renderer renderer)
        {
            costBackground.Draw(renderer);
            base.Draw(renderer);
            icon.Draw(renderer);
            text.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            icon.UnloadContent(contentManager);
            costBackground.UnloadContent(contentManager);
            text.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            icon.Position = new Vector2(X + 7.5f, Y + 7.5f);
            costBackground.Position = new Vector2(X, Y + 108);
            text.Position = new Vector2(X + (120 - text.Width) / 2, Y + 120);
        }
    }
}
