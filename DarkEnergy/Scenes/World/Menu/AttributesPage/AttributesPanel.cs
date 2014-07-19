using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.Attributes
{
    public class AttributesPanel : GameSystem
    {
        private AttributesPage main;

        private Color defaultColor, changeColor;

        private Text strength, strengthValue;
        private Text intuition, intuitionValue;
        private Text reflexes, reflexesValue;
        private Text vitality, vitalityValue;
        private Text vigor, vigorValue;

        private TexturedElement strengthSubtract, strengthAdd;
        private TexturedElement intuitionSubtract, intuitionAdd;
        private TexturedElement reflexesSubtract, reflexesAdd;
        private TexturedElement vitalitySubtract, vitalityAdd;
        private TexturedElement vigorSubtract, vigorAdd;

        public AttributesPanel(AttributesPage main)
        {
            this.main = main;
            changeColor = new Color(0, 255, 0);
            defaultColor = new Color(255, 255, 255);

            strength = new Text(FontStyle.CenturyGothic24, 72, 159) { Parent = this, String = Resources.Strings.Attribute_Strength };
            strengthValue = new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 153) { Parent = this, String = GameManager.Hero.Base.Strength.ToString() };
            strengthSubtract = new TexturedElement(80, 80, 260, 140) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };
            strengthAdd = new TexturedElement(80, 80, 460, 140) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };

            intuition = new Text(FontStyle.CenturyGothic24, 72, 259) { Parent = this, String = Resources.Strings.Attribute_Intuition };
            intuitionValue = new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 253) { Parent = this, String = GameManager.Hero.Base.Intuition.ToString() };
            intuitionSubtract = new TexturedElement(80, 80, 260, 240) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };
            intuitionAdd = new TexturedElement(80, 80, 460, 240) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };

            reflexes = new Text(FontStyle.CenturyGothic24, 72, 359) { Parent = this, String = Resources.Strings.Attribute_Reflexes };
            reflexesValue = new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 353) { Parent = this, String = GameManager.Hero.Base.Reflexes.ToString() };
            reflexesSubtract = new TexturedElement(80, 80, 260, 340) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };
            reflexesAdd = new TexturedElement(80, 80, 460, 340) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };

            vitality = new Text(FontStyle.CenturyGothic24, 72, 459) { Parent = this, String = Resources.Strings.Attribute_Vitality };
            vitalityValue = new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 453) { Parent = this, String = GameManager.Hero.Base.Vitality.ToString() };
            vitalitySubtract = new TexturedElement(80, 80, 260, 440) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };
            vitalityAdd = new TexturedElement(80, 80, 460, 440) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };

            vigor = new Text(FontStyle.CenturyGothic24, 72, 559) { Parent = this, String = Resources.Strings.Attribute_Vigor };
            vigorValue = new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 553) { Parent = this, String = GameManager.Hero.Base.Vigor.ToString() };
            vigorSubtract = new TexturedElement(80, 80, 260, 540) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };
            vigorAdd = new TexturedElement(80, 80, 460, 540) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" };
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            strength.Initialize();
            strengthValue.Initialize();
            strengthAdd.Initialize();
            strengthSubtract.Initialize();
            strengthSubtract.Frame = 1;

            intuition.Initialize();
            intuitionValue.Initialize();
            intuitionAdd.Initialize();
            intuitionSubtract.Initialize();
            intuitionSubtract.Frame = 1;

            reflexes.Initialize();
            reflexesValue.Initialize();
            reflexesAdd.Initialize();
            reflexesSubtract.Initialize();
            reflexesSubtract.Frame = 1;

            vitality.Initialize();
            vitalityValue.Initialize();
            vitalityAdd.Initialize();
            vitalitySubtract.Initialize();
            vitalitySubtract.Frame = 1;

            vigor.Initialize();
            vigorValue.Initialize();
            vigorAdd.Initialize();
            vigorSubtract.Initialize();
            vigorSubtract.Frame = 1;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            strength.LoadContent(contentManager);
            strengthValue.LoadContent(contentManager);
            strengthAdd.LoadContent(contentManager);
            strengthSubtract.LoadContent(contentManager);

            intuition.LoadContent(contentManager);
            intuitionValue.LoadContent(contentManager);
            intuitionAdd.LoadContent(contentManager);
            intuitionSubtract.LoadContent(contentManager);

            reflexes.LoadContent(contentManager);
            reflexesValue.LoadContent(contentManager);
            reflexesAdd.LoadContent(contentManager);
            reflexesSubtract.LoadContent(contentManager);

            vitality.LoadContent(contentManager);
            vitalityValue.LoadContent(contentManager);
            vitalityAdd.LoadContent(contentManager);
            vitalitySubtract.LoadContent(contentManager);

            vigor.LoadContent(contentManager);
            vigorValue.LoadContent(contentManager);
            vigorAdd.LoadContent(contentManager);
            vigorSubtract.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            TouchManager.OnTap(strengthAdd, () =>
            {
                if (main.AttributePoints > 0)
                {
                    main.AttributePoints -= 1;
                    main.AttributeChanges.Strength += 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(strengthSubtract, () =>
            {
                if (main.AttributeChanges.Strength > 0)
                {
                    main.AttributePoints += 1;
                    main.AttributeChanges.Strength -= 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(intuitionAdd, () =>
            {
                if (main.AttributePoints > 0)
                {
                    main.AttributePoints -= 1;
                    main.AttributeChanges.Intuition += 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(intuitionSubtract, () =>
            {
                if (main.AttributeChanges.Intuition > 0)
                {
                    main.AttributePoints += 1;
                    main.AttributeChanges.Intuition -= 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(reflexesAdd, () =>
            {
                if (main.AttributePoints > 0)
                {
                    main.AttributePoints -= 1;
                    main.AttributeChanges.Reflexes += 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(reflexesSubtract, () =>
            {
                if (main.AttributeChanges.Reflexes > 0)
                {
                    main.AttributePoints += 1;
                    main.AttributeChanges.Reflexes -= 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(vitalityAdd, () =>
            {
                if (main.AttributePoints > 0)
                {
                    main.AttributePoints -= 1;
                    main.AttributeChanges.Vitality += 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(vitalitySubtract, () =>
            {
                if (main.AttributeChanges.Vitality > 0)
                {
                    main.AttributePoints += 1;
                    main.AttributeChanges.Vitality -= 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(vigorAdd, () =>
            {
                if (main.AttributePoints > 0)
                {
                    main.AttributePoints -= 1;
                    main.AttributeChanges.Vigor += 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            TouchManager.OnTap(vigorSubtract, () =>
            {
                if (main.AttributeChanges.Vigor > 0)
                {
                    main.AttributePoints += 1;
                    main.AttributeChanges.Vigor -= 1;
                }
                else PhoneEffectsManager.Play(PhoneEffect.Vibration);
            });

            strengthValue.String = main.Preview.Strength.ToString();
            intuitionValue.String = main.Preview.Intuition.ToString();
            reflexesValue.String = main.Preview.Reflexes.ToString();
            vitalityValue.String = main.Preview.Vitality.ToString();
            vigorValue.String = main.Preview.Vigor.ToString();

            if (main.AttributeChanges.Strength != 0) strengthValue.Color = changeColor;
            else strengthValue.Color = defaultColor;

            if (main.AttributeChanges.Intuition != 0) intuitionValue.Color = changeColor;
            else intuitionValue.Color = defaultColor;

            if (main.AttributeChanges.Reflexes != 0) reflexesValue.Color = changeColor;
            else reflexesValue.Color = defaultColor;

            if (main.AttributeChanges.Vitality != 0) vitalityValue.Color = changeColor;
            else vitalityValue.Color = defaultColor;

            if (main.AttributeChanges.Vigor != 0) vigorValue.Color = changeColor;
            else vigorValue.Color = defaultColor;
        }

        public override void Draw(Renderer renderer)
        {
            strength.Draw(renderer);
            strengthValue.Draw(renderer);
            strengthAdd.Draw(renderer);
            strengthSubtract.Draw(renderer);

            intuition.Draw(renderer);
            intuitionValue.Draw(renderer);
            intuitionAdd.Draw(renderer);
            intuitionSubtract.Draw(renderer);

            reflexes.Draw(renderer);
            reflexesValue.Draw(renderer);
            reflexesAdd.Draw(renderer);
            reflexesSubtract.Draw(renderer);

            vitality.Draw(renderer);
            vitalityValue.Draw(renderer);
            vitalityAdd.Draw(renderer);
            vitalitySubtract.Draw(renderer);

            vigor.Draw(renderer);
            vigorValue.Draw(renderer);
            vigorAdd.Draw(renderer);
            vigorSubtract.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            strength.UnloadContent(contentManager);
            strengthValue.UnloadContent(contentManager);
            strengthAdd.UnloadContent(contentManager);
            strengthSubtract.UnloadContent(contentManager);

            intuition.UnloadContent(contentManager);
            intuitionValue.UnloadContent(contentManager);
            intuitionAdd.UnloadContent(contentManager);
            intuitionSubtract.UnloadContent(contentManager);

            reflexes.UnloadContent(contentManager);
            reflexesValue.UnloadContent(contentManager);
            reflexesAdd.UnloadContent(contentManager);
            reflexesSubtract.UnloadContent(contentManager);

            vitality.UnloadContent(contentManager);
            vitalityValue.UnloadContent(contentManager);
            vitalityAdd.UnloadContent(contentManager);
            vitalitySubtract.UnloadContent(contentManager);

            vigor.UnloadContent(contentManager);
            vigorValue.UnloadContent(contentManager);
            vigorAdd.UnloadContent(contentManager);
            vigorSubtract.UnloadContent(contentManager);

            base.UnloadContent(contentManager);
        }
    }
}
