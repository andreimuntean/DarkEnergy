using System;
using DarkEnergy.Characters;

namespace DarkEnergy.Abilities
{
    public enum EffectType
    {
        AbsorbDamagePercentage,
        AbsorbDamageRaw,
        AbsorbMagicPercentage,
        AbsorbMagicRaw,
        AbsorbPhysicalDamagePercentage,
        AbsorbPhysicalDamageRaw,
        AirDamage,
        DarknessDamage,
        DrainLife,
        EarthDamage,
        FireDamage,
        Heal,
        IceDamage,
        IncreaseDamagePercentage,
        IncreaseMagicPercentage,
        IncreasePhysicalDamagePercentage,
        LightDamage,
        PhysicalDamage,
        Revive,
        ShockDamage,
        WaterDamage
    }
    public class Effect
    {
        public EffectType Type { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public int RoundsActive { get; set; }
        public bool AreaEffect { get; set; }

        public Effect(EffectType type, int minimumValue, int maximumValue, int roundsActive = 0, bool areaEffect = false)
        {
            Type = type;
            MinimumValue = (minimumValue < maximumValue ? minimumValue : maximumValue);
            MaximumValue = (minimumValue < maximumValue ? maximumValue : minimumValue);
            RoundsActive = roundsActive;
            AreaEffect = areaEffect;
        }

        #region Methods
        public void AbsorbPercentage(Character current, Character target, float value)
        {
            value = value / 1000;
            target.AddEffect(new ActiveEffect(Type, value, RoundsActive));
        }

        public void AbsorbRaw(Character current, Character target, float value)
        {
            target.AddEffect(new ActiveEffect(Type, value, RoundsActive));
        }

        public void MagicalDamage(Character current, Character target, float value, Element element)
        {
            var damage = value + current.Total.CalculateMagicalPower();

            GameManager.Combat.Damage(current, target, damage, element, Type);
        }

        public void DrainLife(Character current, Character target, float value)
        {
            var damage = value + current.Total.CalculateMagicalPower();
            var damageDealt = GameManager.Combat.Damage(current, target, damage, Element.Darkness, Type, canEvade:false);

            if (damageDealt > 0)
            {
                GameManager.Combat.Heal(current, current, damageDealt, Type, unavoidable:true);
            }
        }

        public void Heal(Character current, Character target, float value)
        {
            var heal = value + current.Total.CalculateMagicalPower();

            GameManager.Combat.Heal(current, target, heal, Type);
        }

        public void IncreaseDamagePercentage(Character current, Character target, float value)
        {
            value = value / 1000;
            target.AddEffect(new ActiveEffect(Type, value, RoundsActive));
        }

        public void PhysicalDamage(Character current, Character target, float value)
        {
            var damage = value + current.Total.CalculatePhysicalDamage();

            GameManager.Combat.Damage(current, target, damage, Element.None, Type);
        }

        public void Revive(Character current, Character target, float value)
        {
            var life = target.Health.Maximum * (value / 1000);

            GameManager.Combat.Heal(current, target, life, Type, true);
        }

        #endregion

        public void Apply(Character current, Character target, TargetRestrictions targetRestrictions)
        {
            Action<Character> action = t =>
                {
                    // Checks for active effects and modifies the value based on them.
                    float value = (float)RandomManager.GetDouble(MinimumValue, MaximumValue + 1);

                    switch (Type)
                    {
                        case EffectType.AbsorbDamagePercentage: AbsorbPercentage(current, t, value);
                            break;

                        case EffectType.AbsorbDamageRaw: AbsorbRaw(current, t, value);
                            break;

                        case EffectType.AbsorbMagicPercentage: AbsorbPercentage(current, t, value);
                            break;

                        case EffectType.AbsorbMagicRaw: AbsorbRaw(current, t, value);
                            break;

                        case EffectType.AbsorbPhysicalDamagePercentage: AbsorbRaw(current, t, value);
                            break;

                        case EffectType.AbsorbPhysicalDamageRaw: AbsorbRaw(current, t, value);
                            break;

                        case EffectType.AirDamage: MagicalDamage(current, t, value, Element.Air);
                            break;

                        case EffectType.DarknessDamage: MagicalDamage(current, t, value, Element.Darkness);
                            break;

                        case EffectType.DrainLife: DrainLife(current, t, value);
                            break;

                        case EffectType.EarthDamage: MagicalDamage(current, t, value, Element.Earth);
                            break;

                        case EffectType.FireDamage: MagicalDamage(current, t, value, Element.Fire);
                            break;

                        case EffectType.Heal: Heal(current, t, value);
                            break;

                        case EffectType.IceDamage: MagicalDamage(current, t, value, Element.Ice);
                            break;

                        case EffectType.IncreaseDamagePercentage: IncreaseDamagePercentage(current, t, value);
                            break;

                        case EffectType.IncreaseMagicPercentage: IncreaseDamagePercentage(current, t, value);
                            break;

                        case EffectType.IncreasePhysicalDamagePercentage: IncreaseDamagePercentage(current, t, value);
                            break;

                        case EffectType.LightDamage: MagicalDamage(current, t, value, Element.Light);
                            break;

                        case EffectType.PhysicalDamage: PhysicalDamage(current, t, value);
                            break;

                        case EffectType.Revive: Revive(current, t, value);
                            break;

                        case EffectType.ShockDamage: MagicalDamage(current, t, value, Element.Shock);
                            break;

                        case EffectType.WaterDamage: MagicalDamage(current, t, value, Element.Water);
                            break;
                    }
                };

            if (AreaEffect)
            {
                var targetGroup = CombatSceneManager.GetBattle().Units.GroupOf(target);
                targetGroup.ForEach(x =>
                    {
                        if (targetRestrictions.Validate(current, x))
                            action(x);
                    });
            }
            else if (targetRestrictions.Validate(current, target))
            {
                action(target);
            }
        }
    }
}
