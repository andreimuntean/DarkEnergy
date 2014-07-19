using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.Abilities
{
    public class AbilityWidePanel : TexturedElement
    {
        private TexturedElement icon, template;
        private Text name, rank, resources, description;

        public bool Locked { get { return GameManager.Hero.Level < Ability.RequiredLevel; } }

        public Ability Ability { get; protected set; }

        public AbilityWidePanel(Ability ability, int template, bool currentRank = true) : base(595, 420)
        {
            Ability = ability;

            Path = @"Interface\World\Menu\WidePanel.dds";
            icon = new TexturedElement(105, 105) { Parent = this, Path = @"Ability\Icons\" + (Locked ? "Locked" : Ability.IconId.ToString()) + ".dds" };
            this.template = new TexturedElement(120, 120) { Parent = this, Path = @"Ability\Icons\Template" + template.ToString() + ".dds" };
            name = new Text(FontStyle.CenturyGothic32) { Parent = this, String = Ability.Name };
            rank = new Text(FontStyle.CenturyGothic20) { Parent = this, String = Resources.Strings.AbilityRank + " " + Ability.Rank.ToString() + " " + (currentRank ? Resources.Strings.AbilityRankCurrent : Locked ? "(" + Resources.Strings.Level + " " + ability.RequiredLevel + ")" : Resources.Strings.AbilityRankNext) };
            resources = new Text(FontStyle.CenturyGothic20) { Parent = this, String = Ability.DarkEnergyCost.ToString() + " " + Resources.Strings.AbilityResourceFull };
            description = new Text(FontStyle.Calibri20) { Parent = this, MaxWidth = 555 };

            Loaded += AdjustElements;
            PositionChanged += AdjustElements;
        }

        public override void Initialize()
        {
            base.Initialize();
            icon.Initialize();
            template.Initialize();
            name.Initialize();
            rank.Initialize();
            resources.Initialize();
            description.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            icon.LoadContent(contentManager);
            template.LoadContent(contentManager);
            name.LoadContent(contentManager);
            rank.LoadContent(contentManager);
            resources.LoadContent(contentManager);
            description.LoadContent(contentManager);
            description.String = Ability.GetDescription();
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            template.Draw(renderer);
            icon.Draw(renderer);
            name.Draw(renderer);
            rank.Draw(renderer);
            resources.Draw(renderer);
            description.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            template.UnloadContent(contentManager);
            icon.UnloadContent(contentManager);
            rank.UnloadContent(contentManager);
            resources.UnloadContent(contentManager);
            description.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            template.Position = new Vector2(X + 20, Y + 20);
            icon.Position = new Vector2(X + 27.5f, Y + 27.5f);
            name.Position = new Vector2(X + 160, Y + 18);
            rank.Position = new Vector2(X + 160, Y + 70);
            resources.Position = new Vector2(X + 160, Y + 105);
            description.Position = new Vector2(X + 20, Y + 150);
        }
    }
}
