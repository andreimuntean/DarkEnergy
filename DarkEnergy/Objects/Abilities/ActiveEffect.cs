using DarkEnergy.Characters;

namespace DarkEnergy.Abilities
{
    public class ActiveEffect
    {
        public EffectType Type { get; set; }
        public float Value { get; set; }
        public int Rounds { get; set; }

        public ActiveEffect(EffectType type, float value, int rounds)
        {
            Type = type;
            Value = value;
            Rounds = rounds;
        }

        #region Methods
        private void absorbDamagePercentage(EffectType type, ref float value)
        {
            switch (type)
            {
                case EffectType.PhysicalDamage: value *= (1 - Value); break;
                case EffectType.AirDamage: value *= (1 - Value); break;
                case EffectType.DarknessDamage: value *= (1 - Value); break;
                case EffectType.EarthDamage: value *= (1 - Value); break;
                case EffectType.FireDamage: value *= (1 - Value); break;
                case EffectType.IceDamage: value *= (1 - Value); break;
                case EffectType.LightDamage: value *= (1 - Value); break;
                case EffectType.ShockDamage: value *= (1 - Value); break;
                case EffectType.WaterDamage: value *= (1 - Value); break;
            }
        }

        private void absorbDamageRaw(EffectType type, ref float value)
        {
            bool meetsConditions = false;

            switch (type)
            {
                case EffectType.PhysicalDamage: meetsConditions = true; break;
                case EffectType.AirDamage: meetsConditions = true; break;
                case EffectType.DarknessDamage: meetsConditions = true; break;
                case EffectType.EarthDamage: meetsConditions = true; break;
                case EffectType.FireDamage: meetsConditions = true; break;
                case EffectType.IceDamage: meetsConditions = true; break;
                case EffectType.LightDamage: meetsConditions = true; break;
                case EffectType.ShockDamage: meetsConditions = true; break;
                case EffectType.WaterDamage: meetsConditions = true; break;
            }

            if (meetsConditions)
            {
                value -= Value;
                if (Value < 0) Value = -value;
                else Value = 0;
            }
        }

        private void absorbMagicPercentage(EffectType type, ref float value)
        {
            switch (type)
            {
                case EffectType.AirDamage: value *= (1 - Value); break;
                case EffectType.DarknessDamage: value *= (1 - Value); break;
                case EffectType.EarthDamage: value *= (1 - Value); break;
                case EffectType.FireDamage: value *= (1 - Value); break;
                case EffectType.IceDamage: value *= (1 - Value); break;
                case EffectType.LightDamage: value *= (1 - Value); break;
                case EffectType.ShockDamage: value *= (1 - Value); break;
                case EffectType.WaterDamage: value *= (1 - Value); break;
            }
        }

        private void absorbMagicRaw(EffectType type, ref float value)
        {
            bool meetsConditions = false;

            switch (type)
            {
                case EffectType.AirDamage: meetsConditions = true; break;
                case EffectType.DarknessDamage: meetsConditions = true; break;
                case EffectType.EarthDamage: meetsConditions = true; break;
                case EffectType.FireDamage: meetsConditions = true; break;
                case EffectType.IceDamage: meetsConditions = true; break;
                case EffectType.LightDamage: meetsConditions = true; break;
                case EffectType.ShockDamage: meetsConditions = true; break;
                case EffectType.WaterDamage: meetsConditions = true; break;
            }

            if (meetsConditions)
            {
                value -= Value;
                if (Value < 0) Value = -value;
                else Value = 0;
            }
        }

        private void absorbPhysicalDamagePercentage(EffectType type, ref float value)
        {
            switch (type)
            {
                case EffectType.PhysicalDamage: value *= (1 - Value); break;
            }
        }

        private void absorbPhysicalDamageRaw(EffectType type, ref float value)
        {
            bool meetsConditions = false;

            switch (type)
            {
                case EffectType.PhysicalDamage: meetsConditions = true; break;
            }

            if (meetsConditions)
            {
                value -= Value;
                if (Value < 0) Value = -value;
                else Value = 0;
            }
        }

        private void increaseDamagePercentage(EffectType type, ref float value)
        {
            switch (type)
            {
                case EffectType.PhysicalDamage: value += value * Value; break;
                case EffectType.AirDamage: value += value * Value; break;
                case EffectType.DarknessDamage: value += value * Value; break;
                case EffectType.EarthDamage: value += value * Value; break;
                case EffectType.FireDamage: value += value * Value; break;
                case EffectType.IceDamage: value += value * Value; break;
                case EffectType.LightDamage: value += value * Value; break;
                case EffectType.ShockDamage: value += value * Value; break;
                case EffectType.WaterDamage: value += value * Value; break;
            }
        }

        private void increaseMagicPercentage(EffectType type, ref float value)
        {
            switch (type)
            {
                case EffectType.Heal: value += value * Value; break;
                case EffectType.AirDamage: value += value * Value; break;
                case EffectType.DarknessDamage: value += value * Value; break;
                case EffectType.EarthDamage: value += value * Value; break;
                case EffectType.FireDamage: value += value * Value; break;
                case EffectType.IceDamage: value += value * Value; break;
                case EffectType.LightDamage: value += value * Value; break;
                case EffectType.ShockDamage: value += value * Value; break;
                case EffectType.WaterDamage: value += value * Value; break;
            }
        }

        private void increasePhysicalDamagePercentage(EffectType type, ref float value)
        {
            switch (type)
            {
                case EffectType.PhysicalDamage: value += value * Value; break;
            }
        }
        #endregion

        public void Evaluate(Character current, Character target, EffectType type, ref float value)
        {
            switch (Type)
            {
                case EffectType.AbsorbDamagePercentage: if (current != target) absorbDamagePercentage(type, ref value);
                    break;

                case EffectType.AbsorbDamageRaw: if (current != target) absorbDamageRaw(type, ref value);
                    break;

                case EffectType.AbsorbMagicPercentage: if (current != target) absorbMagicPercentage(type, ref value);
                    break;

                case EffectType.AbsorbMagicRaw: if (current != target) absorbMagicRaw(type, ref value);
                    break;

                case EffectType.AbsorbPhysicalDamagePercentage: if (current != target) absorbPhysicalDamagePercentage(type, ref value);
                    break;

                case EffectType.AbsorbPhysicalDamageRaw: if (current != target) absorbPhysicalDamageRaw(type, ref value);
                    break;

                case EffectType.IncreaseDamagePercentage: if (current == target) increaseDamagePercentage(type, ref value);
                    break;

                case EffectType.IncreaseMagicPercentage: if (current == target) increaseMagicPercentage(type, ref value);
                    break;

                case EffectType.IncreasePhysicalDamagePercentage: if (current == target) increasePhysicalDamagePercentage(type, ref value);
                    break;
            }
        }

        public void Tick()
        {
            Rounds -= 1;
        }
    }
}