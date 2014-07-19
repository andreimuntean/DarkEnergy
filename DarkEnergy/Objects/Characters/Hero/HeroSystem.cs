using System.Collections.Generic;
using DarkEnergy.Abilities;
using DarkEnergy.Inventory;

namespace DarkEnergy.Characters.Hero
{
    public class HeroSystem : ILoadable, ISaveable
    {
        public string Name { get; set; }
        public int Level { get; private set; }
        public int AttributePoints { get; set; }
        public int MasteryPoints { get; set; }

        public List<AbilitySet> AbilitySets { get; set; }

        public Attributes Base { get; set; }
        public Attributes Total { get { return Base + Equipment.Attributes; } }

        public Element DefensiveElement { get { return (Equipment.Relic != null) ? Equipment.Relic.Element : Element.None; } }
        public Element OffensiveElement { get { return Equipment.Weapon.Element; } }

        public Features Features { get; set; }

        public Quantity Experience
        {
            get { return new Quantity(CurrentExperience, RequiredExperience); }
        }

        public Equipment Equipment
        {
            get { return GameManager.Inventory.Equipment; }
        }

        protected int CurrentExperience, RequiredExperience;

        public HeroSystem()
        {
            Base = new Attributes();
            Features = new Features();
        }

        protected void LevelUp()
        {
            Level += 1;
            AttributePoints += 5;
            MasteryPoints += 1;
            CurrentExperience = 0;
            RequiredExperience = Level * 100;
        }

        public void IncreaseExperience(int value)
        {
            if (value > 0)
            {
                if (CurrentExperience + value >= RequiredExperience)
                {
                    value -= (RequiredExperience - CurrentExperience);
                    LevelUp();
                    IncreaseExperience(value);
                }
                else
                {
                    CurrentExperience += value;
                }
            }
        }

        public void LoadData()
        {
            Name = DataStorageManager.Load<string>("HeroName");
            Level = DataStorageManager.Load<int>("HeroLevel");
            AttributePoints = DataStorageManager.Load<int>("HeroAttributePoints");
            MasteryPoints = DataStorageManager.Load<int>("HeroMasteryPoints");
            CurrentExperience = DataStorageManager.Load<int>("HeroCurrentExperience");
            RequiredExperience = DataStorageManager.Load<int>("HeroRequiredExperience");
            Base = DataStorageManager.Load<Attributes>("HeroBase");
            
            AbilitySets = new List<AbilitySet>();
            var abilitySets = DataStorageManager.Load<object[][]>("HeroAbilitySets");
            
            foreach (var set in abilitySets)
            {
                var idList = new List<int>();

                for (int i = 2; i <= 5; ++i)
                {
                    var value = (int)set[i];
                    if (value > -1) idList.Add(value);
                }

                AbilitySets.Add(new AbilitySet((string)set[0], (int)set[1], idList));
            }

            Features.LoadData();
        }

        public void SaveData()
        {
            DataStorageManager.Save(Name, "HeroName");
            DataStorageManager.Save(Level, "HeroLevel");
            DataStorageManager.Save(AttributePoints, "HeroAttributePoints");
            DataStorageManager.Save(MasteryPoints, "HeroMasteryPoints");
            DataStorageManager.Save(CurrentExperience, "HeroCurrentExperience");
            DataStorageManager.Save(RequiredExperience, "HeroRequiredExperience");
            DataStorageManager.Save(Base, "HeroBase");

            var abilitySets = new List<object[]>();
            foreach (var set in AbilitySets)
            {
                var name = set.Name;
                var template = set.Template;
                var idList = new int[] { -1, -1, -1, -1 };

                for (var i = 0; i < set.Abilities.Count && i < 4; ++i)
                    idList[i] = set.Abilities[i].Id;

                abilitySets.Add(new object[] { name, template, idList[0], idList[1], idList[2], idList[3] });
            }

            DataStorageManager.Save(abilitySets.ToArray(), "HeroAbilitySets");

            Features.SaveData();
        }
    }
}
