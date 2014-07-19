using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using SharpDX;

namespace DarkEnergy.Inventory.Slots
{
    public class Head : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Head; } }
        public bool HairVisible { get; set; }

        public Head(string name, int value, Attributes attributes) : base(name, value, attributes, 128, 128) { }

        public void SetState(Hero hero)
        {
            var origin = hero.PositionRectangle.TopLeft;
            Position = origin + new Vector2(59.8f, -40.9f) * hero.Scale;
        }
    }
}
