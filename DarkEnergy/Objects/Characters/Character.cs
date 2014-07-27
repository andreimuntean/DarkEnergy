using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;

namespace DarkEnergy.Characters
{
    public enum CharacterState { Stand = 0, Charge = 1, Attack = 2, Retreat = 3, CastTarget = 4, CastSelf = 5 }
    
    public abstract class Character : TexturedElement
    {
        public virtual int Level { get; set; }
        public virtual string Name { get; set; }
        public virtual List<AbilitySet> AbilitySets { get; set; }
        public virtual Attributes Base { get; set; }
        public virtual Attributes Total { get { return (this is Hero.Hero) ? Base : Base * (1 + (Level - 1) / 5.0f); } }
        public virtual Quantity Health { get; set; }
        public virtual Quantity DarkEnergy { get; set; }
        public virtual Element DefensiveElement { get; set; }
        public virtual Element OffensiveElement { get; set; }
        public virtual CharacterState State { get; set; }
        public bool Alive { get { return Health > 0; } }
        public List<ActiveEffect> ActiveEffects { get; protected set; }

        public Character(int width, int height) : base(width, height)
        {
            ActiveEffects = new List<ActiveEffect>();
        }

        protected void SetValues(int level, string name, List<AbilitySet> abilitySets, Attributes baseAttributes, Quantity health, Quantity darkEnergy, Element defensiveElement, Element offensiveElement, CharacterState state)
        {
            Level = level;
            Name = name;
            AbilitySets = abilitySets;
            Base = baseAttributes;
            Health = health;
            DarkEnergy = darkEnergy;
            DefensiveElement = defensiveElement;
            OffensiveElement = offensiveElement;
            State = state;
        }

        public void AddEffect(ActiveEffect effect)
        {
            // Checks if this effect has already been applied.
            foreach (var activeEffect in ActiveEffects)
            {
                // If so, it overwrites it.
                if (activeEffect.Type == effect.Type)
                {
                    activeEffect.Rounds = effect.Rounds;
                    activeEffect.Value = effect.Value;
                    return;
                }
            }

            // Otherwise it adds it.
            ActiveEffects.Add(effect);
        }

        public override void Initialize()
        {
            State = CharacterState.Stand;
            Health = new Quantity(Total.CalculateMaximumHealth());
            DarkEnergy = new Quantity(Total.CalculateDarkEnergy());
            base.Initialize();
        }
    }
}
