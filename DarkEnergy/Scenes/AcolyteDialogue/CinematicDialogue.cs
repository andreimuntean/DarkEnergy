using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes
{
    public abstract class CinematicDialogue : GameSystem
    {
        protected Text header, body, footer;

        public TexturedElement Background { get; set; }
        public TexturedElement Panel { get; set; }

        public string Header { get { return header.String; } set { header.String = value; } }
        public string Body { get { return body.String; } set { body.String = value; } }
        public string Footer { get { return footer.String; } set { footer.String = value; } }

        public int Index { get; protected set; }

        public CinematicDialogue()
        {
            Background = new TexturedElement(1280, 720) { Parent = this };
            Panel = new TexturedElement(570, 660, 32, 32) { Parent = this, Path = @"Interface\DialoguePanel.dds" };
            header = new Text(FontStyle.CenturyGothic40, 48, 48) { Parent = this };
            body = new Text(FontStyle.Calibri24, 58, 138) { Parent = this };
            footer = new Text(FontStyle.Calibri24, HorizontalAlignment.None, 48, VerticalAlignment.Bottom, -48) { Color = new Color(0.9f, 0.8f, 1.0f), Parent = this };
        }

        public override void Initialize()
        {
            base.Initialize();
            Background.Initialize();
            Panel.Initialize();
            header.Initialize();
            body.Initialize();
            footer.Initialize();

            body.MaxWidth = Panel.Width - 42;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Background.LoadContent(contentManager);
            Panel.LoadContent(contentManager);
            header.LoadContent(contentManager);
            body.LoadContent(contentManager);
            footer.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            TouchManager.OnTap(Background, () =>
            {
                OnTap();
            });
        }

        public override void Draw(Renderer renderer)
        {
            Background.Draw(renderer);
            Panel.Draw(renderer);
            header.Draw(renderer);
            body.Draw(renderer);
            footer.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            Background.UnloadContent(contentManager);
            Panel.UnloadContent(contentManager);
            header.UnloadContent(contentManager);
            body.UnloadContent(contentManager);
            footer.UnloadContent(contentManager);
        }

        protected virtual void OnTap()
        {
            ++Index;
        }
    }
}
