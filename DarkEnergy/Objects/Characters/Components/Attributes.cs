using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkEnergy.Characters
{
    public enum AttributeType
    {
        Strength, Intuition, Reflexes, Vitality, Vigor
    }

    public class Attributes
    {
        public static Attributes Zero { get { return new Attributes(); } }

        /// <summary>
        /// Increases damage directly.
        /// </summary>
        public float WeaponDamage { get; set; }

        /// <summary>
        /// Reduces all damage received.
        /// </summary>
        public float Armor { get; set; }

        /// <summary>
        /// Increases defense and physical damage.
        /// </summary>
        public float Strength { get; set; }

        /// <summary>
        /// Increases magical power.
        /// </summary>
        public float Intuition { get; set; }

        /// <summary>
        /// Increases evasion.
        /// </summary>
        public float Reflexes { get; set; }

        /// <summary>
        /// Increases health.
        /// </summary>
        public float Vitality { get; set; }

        /// <summary>
        /// Increases Dark Energy generation.
        /// </summary>
        public float Vigor { get; set; }

        public Attributes()
        {
            WeaponDamage = 0;
            Armor = 0;
            Strength = 0;
            Intuition = 0;
            Reflexes = 0;
            Vitality = 0;
            Vigor = 0;
        }

        public Attributes(float strength, float intuition, float reflexes, float vitality, float vigor)
        {
            this.WeaponDamage = 0;
            this.Armor = 0;
            this.Strength = strength;
            this.Intuition = intuition;
            this.Reflexes = reflexes;
            this.Vitality = vitality;
            this.Vigor = vigor;
        }

        public Attributes(float weaponDamage, float armor, float strength, float intuition, float reflexes, float vitality, float vigor)
        {
            this.WeaponDamage = weaponDamage;
            this.Armor = armor;
            this.Strength = strength;
            this.Intuition = intuition;
            this.Reflexes = reflexes;
            this.Vitality = vitality;
            this.Vigor = vigor;
        }

        public float CalculateMagicalPower()
        {
            return Intuition * 2;
        }

        public float CalculatePhysicalDamage()
        {
            return Strength * 2 + WeaponDamage;
        }

        public int CalculateDarkEnergy()
        {
            return 100;
        }

        public int CalculateDarkEnergyGenerated()
        {
            if (Vigor < 0)
            {
                ExceptionManager.Log("The Vigor attribute of a unit is below zero: " + Vigor);
                return 0;
            }
            return (int)Math.Round(100 - 100 / (Vigor / 87 + 1));
        }

        public float CalculateEvasion(float precision)
        {
            if (precision < 0)
            {
                ExceptionManager.Log("The Precision parameter is below zero: " + precision);
                return 0;
            }
            if (Reflexes < 0)
            {
                ExceptionManager.Log("The Reflexes attribute of a unit is below zero: " + Reflexes);
                return 0;
            }
            return 1 - 1 / (Reflexes / (100 + precision) + 1);
        }

        public int CalculateMaximumHealth()
        {
            return (int)(Vitality * 5);
        }

        public float CalculateDefense()
        {
            if (Armor < 0)
            {
                ExceptionManager.Log("The Armor attribute of a unit is below zero: " + Armor);
                return 0;
            }
            if (Strength < 0)
            {
                ExceptionManager.Log("The Strength attribute of a unit is below zero: " + Strength);
                return 0;
            }
            return 1 - 0.85f / (Armor / 100 + 1) - 0.15f / (Strength / 58 + 1);
        }

        public float CalculateSpeed()
        {
            return Reflexes;
        }

        public Dictionary<string, float> GetAttributes()
        {
            var attributes = new Dictionary<string, float>()
            {
                { Resources.Strings.Attribute_Strength, Strength },
                { Resources.Strings.Attribute_Intuition, Intuition },
                { Resources.Strings.Attribute_Reflexes, Reflexes },
                { Resources.Strings.Attribute_Vitality, Vitality },
                { Resources.Strings.Attribute_Vigor, Vigor }
            };

            return (from x in attributes orderby x.Value descending select x).ToDictionary(t => t.Key, t => t.Value);
        }

        public static Attributes operator +(Attributes x, Attributes y)
        {
            return new Attributes()
            {
                WeaponDamage = x.WeaponDamage + y.WeaponDamage,
                Armor = x.Armor + y.Armor,
                Strength = x.Strength + y.Strength,
                Intuition = x.Intuition + y.Intuition,
                Reflexes = x.Reflexes + y.Reflexes,
                Vitality = x.Vitality + y.Vitality,
                Vigor = x.Vigor + y.Vigor,
            };
        }

        public static Attributes operator -(Attributes x, Attributes y)
        {
            return new Attributes()
            {
                WeaponDamage = x.WeaponDamage - y.WeaponDamage,
                Armor = x.Armor - y.Armor,
                Strength = x.Strength - y.Strength,
                Intuition = x.Intuition - y.Intuition,
                Reflexes = x.Reflexes - y.Reflexes,
                Vitality = x.Vitality - y.Vitality,
                Vigor = x.Vigor - y.Vigor,
            };
        }

        public static Attributes operator *(Attributes x, float y)
        {
            return new Attributes()
            {
                WeaponDamage = (int)Math.Round(x.WeaponDamage * y),
                Armor = (int)Math.Round(x.Armor * y),
                Strength = (int)Math.Round(x.Strength * y),
                Intuition = (int)Math.Round(x.Intuition * y),
                Reflexes = (int)Math.Round(x.Reflexes * y),
                Vitality = (int)Math.Round(x.Vitality * y),
                Vigor = (int)Math.Round(x.Vigor * y),
            };
        }

        public static Attributes operator *(float x, Attributes y)
        {
            return y * x;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object o)
        {
            if (!(o is Attributes)) return false;
            return Equals(o as Attributes);
        }

        public bool Equals(Attributes x)
        {
            if (x == null) return false;
            if (WeaponDamage != x.WeaponDamage) return false;
            if (Armor != x.Armor) return false;
            if (Strength != x.Strength) return false;
            if (Intuition != x.Intuition) return false;
            if (Reflexes != x.Reflexes) return false;
            if (Vitality != x.Vitality) return false;
            if (Vigor != x.Vigor) return false;
            return true;
        }

        public static bool operator ==(Attributes x, Attributes y)
        {
            if ((object)y == null) return (object)x == null;
            if (x.WeaponDamage != y.WeaponDamage) return false;
            if (x.Armor != y.Armor) return false;
            if (x.Strength != y.Strength) return false;
            if (x.Intuition != y.Intuition) return false;
            if (x.Reflexes != y.Reflexes) return false;
            if (x.Vitality != y.Vitality) return false;
            if (x.Vigor != y.Vigor) return false;
            return true;
        }

        public static bool operator !=(Attributes x, Attributes y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            var attributes = GetAttributes();
            var result = "";

            if (WeaponDamage != 0) result += WeaponDamage + " " + Resources.Strings.Attribute_WeaponDamage + "\n";
            if (Armor != 0) result += Armor + " " + Resources.Strings.Attribute_Armor + "\n";

            foreach (var attribute in attributes)
            {
                if (attribute.Value == 0)
                    break;

                result += attribute.Value + " " + attribute.Key + "\n";
            }

            return result;
        }

        public string ToString(Attributes comparison)
        {
            if (comparison == null) return ToString();

            var attributes = GetAttributes();
            var comparisonAttributes = comparison.GetAttributes();
            var result = "";

            Action<float, float, string> writeLine = (a, b, name) =>
            {
                result += (a > 0 ? a + " (" : "(");

                var difference = a - b;
                if (difference > 0) result += "+";

                result += difference + ") " + name + "\n";
            };

            if (WeaponDamage != 0) writeLine(WeaponDamage, comparison.WeaponDamage, Resources.Strings.Attribute_WeaponDamage);
            if (Armor != 0) writeLine(Armor, comparison.Armor, Resources.Strings.Attribute_Armor);

            foreach (var key in attributes.Keys)
            {
                if (attributes[key] > 0)
                {
                    writeLine(attributes[key], comparisonAttributes[key], key);
                }
                else if (comparisonAttributes[key] > 0)
                {
                    writeLine(0, comparisonAttributes[key], key);
                }
            }

            return result;
        }
    }
}
