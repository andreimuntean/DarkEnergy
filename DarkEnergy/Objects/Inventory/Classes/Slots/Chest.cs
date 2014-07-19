using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using SharpDX;

namespace DarkEnergy.Inventory.Slots
{
    public class Chest : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Chest; } }

        public Chest(string name, int value, Attributes attributes) : base(name, value, attributes, 250, 180) { }

        public void SetState(Hero hero)
        {
            var origin = hero.PositionRectangle.TopLeft;
            Position = origin + new Vector2(-11.2f, -11.6f) * hero.Scale;
        }
    }
}
