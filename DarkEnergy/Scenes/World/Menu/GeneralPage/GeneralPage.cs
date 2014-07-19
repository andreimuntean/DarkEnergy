using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Interface;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Scenes.World.Menu.Character.General;

namespace DarkEnergy.Scenes.World.Menu.Character
{
    public class GeneralPage : GameSystem
    {
        private TexturedElement background;
        private Hero hero;
        private ExperienceBar experienceBar; 
        private AttributesPanel attributesPanel;
        private ElementsPanel resistancesPanel;

        public GeneralPage()
        {
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\GeneralBackground.dds" };
            experienceBar = new ExperienceBar() { Parent = this, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom, Offset = new Vector2(0, -10) };
            attributesPanel = new AttributesPanel() { Parent = this };
            resistancesPanel = new ElementsPanel() { Parent = this };
            hero = new Hero() { Parent = this, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Middle };
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            experienceBar.Initialize();
            attributesPanel.Initialize();
            resistancesPanel.Initialize();
            hero.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            experienceBar.LoadContent(contentManager);
            attributesPanel.LoadContent(contentManager);
            resistancesPanel.LoadContent(contentManager);
            hero.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            resistancesPanel.Update(gameTime);
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            attributesPanel.Draw(renderer);
            resistancesPanel.Draw(renderer);
            experienceBar.Draw(renderer);
            hero.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            attributesPanel.UnloadContent(contentManager);
            resistancesPanel.UnloadContent(contentManager);
            experienceBar.UnloadContent(contentManager);
            hero.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
