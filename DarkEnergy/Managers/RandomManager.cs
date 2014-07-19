using System;
using System.Collections.Generic;
namespace DarkEnergy
{
    static class RandomManager
    {
        private static Random random;

        public static double GetDouble()
        {
            if (random == null) random = new Random();
            return random.NextDouble();
        }

        public static double GetDouble(double higherExclusive)
        {
            if (random == null) random = new Random();
            return random.NextDouble() * higherExclusive;
        }

        public static double GetDouble(double lowerInclusive, double higherExclusive)
        {
            if (random == null) random = new Random();
            return lowerInclusive + random.NextDouble() * (higherExclusive - lowerInclusive);
        }

        public static bool GetBool()
        {
            if (random == null) random = new Random();
            return random.Next() % 2 == 0;
        }

        public static int GetInt(int higherExclusive)
        {
            if (random == null) random = new Random();
            return random.Next(0, higherExclusive);
        }

        public static int GetInt(int lowerInclusive, int higherExclusive)
        {
            if (random == null) random = new Random();
            return random.Next(lowerInclusive, higherExclusive);
        }

        #region Extensions
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                var k = GetInt(n--);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        #endregion
    }
}
