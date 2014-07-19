namespace DarkEnergy.Inventory
{
    public interface IItem
    {
        string Name { get; set; }
        int Count { get; set; }
        int StackLimit { get; }
        int Value { get; set; }
        bool IsStackable { get; }

        string GetDescription();
    }
}
