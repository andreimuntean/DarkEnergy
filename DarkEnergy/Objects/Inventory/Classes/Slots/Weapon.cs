using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using SharpDX;

namespace DarkEnergy.Inventory.Slots
{
    public class Weapon : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Weapon; } }
        public Element Element { get; set; }

        public Weapon(string name, int value, Attributes attributes) : base(name, value, attributes, 350, 512) { }

        public void SetState(Hero hero)
        {
            Rotation = 135.0f / 360.0f;
            IsScaleIndependent = true;
            Scale = hero.Scale / hero.GenderBodyScale;

            var origin = hero.PositionRectangle.TopLeft;

            switch ((int)(hero.Scale.X / hero.GenderBodyScale.X * 100))
            {
                case 100:
                    Position = (hero.Features.Gender == Gender.Female) ? new Vector2(origin.X + 365, origin.Y + 270) : new Vector2(origin.X + 370, origin.Y + 267);
                    break;
                case 80:
                    Position = (hero.Features.Gender == Gender.Female) ? new Vector2(origin.X + 287, origin.Y + 213) : new Vector2(origin.X + 297, origin.Y + 215);
                    break;
                case 65:
                    Position = (hero.Features.Gender == Gender.Female) ? new Vector2(origin.X + 235, origin.Y + 174) : new Vector2(origin.X + 240, origin.Y + 170);
                    break;
                default:
                    Position = new Vector2(origin.X + System.Math.Abs(Scale.Y - Scale.X) * 14 + 152 * Scale.X + 228 * Scale.Y, origin.Y + 30 - (1 - Scale.X) * 5 - 130f * Scale.X + 376.3f * Scale.Y);
                    break;
            }
        }
    }
}
