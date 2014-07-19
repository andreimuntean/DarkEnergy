using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Scenes.World.Menu.Character.Attributes;

namespace DarkEnergy.Scenes.World.Menu.Character
{
    public class AttributesPage : GameSystem
    {
        private TexturedElement background;
        private AttributesPanel attributesPanel;
        private EffectsPanel effectsPanel;

        public int AttributePoints { get; set; }
        public Characters.Attributes AttributeChanges { get; set; }
        public Characters.Attributes Preview { get { return GameManager.Hero.Base + AttributeChanges; } }

        public AttributesPage()
        {
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\AttributesBackground.dds" };
            attributesPanel = new AttributesPanel(this);
            effectsPanel = new EffectsPanel(this);
            
            AttributePoints = GameManager.Hero.AttributePoints;
            AttributeChanges = Characters.Attributes.Zero; ;
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            attributesPanel.Initialize();
            effectsPanel.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            attributesPanel.LoadContent(contentManager);
            effectsPanel.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            attributesPanel.Update(gameTime);
            effectsPanel.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            attributesPanel.Draw(renderer);
            effectsPanel.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            attributesPanel.UnloadContent(contentManager);
            effectsPanel.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
