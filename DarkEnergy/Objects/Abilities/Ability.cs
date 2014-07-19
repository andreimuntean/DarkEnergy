using System.Collections.Generic;
using DarkEnergy.Characters;

namespace DarkEnergy.Abilities
{
    public class Ability
    {
        protected int NextId;
        protected int PreviousId;

        public int Id { get; protected set; }
        public int IconId { get; protected set; }
        public string Name { get; protected set; }
        public int Rank { get; protected set; }
        public int HighestRank { get; protected set; }
        public int RequiredLevel { get; protected set; }
        public int DarkEnergyCost { get; protected set; }
        public List<Effect> Effects { get; protected set; }
        public TargetRestrictions TargetRestrictions { get; protected set; }
        public CharacterState Animation { get; protected set; }
        public int VisualId { get; protected set; }

        public Ability(int id, int iconId, string name, int rank, int highestRank, int previousId, int nextId, int requiredLevel, int darkEnergyCost, List<Effect> effects, CharacterState animation, int visual, TargetRestrictions restrictions)
        {
            Id = id;
            IconId = iconId;
            Name = name;
            Rank = rank;
            HighestRank = highestRank;
            PreviousId = previousId;
            NextId = nextId;
            RequiredLevel = requiredLevel;
            DarkEnergyCost = darkEnergyCost;
            Effects = effects;
            Animation = animation;
            VisualId = visual;
            TargetRestrictions = restrictions;
        }

        protected void SetValues(Ability ability)
        {
            Id = ability.Id;
            IconId = ability.IconId;
            Name = ability.Name;
            Rank = ability.Rank;
            HighestRank = ability.HighestRank;
            PreviousId = ability.PreviousId;
            NextId = ability.NextId;
            RequiredLevel = ability.RequiredLevel;
            DarkEnergyCost = ability.DarkEnergyCost;
            Effects = ability.Effects;
            Animation = ability.Animation;
            VisualId = ability.VisualId;
            TargetRestrictions = ability.TargetRestrictions;
        }

        public Ability GetNextRank()
        {
            if (Rank < HighestRank && NextId != -1)
            {
                return DataManager.Load<Ability>(NextId);
            }
            else
            {
                return null;
            }
        }

        public Ability GetPreviousRank()
        {
            if (Rank > 0 && PreviousId != -1)
            {
                return DataManager.Load<Ability>(PreviousId);
            }
            else
            {
                return null;
            }
        }

        public void IncreaseRank()
        {
            if (Rank < HighestRank)
            {
                SetValues(GetNextRank());
            }
        }

        public void DecreaseRank()
        {
            if (Rank > 0)
            {
                SetValues(GetPreviousRank());
            }
        }

        public void SetRank(int rank)
        {
            if (Rank != rank)
            {
                if (Rank < rank) IncreaseRank();
                else if (Rank > rank) DecreaseRank();
                SetRank(rank);
            }
        }

        public string GetDescription()
        {
            var result = "";

            foreach (var effect in Effects)
            {
                var value = (effect.MinimumValue == effect.MaximumValue) ? effect.MinimumValue.ToString() : effect.MinimumValue + "-" + effect.MaximumValue;
                var valuePercentage = (effect.MinimumValue == effect.MaximumValue) ? (effect.MinimumValue / 10f).ToString() : effect.MinimumValue / 10f + "-" + effect.MaximumValue / 10f;
                var target = effect.AreaEffect ? Resources.Strings.EffectTargetedGroup : Resources.Strings.EffectTarget;
                var roundsActive = effect.RoundsActive > 0 ? Resources.Strings.EffectUptime.Replace("{R}", effect.RoundsActive.ToString()) : "";

                switch (effect.Type)
                {
                    case EffectType.AbsorbDamagePercentage: result += Resources.Strings.EffectAbsorbDamagePercentage.Replace("{T}", target).Replace("{V}", valuePercentage);
                        break;

                    case EffectType.AbsorbDamageRaw: result += Resources.Strings.EffectAbsorbDamageRaw.Replace("{T}", target);
                        break;

                    case EffectType.AbsorbMagicPercentage: result += Resources.Strings.EffectAbsorbMagicPercentage.Replace("{T}", target).Replace("{V}", valuePercentage);
                        break;

                    case EffectType.AbsorbMagicRaw: result += Resources.Strings.EffectAbsorbMagicRaw.Replace("{T}", target);
                        break;

                    case EffectType.AbsorbPhysicalDamagePercentage: result += Resources.Strings.EffectAbsorbPhysicalDamagePercentage.Replace("{T}", target).Replace("{V}", valuePercentage);
                        break;

                    case EffectType.AbsorbPhysicalDamageRaw: result += Resources.Strings.EffectAbsorbPhysicalDamageRaw.Replace("{T}", target);
                        break;
                    
                    case EffectType.AirDamage: result += Resources.Strings.EffectAirDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.DarknessDamage: result += Resources.Strings.EffectDarknessDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.DrainLife: result += Resources.Strings.EffectDrainLife.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.EarthDamage: result += Resources.Strings.EffectEarthDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.FireDamage: result += Resources.Strings.EffectFireDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.Heal: result += Resources.Strings.EffectHeal.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.IceDamage: result += Resources.Strings.EffectIceDamage.Replace("{T}", target.ToLowerInvariant());
                        break;

                    case EffectType.IncreaseDamagePercentage: result += Resources.Strings.EffectIncreaseDamagePercentage.Replace("{T}", target.ToLowerInvariant()).Replace("{V}", valuePercentage);
                        break;

                    case EffectType.IncreaseMagicPercentage: result += Resources.Strings.EffectIncreaseMagicPercentage.Replace("{T}", target.ToLowerInvariant()).Replace("{V}", valuePercentage);
                        break;

                    case EffectType.IncreasePhysicalDamagePercentage: result += Resources.Strings.EffectIncreasePhysicalDamagePercentage.Replace("{T}", target.ToLowerInvariant()).Replace("{V}", valuePercentage);
                        break;
                    
                    case EffectType.LightDamage: result += Resources.Strings.EffectLightDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.PhysicalDamage: result += Resources.Strings.EffectPhysicalDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.Revive: result += Resources.Strings.EffectRevive.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.ShockDamage: result += Resources.Strings.EffectShockDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                    
                    case EffectType.WaterDamage: result += Resources.Strings.EffectWaterDamage.Replace("{T}", target.ToLowerInvariant());
                        break;
                }

                result = result.Replace("{V}", value) + roundsActive + "\n";
            }

            return result;
        }
    }
}
