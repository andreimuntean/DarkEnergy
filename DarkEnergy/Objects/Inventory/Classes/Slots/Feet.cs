using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using SharpDX;

namespace DarkEnergy.Inventory.Slots
{
    public class Feet : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Feet; } }

        public Feet(string name, int value, Attributes attributes) : base(name, value, attributes, 250, 180) { }

        public void SetState(Hero hero)
        {
            var origin = hero.PositionRectangle.TopLeft;
            Position = origin + new Vector2(-11.2f, 192.7f) * hero.Scale;
        }
    }
}
