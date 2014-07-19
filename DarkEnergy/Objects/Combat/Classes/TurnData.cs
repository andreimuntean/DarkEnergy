using DarkEnergy.Characters;
using DarkEnergy.Combat;

namespace DarkEnergy.Combat
{
    public class CharacterCombatData
    {
        public Character Character { get; set; }
        public AttackStatus AttackStatus { get; set; }
        public Element AbilityElement { get; set; }
        public float TotalDamageReceived { get; set; }
        public float TotalHealingReceived { get; set; }
        public float ElementModifiers { get; set; }

        public CharacterCombatData(Character character)
        {
            Character = character;
            AttackStatus = AttackStatus.None;
            AbilityElement = Element.None;
            TotalDamageReceived = 0;
            TotalHealingReceived = 0;
            ElementModifiers = 0;
        }
    }
}
