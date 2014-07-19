using System;
using System.Collections.Generic;
using DarkEnergy.Characters;

namespace DarkEnergy.Party
{
    public class PartySystem : ILoadable, ISaveable
    {
        public List<Character> Members { get; protected set;}

        public PartySystem()
        {
            Members = new List<Character>();
        }

        public void LoadData()
        {
            
        }

        public void SaveData()
        {
            
        }
    }
}
