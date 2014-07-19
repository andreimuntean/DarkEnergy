using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Combat;

namespace DarkEnergy.Characters
{
    public class Enemy : Character
    {
        public int Currency { get; set; }
        public int Experience { get; set; }

        public override int Level
        {
            get { return base.Level; }
            set
            {
                base.Level = value;
                Health = new Quantity(Total.CalculateMaximumHealth());
                DarkEnergy = new Quantity(Total.CalculateDarkEnergy());
            }
        }

        public Enemy(string name, Element defensiveElement, Element offensiveElement, Attributes attributes, List<AbilitySet> abilitySets, int currency, int experience, int textureId, int width, int height) : base(width, height)
        {
            Name = name;
            DefensiveElement = defensiveElement;
            OffensiveElement = offensiveElement;
            Base = attributes;
            AbilitySets = abilitySets;
            Currency = currency;
            Experience = experience;
            Path = @"Characters\Enemies\" + textureId.ToString() + ".dds";
        }

        // The enemy AI will be improved in the future
        // (possibly by using LUA scripts) but for now:
        public CombatAction ComputeAction(Units units)
        {
            var abilitySets = new List<AbilitySet>(AbilitySets);
            abilitySets.Shuffle();

            // Randomly iterates through this unit's ability sets.
            foreach (var abilitySet in abilitySets)
            {
                var abilities = new List<Ability>(abilitySet.Abilities);
                abilities.Shuffle();

                // Randomly iterates through every ability in this ability set.
                foreach (var ability in abilities)
                {
                    // Verifies whether there is enough Dark Energy to use
                    // this ability and if the level requirement is met.
                    if (DarkEnergy >= ability.DarkEnergyCost && Level >= ability.RequiredLevel)
                    {
                        var targets = new List<Character>(units.All);
                        targets.Shuffle();

                        // Randomly searches for a target.
                        foreach (var target in targets)
                        {
                            // Verifies whether this unit can be targeted.
                            if (ability.TargetRestrictions.Validate(this, target))
                            {
                                // Creates the action.
                                return new CombatAction(ability, this, target);
                            }
                        }
                    }
                }
            }

            // Returns the default value.
            return new CombatAction(AbilitySets[0].Abilities[0], this, units.FirstAlive(units.GroupA));
        }
    }
}
