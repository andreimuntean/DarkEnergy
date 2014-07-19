using System;
using System.Collections.Generic;

namespace DarkEnergy.Characters
{
    /// <summary>
    /// A character can master one out of eight elements.
    /// </summary>
    public enum Element
    {
        None = 0,
        Light = 1, Air = 2,
        Ice = 3, Water = 4,
        Darkness = 5, Earth = 6,
        Fire = 7, Shock = 8
    }

    static class Resistances
    {
        public static float GetModifier(Element source, Element target)
        {
            if (source == Element.None || target == Element.None)
                return 1.0f;

            #region Magic. SPOILER
            // It figures out the modifier based on the cyclic structure of
            // the Element enum. The absolute operation finds the distance on
            // the Element circle between the source and the target. The
            // (x > 4 ? 8 - x : x) conditional then mirrors the value of x
            // around the value of 4; x becoming a value between 0 and 4.
            // The remaining operations fine-tune the result.
            #endregion

            float x = Math.Abs(source - target);
            return (x == 0) ? 0.1f : ((x > 4 ? 8 - x : x) / 2);
        }

        public static string GetName(Element element)
        {
            switch (element)
            {
                case Element.Light: return Resources.Strings.ResistanceLight;
                case Element.Air: return Resources.Strings.ResistanceAir;
                case Element.Ice: return Resources.Strings.ResistanceIce;
                case Element.Water: return Resources.Strings.ResistanceWater;
                case Element.Darkness: return Resources.Strings.ResistanceDarkness;
                case Element.Earth: return Resources.Strings.ResistanceEarth;
                case Element.Fire: return Resources.Strings.ResistanceFire;
                case Element.Shock: return Resources.Strings.ResistanceShock;
                default: return Resources.Strings.ResistanceNone;
            }
        }
    }
}
