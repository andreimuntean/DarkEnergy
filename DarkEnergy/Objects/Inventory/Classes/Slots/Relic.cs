using DarkEnergy.Characters;

namespace DarkEnergy.Inventory.Slots
{
    public class Relic : EquippableItem
    {
        public override EquipmentSlot Slot { get { return EquipmentSlot.Relic; } }
        public Element Element { get; set; }
        new public bool Visible { get { return false; } }

        public Relic(string name, int value, Attributes attributes) : base(name, value, attributes, 0, 0) { }
    }
}
