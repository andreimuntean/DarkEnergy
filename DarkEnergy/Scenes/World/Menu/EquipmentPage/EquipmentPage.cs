using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Scenes.World.Menu.Inventory.Equipment;

namespace DarkEnergy.Scenes.World.Menu.Inventory
{
    public class EquipmentPage : GameSystem
    {
        private TexturedElement background;
        private AttributesPanel attributesPanel;
        private EquipmentPanel equipmentPanel;

        public EquipmentPage()
        {
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"Interface\World\Menu\EquipmentBackground.dds" };
            attributesPanel = new AttributesPanel() { Parent = this };
            equipmentPanel = new EquipmentPanel() { Parent = this };
        }

        public override void Initialize()
        {
            base.Initialize();
            background.Initialize();
            attributesPanel.Initialize();
            equipmentPanel.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            attributesPanel.LoadContent(contentManager);
            equipmentPanel.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            attributesPanel.Draw(renderer);
            equipmentPanel.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            attributesPanel.UnloadContent(contentManager);
            equipmentPanel.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
