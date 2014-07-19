using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.General
{
    public class AttributesPanel : GameSystem
    {
        private Text name;

        private Text strength, strengthValue;
        private Text intuition, intuitionValue;
        private Text reflexes, reflexesValue;
        private Text vitality, vitalityValue;
        private Text vigor, vigorValue;

        private Text health, protection, evasion, physicalDamage, magicalDamage, darkEnergyGenerated;

        public AttributesPanel()
        {
            name = new Text(FontStyle.CenturyGothic32, HorizontalAlignment.Center, -397, 127) { Parent = this, String = GameManager.Hero.Name };

            strength = new Text(FontStyle.CenturyGothic24, 60, 194) { Parent = this, String = Resources.Strings.Attribute_Strength };
            strengthValue = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -276, 190) { Parent = this, String = GameManager.Hero.Total.Strength.ToString() };

            intuition = new Text(FontStyle.CenturyGothic24, 60, 244) { Parent = this, String = Resources.Strings.Attribute_Intuition };
            intuitionValue = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -276, 240) { Parent = this, String = GameManager.Hero.Total.Intuition.ToString() };

            reflexes = new Text(FontStyle.CenturyGothic24, 60, 294) { Parent = this, String = Resources.Strings.Attribute_Reflexes };
            reflexesValue = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -276, 290) { Parent = this, String = GameManager.Hero.Total.Reflexes.ToString() };

            vitality = new Text(FontStyle.CenturyGothic24, 60, 344) { Parent = this, String = Resources.Strings.Attribute_Vitality };
            vitalityValue = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -276, 340) { Parent = this, String = GameManager.Hero.Total.Vitality.ToString() };

            vigor = new Text(FontStyle.CenturyGothic24, 60, 394) { Parent = this, String = Resources.Strings.Attribute_Vigor };
            vigorValue = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -276, 390) { Parent = this, String = GameManager.Hero.Total.Vigor.ToString() };

            health = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -480, 470) { Parent = this, String = GameManager.Hero.Total.CalculateMaximumHealth().ToString() };
            protection = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -480, 520) { Parent = this, String = (GameManager.Hero.Total.CalculateDefense() * 100).ToString("F1") + "%" };
            evasion = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -480, 570) { Parent = this, String = (GameManager.Hero.Total.CalculateEvasion(0) * 100).ToString("F1") + "%" };
            physicalDamage = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -270, 470) { Parent = this, String = GameManager.Hero.Total.CalculatePhysicalDamage().ToString() };
            magicalDamage = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -270, 520) { Parent = this, String = GameManager.Hero.Total.CalculateMagicalPower().ToString() };
            darkEnergyGenerated = new Text(FontStyle.Calibri30, HorizontalAlignment.Center, -270, 570) { Parent = this, String = GameManager.Hero.Total.CalculateDarkEnergyGenerated().ToString() };
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            name.Initialize();

            strength.Initialize();
            strengthValue.Initialize();
            intuition.Initialize();
            intuitionValue.Initialize();
            reflexes.Initialize();
            reflexesValue.Initialize();
            vitality.Initialize();
            vitalityValue.Initialize();
            vigor.Initialize();
            vigorValue.Initialize();

            health.Initialize();
            protection.Initialize();
            evasion.Initialize();
            physicalDamage.Initialize();
            magicalDamage.Initialize();
            darkEnergyGenerated.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            name.LoadContent(contentManager);

            strength.LoadContent(contentManager);
            strengthValue.LoadContent(contentManager);
            intuition.LoadContent(contentManager);
            intuitionValue.LoadContent(contentManager);
            reflexes.LoadContent(contentManager);
            reflexesValue.LoadContent(contentManager);
            vitality.LoadContent(contentManager);
            vitalityValue.LoadContent(contentManager);
            vigor.LoadContent(contentManager);
            vigorValue.LoadContent(contentManager);

            health.LoadContent(contentManager);
            protection.LoadContent(contentManager);
            evasion.LoadContent(contentManager);
            physicalDamage.LoadContent(contentManager);
            magicalDamage.LoadContent(contentManager);
            darkEnergyGenerated.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            name.Draw(renderer);

            strength.Draw(renderer);
            strengthValue.Draw(renderer);
            intuition.Draw(renderer);
            intuitionValue.Draw(renderer);
            reflexes.Draw(renderer);
            reflexesValue.Draw(renderer);
            vitality.Draw(renderer);
            vitalityValue.Draw(renderer);
            vigor.Draw(renderer);
            vigorValue.Draw(renderer);

            health.Draw(renderer);
            protection.Draw(renderer);
            evasion.Draw(renderer);
            physicalDamage.Draw(renderer);
            magicalDamage.Draw(renderer);
            darkEnergyGenerated.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            name.UnloadContent(contentManager);

            strength.UnloadContent(contentManager);
            strengthValue.UnloadContent(contentManager);
            intuition.UnloadContent(contentManager);
            intuitionValue.UnloadContent(contentManager);
            reflexes.UnloadContent(contentManager);
            reflexesValue.UnloadContent(contentManager);
            vitality.UnloadContent(contentManager);
            vitalityValue.UnloadContent(contentManager);
            vigor.UnloadContent(contentManager);
            vigorValue.UnloadContent(contentManager);

            health.UnloadContent(contentManager);
            protection.UnloadContent(contentManager);
            evasion.UnloadContent(contentManager);
            physicalDamage.UnloadContent(contentManager);
            magicalDamage.UnloadContent(contentManager);
            darkEnergyGenerated.UnloadContent(contentManager);

            base.UnloadContent(contentManager);
        }
    }
}
