using DarkEnergy.Characters;

namespace DarkEnergy.Inventory
{
    public enum EquipmentSlot
    {
        Weapon,
        Relic,
        Head,
        Neck,
        Chest,
        Back,
        Hands,
        Finger,
        Legs,
        Feet
    }

    public abstract class EquippableItem : TexturedElement, IItem
    {
        public static string GetName(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Weapon: return Resources.Strings.ItemWeapon;
                case EquipmentSlot.Relic: return Resources.Strings.ItemRelic;
                case EquipmentSlot.Head: return Resources.Strings.ItemHead;
                case EquipmentSlot.Neck: return Resources.Strings.ItemNeck;
                case EquipmentSlot.Chest: return Resources.Strings.ItemChest;
                case EquipmentSlot.Back: return Resources.Strings.ItemBack;
                case EquipmentSlot.Hands: return Resources.Strings.ItemHands;
                case EquipmentSlot.Finger: return Resources.Strings.ItemFinger;
                case EquipmentSlot.Legs: return Resources.Strings.ItemLegs;
                case EquipmentSlot.Feet: return Resources.Strings.ItemFeet;
                default: return "";
            }
        }

        protected Quantity stack;
        public abstract EquipmentSlot Slot { get; }
        public Attributes Attributes { get; protected set; }
        public string Name { get; set; }
        public int Count { get { return stack.Current; } set { stack = new Quantity(value, StackLimit); } }
        public int StackLimit { get { return stack.Maximum; } }
        public int Value { get; set; }
        public bool IsStackable { get { return stack.Maximum > 1; } }

        public EquippableItem(string name, int value, Attributes attributes, int width, int height) : base(width, height)
        {
            this.Name = name;
            this.Value = value;
            this.Attributes = attributes;
            this.stack = Quantity.One;
        }

        public EquippableItem(string name, int value, Attributes attributes, int width, int height, int stackLimit) : base(width, height)
        {
            this.Name = name;
            this.Value = value;
            this.Attributes = attributes;
            this.stack = new Quantity(1, stackLimit);
        }

        public string GetDescription()
        {
            return GetDescription(null);
        }

        public string GetDescription(EquippableItem comparison)
        {
            var result = (comparison == null) ? Attributes.ToString() : Attributes.ToString(comparison.Attributes);
            result += Resources.Strings.Item_Value + ": " + Value;

            return result;
        }
    }
}