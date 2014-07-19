namespace DarkEnergy
{
    public struct Quantity
    {
        public static Quantity Zero = new Quantity(0, 0);
        public static Quantity One = new Quantity(1, 1);

        public int Current { get; private set; }
        public int Maximum { get; private set; }

        public Quantity(int maximum) : this()
        {
            Current = maximum;
            Maximum = maximum;
        }

        public Quantity(int current, int maximum) : this()
        {
            Current = (current > maximum) ? maximum : (current < 0) ? 0 : current;
            Maximum = maximum;
        }

        public static Quantity operator +(Quantity x, int y)
        {
            return new Quantity(x.Current + y, x.Maximum);
        }

        public static Quantity operator +(int x, Quantity y)
        {
            return new Quantity(y.Current + x, y.Maximum);
        }

        public static Quantity operator -(Quantity x, int y)
        {
            return new Quantity(x.Current - y, x.Maximum);
        }

        public static Quantity operator -(int x, Quantity y)
        {
            return new Quantity(y.Current - x, y.Maximum);
        }

        public static bool operator <(Quantity x, int y)
        {
            return x.Current < y;
        }

        public static bool operator >(Quantity x, int y)
        {
            return x.Current > y;
        }

        public static bool operator <(int x, Quantity y)
        {
            return x < y.Current;
        }

        public static bool operator >(int x, Quantity y)
        {
            return x > y.Current;
        }

        public static bool operator <=(Quantity x, int y)
        {
            return x.Current <= y;
        }

        public static bool operator >=(Quantity x, int y)
        {
            return x.Current >= y;
        }

        public static bool operator <=(int x, Quantity y)
        {
            return x <= y.Current;
        }

        public static bool operator >=(int x, Quantity y)
        {
            return x >= y.Current;
        }

        public override string ToString()
        {
            return Current.ToString() + " / " + Maximum.ToString();
        }
    }
}
