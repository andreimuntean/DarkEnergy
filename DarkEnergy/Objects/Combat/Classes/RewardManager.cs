using System;
using System.Collections.Generic;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Inventory;

namespace DarkEnergy.Combat
{
    public struct Reward
    {
        public int Experience { get; private set; }
        public int Currency { get; private set; }
        public int DarkCrystals { get; private set; }
        public List<int> ItemIdList { get; private set; }

        public Reward(int experience, int currency, int darkCrystals, List<int> itemIdList) : this()
        {
            Experience = experience;
            Currency = currency;
            DarkCrystals = darkCrystals;
            ItemIdList = itemIdList;
        }
    }

    public class RewardManager
    {
        private const float experienceBoost = 3.0f;
        private Battle battle;
        private Reward? reward;

        public RewardManager(Battle battle)
        {
            this.battle = battle;
        }

        public Reward? GetReward()
        {
            if (reward == null)
            {
                int experience = (int)(experienceBoost * CalculateExperienceReceived());
                int currency = CalculateCurrencyReceived();
                int darkCrystals = CalculateDarkCrystalsReceived();
                List<int> itemIdList = CalculateItemsReceived();

                reward = new Reward(experience, currency, darkCrystals, itemIdList);
            }

            return reward;
        }

        public void Assign(List<Character> characters)
        {
            var reward = GetReward().Value;
            GameManager.Inventory.Currency += reward.Currency;
            GameManager.Inventory.DarkCrystals += reward.DarkCrystals;
            GameManager.Hero.IncreaseExperience(reward.Experience);
        }

        protected int CalculateCurrencyReceived()
        {
            int result = 0;

            if (battle.Victory)
            {
                foreach (var unit in battle.Units.GroupB)
                {
                    if (unit is Enemy)
                    {
                        var enemy = unit as Enemy;
                        result += (int)(enemy.Currency * enemy.Level / 2.0f);
                    }
                }
            }

            return result;
        }

        protected int CalculateDarkCrystalsReceived()
        {
            int result = 0;

            if (battle.Victory)
            {
                var random = new Random();
                while (random.NextDouble() <= 0.01 && result < 100)
                {
                    result += 1;
                }
            }

            return result;
        }

        protected List<int> CalculateItemsReceived()
        {
            List<int> itemIdList = new List<int>();

            if (battle.Victory)
            {
                
            }

            return itemIdList;
        }

        protected int CalculateExperienceReceived()
        {
            int result = 0;

            if (battle.Victory)
            {
                foreach (var unit in battle.Units.GroupB)
                {
                    if (unit is Enemy)
                    {
                        var enemy = unit as Enemy;
                        result += (int)(enemy.Experience * enemy.Level / 2.0f);
                    }
                }
            }

            return result;
        }
    }
}
