using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.Attributes
{
    public class EffectsPanel : GameSystem
    {
        private AttributesPage main;

        private Text health, healthValue;
        private Text physical, physicalValue;
        private Text magical, magicalValue;
        private Text protection, protectionValue;
        private Text evasion, evasionValue;
        private Text darkEnergy, darkEnergyValue;

        private Text unspentPoints;
        private CalligraphedImage confirmChanges;

        public EffectsPanel(AttributesPage main)
        {
            this.main = main;

            health = new Text(FontStyle.Calibri24, 784, 160) { Parent = this, String = Resources.Strings.DescriptionHealth };
            healthValue = new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 156) { Parent = this, String = GameManager.Hero.Base.CalculateMaximumHealth().ToString() };

            physical = new Text(FontStyle.Calibri24, 784, 210) { Parent = this, String = Resources.Strings.DescriptionPhysicalDamage };
            physicalValue = new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 206) { Parent = this, String = GameManager.Hero.Base.CalculatePhysicalDamage().ToString() };

            magical = new Text(FontStyle.Calibri24, 784, 260) { Parent = this, String = Resources.Strings.DescriptionMagicalDamage };
            magicalValue = new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 256) { Parent = this, String = GameManager.Hero.Base.CalculateMagicalPower().ToString() };

            protection = new Text(FontStyle.Calibri24, 784, 310) { Parent = this, String = Resources.Strings.DescriptionProtection };
            protectionValue = new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 306) { Parent = this, String = (GameManager.Hero.Base.CalculateDefense() * 100).ToString("F1") + "%" };

            evasion = new Text(FontStyle.Calibri24, 784, 360) { Parent = this, String = Resources.Strings.DescriptionEvasion };
            evasionValue = new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 356) { Parent = this, String = (GameManager.Hero.Base.CalculateEvasion(0) * 100).ToString("F1") + "%" };

            darkEnergy = new Text(FontStyle.Calibri24, 784, 410) { Parent = this, String = Resources.Strings.DescriptionDarkEnergy };
            darkEnergyValue = new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 406) { Parent = this, String = GameManager.Hero.Base.CalculateDarkEnergyGenerated().ToString() };

            unspentPoints = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 348, 485) { Parent = this, String = Resources.Strings.RemainingAttributePoints.Replace("$", GameManager.Hero.AttributePoints.ToString()) };
            confirmChanges = new CalligraphedImage(316, 80, HorizontalAlignment.Center, 348, 540, Resources.Strings.ConfirmChanges, FontStyle.Calibri30, Color.White, Vector2.Zero) { Parent = this, Path = @"Interface\DefaultButton.dds" };
        }
        
        public override void Initialize()
        {
            base.Initialize();

            health.Initialize();
            healthValue.Initialize();
            physical.Initialize();
            physicalValue.Initialize();
            magical.Initialize();
            magicalValue.Initialize();
            protection.Initialize();
            protectionValue.Initialize();
            evasion.Initialize();
            evasionValue.Initialize();
            darkEnergy.Initialize();
            darkEnergyValue.Initialize();

            unspentPoints.Initialize();
            confirmChanges.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            health.LoadContent(contentManager);
            healthValue.LoadContent(contentManager);
            physical.LoadContent(contentManager);
            physicalValue.LoadContent(contentManager);
            magical.LoadContent(contentManager);
            magicalValue.LoadContent(contentManager);
            protection.LoadContent(contentManager);
            protectionValue.LoadContent(contentManager);
            evasion.LoadContent(contentManager);
            evasionValue.LoadContent(contentManager);
            darkEnergy.LoadContent(contentManager);
            darkEnergyValue.LoadContent(contentManager);

            unspentPoints.LoadContent(contentManager);
            confirmChanges.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            healthValue.String = main.Preview.CalculateMaximumHealth().ToString();
            physicalValue.String = main.Preview.CalculatePhysicalDamage().ToString();
            magicalValue.String = main.Preview.CalculateMagicalPower().ToString();
            protectionValue.String = (main.Preview.CalculateDefense() * 100).ToString("F1") + "%";
            evasionValue.String = (main.Preview.CalculateEvasion(0) * 100).ToString("F1") + "%";
            darkEnergyValue.String = main.Preview.CalculateDarkEnergyGenerated().ToString();
            unspentPoints.String = Resources.Strings.RemainingAttributePoints.Replace("$", main.AttributePoints.ToString());

            TouchManager.OnTap(confirmChanges, () =>
            {
                if (main.AttributeChanges != Characters.Attributes.Zero)
                {
                    GameManager.Hero.Base += main.AttributeChanges;
                    GameManager.Hero.AttributePoints = main.AttributePoints;
                    main.AttributeChanges = Characters.Attributes.Zero;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });
        }

        public override void Draw(Renderer renderer)
        {
            health.Draw(renderer);
            healthValue.Draw(renderer);
            physical.Draw(renderer);
            physicalValue.Draw(renderer);
            magical.Draw(renderer);
            magicalValue.Draw(renderer);
            protection.Draw(renderer);
            protectionValue.Draw(renderer);
            evasion.Draw(renderer);
            evasionValue.Draw(renderer);
            darkEnergy.Draw(renderer);
            darkEnergyValue.Draw(renderer);

            unspentPoints.Draw(renderer);
            confirmChanges.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            health.UnloadContent(contentManager);
            healthValue.UnloadContent(contentManager);
            physical.UnloadContent(contentManager);
            physicalValue.UnloadContent(contentManager);
            magical.UnloadContent(contentManager);
            magicalValue.UnloadContent(contentManager);
            protection.UnloadContent(contentManager);
            protectionValue.UnloadContent(contentManager);
            evasion.UnloadContent(contentManager);
            evasionValue.UnloadContent(contentManager);
            darkEnergy.UnloadContent(contentManager);
            darkEnergyValue.UnloadContent(contentManager);

            unspentPoints.UnloadContent(contentManager);
            confirmChanges.UnloadContent(contentManager);

            base.UnloadContent(contentManager);
        }
    }
}
