using DarkEnergy.Characters;

namespace DarkEnergy.Inventory.Slots
{
    public class Neck : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Neck; } }
        new public bool Visible { get { return false; } }

        public Neck(string name, int value, Attributes attributes) : base(name, value, attributes, 0, 0) { }
    }
}
