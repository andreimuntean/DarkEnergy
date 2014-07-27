using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.Attributes
{
    public class EffectsPanel : GameSystem
    {
        private AttributesPage main;
        private List<Text> data;
        private CalligraphedImage confirmChanges;

        public EffectsPanel(AttributesPage main)
        {
            this.main = main;

            data = new List<Text>()
            {
                new Text(FontStyle.Calibri24, 784, 160) { Parent = this, String = Resources.Strings.DescriptionHealth },
                new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 156) { Parent = this, String = GameManager.Hero.Base.CalculateMaximumHealth().ToString() },

                new Text(FontStyle.Calibri24, 784, 210) { Parent = this, String = Resources.Strings.DescriptionPhysicalDamage },
                new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 206) { Parent = this, String = GameManager.Hero.Base.CalculatePhysicalDamage().ToString() },

                new Text(FontStyle.Calibri24, 784, 260) { Parent = this, String = Resources.Strings.DescriptionMagicalDamage },
                new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 256) { Parent = this, String = GameManager.Hero.Base.CalculateMagicalPower().ToString() },

                new Text(FontStyle.Calibri24, 784, 310) { Parent = this, String = Resources.Strings.DescriptionProtection },
                new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 306) { Parent = this, String = (GameManager.Hero.Base.CalculateDefense() * 100).ToString("F1") + "%" },

                new Text(FontStyle.Calibri24, 784, 360) { Parent = this, String = Resources.Strings.DescriptionEvasion },
                new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 356) { Parent = this, String = (GameManager.Hero.Base.CalculateEvasion(0) * 100).ToString("F1") + "%" },

                new Text(FontStyle.Calibri24, 784, 410) { Parent = this, String = Resources.Strings.DescriptionDarkEnergy },
                new Text(FontStyle.Calibri28, HorizontalAlignment.Center, 515, 406) { Parent = this, String = GameManager.Hero.Base.CalculateDarkEnergyGenerated().ToString() },

                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 348, 485) { Parent = this, String = Resources.Strings.RemainingAttributePoints.Replace("$", GameManager.Hero.AttributePoints.ToString()) }
            };

            confirmChanges = new CalligraphedImage(316, 80, HorizontalAlignment.Center, 348, 540, Resources.Strings.ConfirmChanges, FontStyle.Calibri30, Color.White, Vector2.Zero) { Parent = this, Path = @"Interface\DefaultButton.dds" };
        }
        
        public override void Initialize()
        {
            base.Initialize();
            data.ForEach(datum => datum.Initialize());
            confirmChanges.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            data.ForEach(datum => datum.LoadContent(contentManager));
            confirmChanges.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            data[1].String = main.Preview.CalculateMaximumHealth().ToString();
            data[3].String = main.Preview.CalculatePhysicalDamage().ToString();
            data[5].String = main.Preview.CalculateMagicalPower().ToString();
            data[7].String = (main.Preview.CalculateDefense() * 100).ToString("F1") + "%";
            data[9].String = (main.Preview.CalculateEvasion(0) * 100).ToString("F1") + "%";
            data[11].String = main.Preview.CalculateDarkEnergyGenerated().ToString();
            data[12].String = Resources.Strings.RemainingAttributePoints.Replace("$", main.AttributePoints.ToString());

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
            data.ForEach(datum => datum.Draw(renderer));
            confirmChanges.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            data.ForEach(datum => datum.UnloadContent(contentManager));
            confirmChanges.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
