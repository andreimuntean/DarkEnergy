using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using SharpDX;

namespace DarkEnergy.Inventory.Slots
{
    public class Legs : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Legs; } }

        public Legs(string name, int value, Attributes attributes) : base(name, value, attributes, 250, 180) { }

        public void SetState(Hero hero)
        {
            var origin = hero.PositionRectangle.TopLeft;
            Position = origin + new Vector2(-11.2f, 128.95f) * hero.Scale;
        }
    }
}
