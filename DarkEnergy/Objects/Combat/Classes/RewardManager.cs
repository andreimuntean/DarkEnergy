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
        private float experienceRate;
        private float coinsRate;
        private float darkCrystalsRate;
        private float itemRate;
        private Battle battle;
        private Reward? reward;

        public RewardManager(Battle battle)
        {
            this.battle = battle;
            experienceRate = float.Parse(Resources.Rates.Experience);
            coinsRate = float.Parse(Resources.Rates.Coins);
            darkCrystalsRate = float.Parse(Resources.Rates.DarkCrystals);
            itemRate = float.Parse(Resources.Rates.Item);
        }

        public Reward? GetReward()
        {
            if (reward == null)
            {
                int experience = CalculateExperienceReceived();
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
            GameManager.Inventory.Coins += reward.Currency;
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

            return (int)(Math.Round(coinsRate * result));
        }

        protected int CalculateDarkCrystalsReceived()
        {
            var chance = RandomManager.GetDouble();

            if (darkCrystalsRate > 0) chance /= darkCrystalsRate;
            else return 0;

            if (chance == 0) return 100;
            else return (int)(1 / chance / 100);
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

            return (int)(Math.Round(experienceRate * result));
        }
    }
}
