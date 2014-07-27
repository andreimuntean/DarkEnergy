namespace DarkEnergy.Trading
{
    public enum Currency
    {
        Coins = 0,
        DarkCrystals = 1
    }

    public struct Price
    {
        public readonly int Value;
        public readonly Currency Currency;

        public Price(int value, Currency currency) : this()
        {
            Value = value;
            Currency = currency;
        }
    }
}
