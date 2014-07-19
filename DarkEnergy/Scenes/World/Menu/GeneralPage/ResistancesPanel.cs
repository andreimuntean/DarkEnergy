using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;

namespace DarkEnergy.Scenes.World.Menu.Character.General
{
    public class ElementsPanel : GameSystem
    {
        enum ElementType { Offensive, Defensive }

        private ElementType type;
        private List<Text> elements;
        private TexturedElement currentElement;
        private Text selection;
        private TexturedElement leftButton, rightButton;

        public ElementsPanel()
        {
            elements = new List<Text>()
            {
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 434, 177) { Parent = this },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 544, 213) { Parent = this },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 583, 341) { Parent = this },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 544, 469) { Parent = this },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 434, 505) { Parent = this },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 323, 469) { Parent = this },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 288, 341) { Parent = this },
                new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 323, 213) { Parent = this }
            };

            currentElement = new TexturedElement(54, 54, 1047.4f, 309.3f) { Parent = this, Path = @"Interface\Character\Elements.dds" };
            selection = new Text(FontStyle.Calibri24, HorizontalAlignment.Center, 435.5f, 580) { Parent = this };
            leftButton = new TexturedElement(120, 120, 896.5f, 565) { Parent = this, Path = @"Interface\ArrowButtons.dds" };
            rightButton = new TexturedElement(120, 120, 1185, 565) { Parent = this, Path = @"Interface\ArrowButtons.dds" };
        }

        protected void AssignData()
        {
            selection.String = type == ElementType.Defensive ? Resources.Strings.ElementDefensive : Resources.Strings.ElementOffensive;

            for (int i = 0; i < elements.Count; ++i)
            {
                var text = elements[i];
                var element = (Element)(i + 1);

                Color positive = new Color(255, 187, 187);
                Color neutral = new Color(255, 255, 255);
                Color negative = new Color(204, 187, 255);

                float x = Resistances.GetModifier(type == ElementType.Defensive ? GameManager.Hero.DefensiveElement : GameManager.Hero.OffensiveElement, element) * 100 - 100;

                if (x > 0) text.Color = positive;
                else if (x < 0) text.Color = negative;
                else text.Color = neutral;

                text.String = Math.Abs(x).ToString();
            }

            currentElement.Frame = (int)(type == ElementType.Defensive ? GameManager.Hero.DefensiveElement : GameManager.Hero.OffensiveElement);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            elements.ForEach(element => element.Initialize());
            currentElement.Initialize();
            selection.Initialize();
            leftButton.Initialize();
            rightButton.Initialize();

            leftButton.Scale = new Vector2(0.58333f, 0.58333f);
            leftButton.Frame = 0;
            rightButton.Scale = new Vector2(0.58333f, 0.58333f);
            rightButton.Frame = 1;
            type = ElementType.Offensive;
            AssignData();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            elements.ForEach(element => element.LoadContent(contentManager));
            currentElement.LoadContent(contentManager);
            selection.LoadContent(contentManager);
            leftButton.LoadContent(contentManager);
            rightButton.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            TouchManager.OnTap(leftButton, () =>
            {
                type = (ElementType)Math.Abs(1 - (int)type);
                AssignData();
            });

            TouchManager.OnTap(rightButton, () =>
            {
                type = (ElementType)Math.Abs(1 - (int)type);
                AssignData();
            });
        }

        public override void Draw(Renderer renderer)
        {
            elements.ForEach(element => element.Draw(renderer));
            currentElement.Draw(renderer);
            selection.Draw(renderer);
            leftButton.Draw(renderer);
            rightButton.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            elements.ForEach(element => element.UnloadContent(contentManager));
            currentElement.UnloadContent(contentManager);
            selection.UnloadContent(contentManager);
            leftButton.UnloadContent(contentManager);
            rightButton.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }
    }
}
