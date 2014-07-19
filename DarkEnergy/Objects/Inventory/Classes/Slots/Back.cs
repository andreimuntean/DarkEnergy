using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using SharpDX;

namespace DarkEnergy.Inventory.Slots
{
    public class Back : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Back; } }

        public Back(string name, int value, Attributes attributes) : base(name, value, attributes, 500, 500) { }

        public void SetState(Hero hero)
        {
            var origin = hero.PositionRectangle.TopLeft;
            Position = origin + new Vector2(-137.2f, -92.9f) * hero.Scale;
        }
    }
}
