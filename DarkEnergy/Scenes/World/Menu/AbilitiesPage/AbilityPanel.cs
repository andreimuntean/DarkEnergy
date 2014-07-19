using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.Abilities
{
    public class AbilityPanel : TexturedElement
    {
        private int setIndex;
        private TexturedElement icon, template;
        private Text name, rank;

        public bool Locked { get { return GameManager.Hero.Level < Ability.RequiredLevel; } }
        public Ability Ability { get; protected set; }

        public AbilityPanel(AbilitySet set, int abilityIndex, int setIndex) : base(280, 290)
        {
            Ability = set.Abilities[abilityIndex];
            this.setIndex = setIndex;

            Path = @"Interface\World\Menu\Panel.dds";
            template = new TexturedElement(120, 120) { Parent = this, Path = @"Ability\Icons\Template" + set.Template.ToString() + ".dds" };
            icon = new TexturedElement(105, 105) { Parent = this, Path = @"Ability\Icons\" + (Locked ? "Locked" : Ability.IconId.ToString()) + ".dds" };
            name = new Text(FontStyle.CenturyGothic24) { Parent = this, String = Locked ? Resources.Strings.AbilityDisabled : Ability.Name };
            rank = new Text(FontStyle.CenturyGothic16) { Parent = this, String = Locked ? Resources.Strings.Level.ToUpperInvariant() + " " + Ability.RequiredLevel : Resources.Strings.AbilityRank + " " + Ability.Rank.ToString() };

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
        }

        public override void LoadContent(ContentManager contentManager)
        {
            icon.LoadContent(contentManager);
            template.LoadContent(contentManager);
            name.LoadContent(contentManager);
            rank.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Locked)
            {
                TouchManager.OnTap(this, () =>
                {
                    var worldScene = (Parent.Parent as CharacterMenu).WorldScene;
                    SceneManager.Play(new CharacterMenu(worldScene, 4, Ability, setIndex));
                });
            }
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
            template.Draw(renderer);
            icon.Draw(renderer);
            name.Draw(renderer);
            rank.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            template.UnloadContent(contentManager);
            icon.UnloadContent(contentManager);
            name.UnloadContent(contentManager);
            rank.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            template.Position = new Vector2(X + 80, Y + 40);
            icon.Position = new Vector2(X + 87.5f, Y + 47.5f);
            name.Position = new Vector2(X + (Width - name.Width) / 2, Y + 175);
            rank.Position = new Vector2(X + (Width - rank.Width) / 2, Y + 220);
        }
    }
}
