using DarkEnergy.Abilities;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat
{
    public class CombatAction
    {
        public Ability Ability { get; set; }
        public Character Current { get; set; }
        public Character Target { get; set; }

        public CombatAction(Ability ability, Character character, Character target)
        {
            Ability = ability;
            Current = character;
            Target = target;
        }

        public void Use()
        {
            Current.DarkEnergy -= Ability.DarkEnergyCost;

            Ability.Effects.ForEach(effect =>
            {
                effect.Apply(Current, Target, Ability.TargetRestrictions);
            });

            GameManager.Combat.EndAction();
        }
    }
}
