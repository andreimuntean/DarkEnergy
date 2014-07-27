using System.Collections.Generic;
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
        private List<GameObject> interfaceObjects;

        public AttributesPanel(AttributesPage main)
        {
            this.main = main;
            changeColor = new Color(0, 255, 0);
            defaultColor = new Color(255, 255, 255);

            interfaceObjects = new List<GameObject>()
            {
                new Text(FontStyle.CenturyGothic24, 72, 159) { Parent = this, String = Resources.Strings.Attribute_Strength },
                new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 153) { Parent = this, String = GameManager.Hero.Base.Strength.ToString() },
                new TexturedElement(80, 80, 260, 140) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },
                new TexturedElement(80, 80, 460, 140) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },

                new Text(FontStyle.CenturyGothic24, 72, 259) { Parent = this, String = Resources.Strings.Attribute_Intuition },
                new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 253) { Parent = this, String = GameManager.Hero.Base.Intuition.ToString() },
                new TexturedElement(80, 80, 260, 240) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },
                new TexturedElement(80, 80, 460, 240) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },

                new Text(FontStyle.CenturyGothic24, 72, 359) { Parent = this, String = Resources.Strings.Attribute_Reflexes },
                new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 353) { Parent = this, String = GameManager.Hero.Base.Reflexes.ToString() },
                new TexturedElement(80, 80, 260, 340) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },
                new TexturedElement(80, 80, 460, 340) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },

                new Text(FontStyle.CenturyGothic24, 72, 459) { Parent = this, String = Resources.Strings.Attribute_Vitality },
                new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 453) { Parent = this, String = GameManager.Hero.Base.Vitality.ToString() },
                new TexturedElement(80, 80, 260, 440) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },
                new TexturedElement(80, 80, 460, 440) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },

                new Text(FontStyle.CenturyGothic24, 72, 559) { Parent = this, String = Resources.Strings.Attribute_Vigor },
                new Text(FontStyle.Calibri32, HorizontalAlignment.Center, -240, 553) { Parent = this, String = GameManager.Hero.Base.Vigor.ToString() },
                new TexturedElement(80, 80, 260, 540) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" },
                new TexturedElement(80, 80, 460, 540) { Parent = this, Path = @"Interface\AddSubtractButtons.dds" }
            };
        }
        
        public override void Initialize()
        {
            base.Initialize();
            interfaceObjects.ForEach(interfaceObject => interfaceObject.Initialize());
            (interfaceObjects[2] as TexturedElement).Frame = 1;
            (interfaceObjects[6] as TexturedElement).Frame = 1;
            (interfaceObjects[10] as TexturedElement).Frame = 1;
            (interfaceObjects[14] as TexturedElement).Frame = 1;
            (interfaceObjects[18] as TexturedElement).Frame = 1;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            interfaceObjects.ForEach(interfaceObject => interfaceObject.LoadContent(contentManager));
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < interfaceObjects.Count; ++i)
            {
                if (interfaceObjects[i] is TexturedElement)
                {
                    var current = interfaceObjects[i] as ITappable;
                    
                    TouchManager.OnTap(current, () =>
                    {
                        if (i % 2 != 0)
                        {
                            if (main.AttributePoints > 0)
                            {
                                main.AttributePoints -= 1;
                                if (i == 3) main.AttributeChanges.Strength += 1;
                                else if (i == 7) main.AttributeChanges.Intuition += 1;
                                else if (i == 11) main.AttributeChanges.Reflexes += 1;
                                else if (i == 15) main.AttributeChanges.Vitality += 1;
                                else if (i == 19) main.AttributeChanges.Vigor += 1;
                            }
                            else PhoneEffectsManager.Play(PhoneEffect.Vibration);
                        }
                        else
                        {
                            if (i == 2 && main.AttributeChanges.Strength > 0)
                            {
                                main.AttributePoints += 1;
                                main.AttributeChanges.Strength -= 1;
                            }
                            else if (i == 6 && main.AttributeChanges.Intuition > 0)
                            {
                                main.AttributePoints += 1;
                                main.AttributeChanges.Intuition -= 1;
                            }
                            else if (i == 10 && main.AttributeChanges.Reflexes > 0)
                            {
                                main.AttributePoints += 1;
                                main.AttributeChanges.Reflexes -= 1;
                            }
                            else if (i == 14 && main.AttributeChanges.Vitality > 0)
                            {
                                main.AttributePoints += 1;
                                main.AttributeChanges.Vitality -= 1;
                            }
                            else if (i == 18 && main.AttributeChanges.Vigor > 0)
                            {
                                main.AttributePoints += 1;
                                main.AttributeChanges.Vigor -= 1;
                            }
                            else PhoneEffectsManager.Play(PhoneEffect.Vibration);
                        }
                    });
                }
            }

            var strength = interfaceObjects[1] as Text;
            var intuition = interfaceObjects[5] as Text;
            var reflexes = interfaceObjects[9] as Text;
            var vitality = interfaceObjects[13] as Text;
            var vigor = interfaceObjects[17] as Text;

            strength.String = main.Preview.Strength.ToString();
            intuition.String = main.Preview.Intuition.ToString();
            reflexes.String = main.Preview.Reflexes.ToString();
            vitality.String = main.Preview.Vitality.ToString();
            vigor.String = main.Preview.Vigor.ToString();

            if (main.AttributeChanges.Strength != 0)
            {
                strength.Color = changeColor;
            }
            else strength.Color = defaultColor;

            if (main.AttributeChanges.Intuition != 0)
            {
                intuition.Color = changeColor;
            }
            else intuition.Color = defaultColor;

            if (main.AttributeChanges.Reflexes != 0)
            {
                reflexes.Color = changeColor;
            }
            else reflexes.Color = defaultColor;

            if (main.AttributeChanges.Vitality != 0)
            {
                vitality.Color = changeColor;
            }
            else vitality.Color = defaultColor;

            if (main.AttributeChanges.Vigor != 0)
            {
                vigor.Color = changeColor;
            }
            else vigor.Color = defaultColor;
        }

        public override void Draw(Renderer renderer)
        {
            interfaceObjects.ForEach(interfaceObject => interfaceObject.Draw(renderer));
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            interfaceObjects.ForEach(interfaceObject => interfaceObject.UnloadContent(contentManager));
            base.UnloadContent(contentManager);
        }
    }
}
