using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Inventory.Equipment
{
    public class AttributesPanel : GameSystem
    {
        private Text title;
        private List<Text> attributes;
        private Text offensive, offensiveValue;
        private Text defensive, defensiveValue;
        private TexturedElement offensiveElement, defensiveElement;

        protected DarkEnergy.Inventory.Equipment Equipment { get { return GameManager.Inventory.Equipment; } }

        public AttributesPanel()
        {
            title = new Text(FontStyle.CenturyGothic28, HorizontalAlignment.Center, -397, 137) { Parent = this, String = Resources.Strings.InventoryMenu_AttributeImprovements };

            attributes = new List<Text>()
            {
                new Text(FontStyle.CenturyGothic20, 60, 199) { Parent = this, String = Resources.Strings.Attribute_WeaponDamage },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, -276, 197) { Parent = this, String = Equipment.Attributes.WeaponDamage.ToString() },

                new Text(FontStyle.CenturyGothic20, 60, 244) { Parent = this, String = Resources.Strings.Attribute_Armor },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, -276, 242) { Parent = this, String = Equipment.Attributes.Armor.ToString() },

                new Text(FontStyle.CenturyGothic20, 60, 289) { Parent = this, String = Resources.Strings.Attribute_Strength },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, -276, 287) { Parent = this, String = Equipment.Attributes.Strength.ToString() },

                new Text(FontStyle.CenturyGothic20, 60, 334) { Parent = this, String = Resources.Strings.Attribute_Intuition },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, -276, 332) { Parent = this, String = Equipment.Attributes.Intuition.ToString() },

                new Text(FontStyle.CenturyGothic20, 60, 379) { Parent = this, String = Resources.Strings.Attribute_Reflexes },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, -276, 377) { Parent = this, String = Equipment.Attributes.Reflexes.ToString() },

                new Text(FontStyle.CenturyGothic20, 60, 424) { Parent = this, String = Resources.Strings.Attribute_Vitality },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, -276, 422) { Parent = this, String = Equipment.Attributes.Vitality.ToString() },

                new Text(FontStyle.CenturyGothic20, 60, 469) { Parent = this, String = Resources.Strings.Attribute_Vigor },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, -276, 467) { Parent = this, String = Equipment.Attributes.Vigor.ToString() },
            };

            offensive = new Text(FontStyle.Calibri20, HorizontalAlignment.Center, -463.5f, 546) { Parent = this, String = Resources.Strings.ElementOffensive };
            offensiveValue = new Text(FontStyle.Calibri18, HorizontalAlignment.Center, -463.5f, 573) { Parent = this, String = Resistances.GetName(GameManager.Hero.OffensiveElement) };
            offensiveElement = new TexturedElement(54, 54, 55, 545) { Parent = this, Path = @"Interface\Character\Elements.dds" };

            defensive = new Text(FontStyle.Calibri20, HorizontalAlignment.Center, -326.5f, 546) { Parent = this, String = Resources.Strings.ElementDefensive };
            defensiveValue = new Text(FontStyle.Calibri18, HorizontalAlignment.Center, -326.5f, 573) { Parent = this, String = Resistances.GetName(GameManager.Hero.DefensiveElement) };
            defensiveElement = new TexturedElement(54, 54, 375, 545) { Parent = this, Path = @"Interface\Character\Elements.dds" };
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            title.Initialize();
            attributes.ForEach(attribute => attribute.Initialize());

            offensive.Initialize();
            offensiveValue.Initialize();
            offensiveElement.Initialize();

            defensive.Initialize();
            defensiveValue.Initialize();
            defensiveElement.Initialize();

            offensiveElement.Scale = new Vector2(1.111f, 1.111f);
            offensiveElement.Frame = (int)(GameManager.Hero.OffensiveElement);

            defensiveElement.Scale = new Vector2(1.111f, 1.111f);
            defensiveElement.Frame = (int)(GameManager.Hero.DefensiveElement);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            title.LoadContent(contentManager);
            attributes.ForEach(attribute => attribute.LoadContent(contentManager));

            offensive.LoadContent(contentManager);
            offensiveValue.LoadContent(contentManager);
            offensiveElement.LoadContent(contentManager);

            defensive.LoadContent(contentManager);
            defensiveValue.LoadContent(contentManager);
            defensiveElement.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            title.Draw(renderer);
            attributes.ForEach(attribute => attribute.Draw(renderer));

            offensive.Draw(renderer);
            offensiveValue.Draw(renderer);
            offensiveElement.Draw(renderer);

            defensive.Draw(renderer);
            defensiveValue.Draw(renderer);
            defensiveElement.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            title.UnloadContent(contentManager);
            attributes.ForEach(attribute => attribute.UnloadContent(contentManager));

            offensive.LoadContent(contentManager);
            offensiveValue.LoadContent(contentManager);
            offensiveElement.LoadContent(contentManager);

            defensive.LoadContent(contentManager);
            defensiveValue.LoadContent(contentManager);
            defensiveElement.LoadContent(contentManager);

            base.UnloadContent(contentManager);
        }
    }
}
