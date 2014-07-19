using System.Collections.Generic;

namespace DarkEnergy.Abilities
{
    public class AbilitySet
    {
        private List<int> idList;

        public string Name { get; protected set; }
        public int Template { get; protected set; }

        private List<Ability> abilities;
        public List<Ability> Abilities
        {
            get
            {
                if (abilities == null && idList != null)
                {
                    abilities = new List<Ability>();
                    idList.ForEach(id => abilities.Add(DataManager.Load<Ability>(id)));
                }

                return abilities;
            }
        }

        public AbilitySet(string name, int template, List<Ability> abilities)
        {
            Name = name;
            Template = template;
            this.abilities = abilities;
        }

        public AbilitySet(string name, int template, List<int> idList)
        {
            Name = name;
            Template = template;
            this.idList = idList;
        }
    }
}
