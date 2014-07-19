using DarkEnergy.Characters;

namespace DarkEnergy.Inventory.Slots
{
    public class Finger : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Finger; } }
        new public bool Visible { get { return false; } }

        public Finger(string name, int value, Attributes attributes) : base(name, value, attributes, 0, 0) { }
    }
}
