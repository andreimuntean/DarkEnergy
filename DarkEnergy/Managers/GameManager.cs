using System;
using System.Collections.Generic;
using SharpDX;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Combat;
using DarkEnergy.Inventory;
using DarkEnergy.Inventory.Slots;
using DarkEnergy.Party;
using DarkEnergy.Quests;

namespace DarkEnergy
{
    static class GameManager
    {
        public static int SelectedCharacter { get; private set; }
        public static CombatSystem Combat { get; private set; }
        public static HeroSystem Hero { get; private set; }
        public static InventorySystem Inventory { get; private set; }
        public static PartySystem Party { get; private set; }
        public static QuestSystem Quests { get; private set; }

        public static void Construct()
        {
            Combat = new CombatSystem();
            Hero = new HeroSystem();
            Inventory = new InventorySystem();
            Party = new PartySystem();
            Quests = new QuestSystem();
        }

        public static void InitializeInventory()
        {
            Inventory.Clear();
            Inventory.Capacity = 40;
            Inventory.Coins = 0;
            Inventory.DarkCrystals = 0;

            Inventory.Add(100005);
            Inventory.Add(140000);
            Inventory.Add(180000);
            Inventory.Add(190000);

            Inventory.Equip(Inventory.Equipment, 100005, 140000, 180000, 190000);
        }

        public static void CreateCharacter(int id, string name, Gender gender, byte skin, byte face, byte hair)
        {
            // Selects this character.
            SelectedCharacter = id;

            // Saves the character features.
            DataStorageManager.Save(gender, "HeroGender");
            DataStorageManager.Save(skin, "HeroSkin");
            DataStorageManager.Save(face, "HeroFace");
            DataStorageManager.Save(hair, "HeroHair");

            // Saves the character details.
            DataStorageManager.Save(name, "HeroName");
            DataStorageManager.Save(1, "HeroLevel");
            DataStorageManager.Save(0, "HeroAttributePoints");
            DataStorageManager.Save(0, "HeroMasteryPoints");
            DataStorageManager.Save(0, "HeroCurrentExperience");
            DataStorageManager.Save(100, "HeroRequiredExperience");
            DataStorageManager.Save(new Attributes(5, 5, 5, 5, 5), "HeroBase");

            // Saves the starting abilities.
            var abilitySets = new object[][]
            {
                new object[] { "Aggression", 0, 100000, 100010, 100020, 100030 },
                new object[] { "Vengeance", 2, 100040, 100050, 100060, 100070 },
                new object[] { "Redemption", 1, 100080, 100090, 100100, 100110 }
            };
            DataStorageManager.Save(abilitySets, "HeroAbilitySets");

            // Saves the inventory.
            Inventory.SaveData();

            // Sets the starting scene.
            DataStorageManager.SaveCharacterLocation<Scenes.Intro>();

            // Writes all data to the disk.
            DataStorageManager.Flush();

            // Deselects the character.
            SelectedCharacter = -1;
        }

        public static void Start(int characterId)
        {
            SelectedCharacter = characterId;

            Action action = () =>
            {
                Inventory.LoadData();
                Quests.LoadData();
                Hero.LoadData();

                SceneManager.Play(DataStorageManager.LoadScene());
            };

            LoadingScreenManager.Show(action, Resources.Strings.LoadingWorld);
        }

        public static void SaveGame()
        {
            Inventory.SaveData();
            Quests.SaveData();
            Hero.SaveData();
            DataStorageManager.Flush();
        }
    }
}
