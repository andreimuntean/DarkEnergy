using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Inventory;

namespace DarkEnergy.Scenes.World.Menu.Inventory.Equipment
{
    public class EquipmentPanel : GameSystem
    {
        private List<Text> items;

        protected DarkEnergy.Inventory.Equipment Equipment { get { return GameManager.Inventory.Equipment; } }

        public EquipmentPanel()
        {
            items = new List<Text>();
            var equipment = Equipment.ToList();

            for (int i = 0; i < 10; ++i)
            {
                items.Add(new Text(FontStyle.Calibri24, 759, 147.1f + i * 47.5f) { Parent = this, String = (equipment[i] == null ? Resources.Strings.ItemNone : equipment[i].Name)  });
                items.Add(new Text(FontStyle.Calibri20, HorizontalAlignment.Right, -67, 148.5f + i * 47.5f) { Parent = this, String = EquippableItem.GetName((EquipmentSlot)(i)), Color = new Color(0.9f, 0.8f, 1.0f, 1.0f) });
            }
        }
        
        public override void Initialize()
        {
            base.Initialize();
            items.ForEach(item => item.Initialize());
        }

        public override void LoadContent(ContentManager contentManager)
        {
            items.ForEach(item => item.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            items.ForEach(item => item.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            base.UnloadContent(contentManager);
            items.ForEach(item => item.UnloadContent(contentManager));
        }
    }
}
