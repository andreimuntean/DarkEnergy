using System;
using System.Collections.Generic;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat
{
    public enum AttackStatus
    {
        None,
        Missed,
        Evaded,
        Resisted,
        Successful
    }

    public class CombatSystem
    {
        private const double missChance = 0.04;
        private List<CharacterCombatData> turnData;

        private CharacterCombatData getTargetData(Character target, bool createIfNull = true)
        {
            CharacterCombatData characterCombatData = null;

            turnData.ForEach(data => { if (data.Character == target) characterCombatData = data; });

            if (characterCombatData == null)
            {
                if (createIfNull)
                {
                    characterCombatData = new CharacterCombatData(target);
                    turnData.Add(characterCombatData);
                }
                else return null;
            }

            return characterCombatData;
        }

        private void evaluateActiveEffects(Character current, Character target, EffectType type, ref float value)
        {
            float result = value;

            current.ActiveEffects.ForEach(activeEffect => activeEffect.Evaluate(current, current, type, ref result));

            if (current != target)
            {
                target.ActiveEffects.ForEach(activeEffect => activeEffect.Evaluate(current, target, type, ref result));
            }

            value = result;
        }

        public List<CharacterCombatData> GetTurnData()
        {
            return turnData;
        }

        public void Next()
        {
            turnData = new List<CharacterCombatData>();
        }

        public bool AllAttacksMissed()
        {
            if (turnData != null)
            {
                foreach (var characterData in turnData)
                {
                    if (characterData.AttackStatus != AttackStatus.Missed && characterData.AttackStatus != AttackStatus.Evaded)
                        return false;
                }
            }
            return true;
        }

        public float Damage(Character current, Character target, float value, Element element, EffectType type, bool canEvade = true, bool unavoidable = false)
        {
            var targetData = getTargetData(target);
            float damageDealt = value;

            if (targetData.AttackStatus == AttackStatus.None)
            {
                // Checking if the attack missed.
                if (RandomManager.GetDouble() <= missChance && !unavoidable)
                {
                    targetData.AttackStatus = AttackStatus.Missed;
                    return 0;
                }
                
                // Checking if the enemy evaded it.
                var evasion = target.Total.CalculateEvasion(target is Enemy ? current.Level : 0);
                if (RandomManager.GetDouble() <= evasion && canEvade)
                {
                    targetData.AttackStatus = AttackStatus.Evaded;
                    return 0;
                }

                targetData.AttackStatus = AttackStatus.Successful;
            }
            
            if (targetData.AttackStatus == AttackStatus.Successful)
            {
                evaluateActiveEffects(current, target, type, ref damageDealt);                

                if (type == EffectType.PhysicalDamage)
                {
                    // Turns the physical attack into a magical one
                    // based on the offensive element of the attacker.

                    element = current.OffensiveElement;

                    switch (element)
                    {
                        case Element.None: break;
                        case Element.Light: type = EffectType.LightDamage; break;
                        case Element.Air: type = EffectType.AirDamage; break;
                        case Element.Ice: type = EffectType.IceDamage; break;
                        case Element.Water: type = EffectType.WaterDamage; break;
                        case Element.Darkness: type = EffectType.DarknessDamage; break;
                        case Element.Earth: type = EffectType.EarthDamage; break;
                        case Element.Fire: type = EffectType.FireDamage; break;
                        case Element.Shock: type = EffectType.ShockDamage; break;
                    }
                }

                // Checking enemy resistances.
                var modifier = Resistances.GetModifier(element, target.DefensiveElement);
                damageDealt *= modifier;

                // Logs this element if it is special to display it later.
                if (element != Element.None && targetData.AbilityElement == Element.None)
                {
                    targetData.AbilityElement = element;
                }
                targetData.ElementModifiers += modifier;

                // Checking enemy protection.
                damageDealt -= damageDealt * target.Total.CalculateDefense();
                targetData.TotalDamageReceived += damageDealt;
            }

            return damageDealt;
        }

        public float Heal(Character current, Character target, float value, EffectType type, bool unavoidable = false)
        {
            var targetData = getTargetData(target);
            float healingDone = 0;

            if (targetData.AttackStatus == AttackStatus.None)
            {
                // Checking accuracy.
                if (RandomManager.GetDouble() <= missChance && !unavoidable)
                {
                    targetData.AttackStatus = AttackStatus.Missed;
                    return 0;
                }

                targetData.AttackStatus = AttackStatus.Successful;
            }

            if (targetData.AttackStatus == AttackStatus.Successful)
            {
                healingDone = value;
                evaluateActiveEffects(current, target, type, ref healingDone);
                targetData.TotalHealingReceived += healingDone;
            }

            return healingDone;
        }

        public void EndAction()
        {
            var battle = CombatSceneManager.GetBattle();
            var characters = new List<Character>();

            if (battle != null)
            {
                characters.AddRange(battle.Units.GroupA);
                characters.AddRange(battle.Units.GroupB);

                characters.ForEach(character =>
                {
                    var characterData = getTargetData(character, false);

                    if (characterData != null)
                    {
                        if (characterData.AttackStatus == AttackStatus.Successful && characterData.TotalHealingReceived - characterData.TotalDamageReceived == 0)
                        {
                            characterData.AttackStatus = AttackStatus.Resisted;
                        }

                        character.Health += (int)(characterData.TotalHealingReceived - characterData.TotalDamageReceived);
                    }
                });
            }
        }
    }
}
