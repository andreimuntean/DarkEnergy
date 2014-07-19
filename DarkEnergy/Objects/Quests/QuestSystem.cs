using System;
using System.Collections.Generic;
using DarkEnergy.Quests;

namespace DarkEnergy.Quests
{
    public class QuestSystem : ILoadable, ISaveable
    {
        private List<Quest> activeQuests;
        private List<Quest> completedQuests;

        public QuestSystem()
        {
            activeQuests = new List<Quest>();
            completedQuests = new List<Quest>();
        }

        public bool IsActive(Quest quest)
        {
            if (activeQuests.Contains(quest))
                return true;
            return false;
        }

        public bool IsCompleted(Quest quest)
        {
            if (completedQuests.Contains(quest))
                return true;
            return false;
        }

        public void Initialize()
        {
            activeQuests = new List<Quest>();
            completedQuests = new List<Quest>();
        }

        public void LoadData()
        {
            activeQuests = DataStorageManager.Load<List<Quest>>("ActiveQuests");
            completedQuests = DataStorageManager.Load<List<Quest>>("CompletedQuests");
        }

        public void SaveData()
        {
            DataStorageManager.Save(activeQuests, "ActiveQuests");
            DataStorageManager.Save(completedQuests, "CompletedQuests");
        }
    }
}
