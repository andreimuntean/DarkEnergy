namespace DarkEnergy.Inventory
{
    public class GenericItem : IItem
    {
        protected Quantity stack;
        public string Name { get; set; }
        public int Count { get { return stack.Current; } set { stack += value; } }
        public int StackLimit { get { return stack.Maximum; } }
        public int Value { get; set; }
        public bool IsStackable { get { return stack.Maximum > 1; } }

        public GenericItem() { }

        public GenericItem(string name, int value)
        {
            this.Name = name;
            this.Value = value;
            this.stack = Quantity.One;
        }

        public GenericItem(string name, int value, int stackLimit)
        {
            this.Name = name;
            this.Value = value;
            this.stack = new Quantity(1, stackLimit);
        }

        public string GetDescription()
        {
            return Resources.Strings.Item_Value + ": " + Value;
        }
    }
}