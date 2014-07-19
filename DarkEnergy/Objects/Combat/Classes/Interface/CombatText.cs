using System;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat.Interface
{
    public enum CombatTextStyle { Message, Value }

    public class CombatText : GameSystem
    {
        private CombatTextStyle combatTextStyle;
        private TexturedElement valueBackground;
        private TexturedElement messageBackground;
        private Text attackType;
        private Text message;
        private Text number;
        private double elapsedTime;
        private const double displayTime = 1.5;

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                AdjustElements(this, EventArgs.Empty);
            }
        }

        public CombatText(CharacterCombatData data)
        {
            elapsedTime = 0;
            int value = (int)Math.Abs(data.TotalDamageReceived - data.TotalHealingReceived);
            valueBackground = new TexturedElement(128, 85) { Parent = this, Path = @"Interface\World\Combat\CombatTextBackground.dds" };
            messageBackground = new TexturedElement(128, 85) { Parent = this, Path = @"Interface\World\Combat\CombatTextBackground.dds" };
            attackType = new Text(FontStyle.SegoeWP28) { Parent = this };
            message = new Text(FontStyle.SegoeWP44) { Parent = this };
            number = new Text(FontStyle.SegoeWP64) { Parent = this };

            if (data.AttackStatus == AttackStatus.Missed)
            {
                combatTextStyle = CombatTextStyle.Message;
                message.String = Resources.Strings.CombatMessageMissed;
            }
            else if (data.AttackStatus == AttackStatus.Evaded)
            {
                combatTextStyle = CombatTextStyle.Message;
                message.String = Resources.Strings.CombatMessageEvaded;
            }
            else if (data.AttackStatus == AttackStatus.Resisted)
            {
                combatTextStyle = CombatTextStyle.Message;
                message.String = Resources.Strings.CombatMessageResisted;
            }
            else
            {
                combatTextStyle = CombatTextStyle.Value;

                if (data.TotalHealingReceived > data.TotalDamageReceived)
                {
                    attackType.String = Resources.Strings.CombatMessageHealed;
                    attackType.Color = new Color(0, 255, 0);
                }
                else
                {
                    switch (data.AbilityElement)
                    {
                        case Element.None:
                            attackType.String = Resources.Strings.CombatMessagePhysical;
                            attackType.Color = new Color(255, 0, 0);
                            break;

                        case Element.Light:
                            attackType.String = Resources.Strings.CombatMessageLight;
                            attackType.Color = new Color(255, 255, 200);
                            break;

                        case Element.Air:
                            attackType.String = Resources.Strings.CombatMessageAir;
                            attackType.Color = new Color(255, 255, 255);
                            break;

                        case Element.Ice:
                            attackType.String = Resources.Strings.CombatMessageIce;
                            attackType.Color = new Color(100, 255, 255);
                            break;

                        case Element.Water:
                            attackType.String = Resources.Strings.CombatMessageWater;
                            attackType.Color = new Color(0, 0, 255);
                            break;

                        case Element.Darkness:
                            attackType.String = Resources.Strings.CombatMessageDarkness;
                            attackType.Color = new Color(100, 0, 255);
                            break;

                        case Element.Earth:
                            attackType.String = Resources.Strings.CombatMessageEarth;
                            attackType.Color = new Color(220, 255, 0);
                            break;

                        case Element.Fire:
                            attackType.String = Resources.Strings.CombatMessageFire;
                            attackType.Color = new Color(255, 102, 0);
                            break;

                        case Element.Shock:
                            attackType.String = Resources.Strings.CombatMessageShock;
                            attackType.Color = new Color(97, 193, 235);
                            break;
                    }
                }

                number.String = value.ToString();
            }

            Position = new Vector2(data.Character.X + data.Character.Width / 2, data.Character.Y);
        }

        public override void Initialize()
        {
            base.Initialize();
            valueBackground.Initialize();
            messageBackground.Initialize();
            attackType.Initialize();
            message.Initialize();
            number.Initialize();

            if (combatTextStyle == CombatTextStyle.Message)
            {
                attackType.Visible = false;
                number.Visible = false;
            }
            else if (combatTextStyle == CombatTextStyle.Value)
            {
                message.Visible = false;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            valueBackground.LoadContent(contentManager);
            messageBackground.LoadContent(contentManager);
            attackType.LoadContent(contentManager);
            message.LoadContent(contentManager);
            number.LoadContent(contentManager);
            base.LoadContent(contentManager);

            if (combatTextStyle == CombatTextStyle.Message)
            {
                attackType.Visible = false;
                number.Visible = false;
                valueBackground.Visible = false;
                messageBackground.Scale = new Vector2(message.Width / messageBackground.Width, 0.85f);
            }
            else if (combatTextStyle == CombatTextStyle.Value)
            {
                message.Visible = false;
                valueBackground.Visible = true;
                valueBackground.Scale = new Vector2(number.Width / valueBackground.Width, 1);
                messageBackground.Scale = new Vector2(attackType.Width / messageBackground.Width, 0.4f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            var modifier = gameTime.ElapsedGameTime.TotalSeconds;
            
            if (elapsedTime + modifier >= displayTime)
            {
                Visible = false;
            }
            else 
            {
                elapsedTime += modifier;
                Opacity = (float)(displayTime - elapsedTime);
                Position += new Vector2(0, (float)(-150 * modifier));
            }
        }

        public override void Draw(Renderer renderer)
        {
            valueBackground.Draw(renderer);
            messageBackground.Draw(renderer);
            attackType.Draw(renderer);
            message.Draw(renderer);
            number.Draw(renderer);
        }

        protected void AdjustElements(object sender, EventArgs e)
        {
            attackType.Position = Position + new Vector2(-attackType.Width / 2, 0);
            message.Position = Position + new Vector2(-message.Width / 2, 20);
            number.Position = Position + new Vector2(-number.Width / 2, 25);

            if (number.Visible)
            {
                valueBackground.Position = new Vector2(Position.X - valueBackground.Width / 2, number.Position.Y + 20);
                messageBackground.Position = new Vector2(Position.X - messageBackground.Width / 2, attackType.Position.Y + 9);
            }
            else
            {
                messageBackground.Position = new Vector2(Position.X - messageBackground.Width / 2, message.Position.Y + 9);
            }
        }
    }
}
