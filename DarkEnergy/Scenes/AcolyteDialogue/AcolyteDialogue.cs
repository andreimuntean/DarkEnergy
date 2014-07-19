using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy.Scenes
{
    public class AcolyteDialogue : CinematicDialogue, IScene
    {
        public AcolyteDialogue() : base()
        {
            Background.Path = @"World\DialogueAcolyteBackground.dds";
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            Header = Resources.Dialogue.Acolyte1_Header;
            Body = Resources.Dialogue.Acolyte1_Body1.Replace("{N}", GameManager.Hero.Name);
            Footer = Resources.Dialogue.Acolyte1_Footer1;
        }

        protected override void OnTap()
        {
            base.OnTap();
            if (Index == 1)
            {
                Body = Resources.Dialogue.Acolyte1_Body2;
                Footer = Resources.Dialogue.Acolyte1_Footer2;
            }
            else SceneManager.Play(new World.Westhill.Village());
        }

        public void OnBackKeyPress()
        {
            OnTap();
        }
    }
}
