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
            Inventory.Capacity = 60;
            Inventory.Currency = 0;
            Inventory.DarkCrystals = 0;

            Inventory.Add(100005);
            Inventory.Add(140000);
            Inventory.Add(180000);
            Inventory.Add(190000);

            // Vendors are not currently active in the game,
            // therefore I have decided to have the character
            // start with a wide set of weapons and armor in
            // order to showcase the inventory system.

            #region Extra items
            Inventory.Add(120000);
            Inventory.Add(130000);
            Inventory.Add(150001);
            Inventory.Add(160001);
            Inventory.Add(170000);

            Inventory.Add(110000);
            Inventory.Add(110001);
            Inventory.Add(110002);
            Inventory.Add(110003);
            Inventory.Add(110004);
            Inventory.Add(110005);

            Inventory.Add(100000);

            Inventory.Add(140001);
            Inventory.Add(140002);
            Inventory.Add(140003);
            Inventory.Add(140004);
            Inventory.Add(140005);
            Inventory.Add(140006);
            Inventory.Add(140007);
            Inventory.Add(140008);
            Inventory.Add(140009);
            Inventory.Add(140010);
            Inventory.Add(140011);

            Inventory.Add(100010);

            Inventory.Add(180001);
            Inventory.Add(180002);
            Inventory.Add(180003);
            Inventory.Add(180004);
            Inventory.Add(180005);
            Inventory.Add(180006);
            Inventory.Add(180007);
            Inventory.Add(180008);
            Inventory.Add(180009);
            Inventory.Add(180010);
            Inventory.Add(180011);

            Inventory.Add(100011);

            Inventory.Add(190001);
            Inventory.Add(190002);
            Inventory.Add(190003);
            Inventory.Add(190004);
            Inventory.Add(190005);
            Inventory.Add(190006);
            Inventory.Add(190007);
            Inventory.Add(190008);
            Inventory.Add(190009);
            Inventory.Add(190010);
            Inventory.Add(190011);

            Inventory.Add(100020);
            #endregion

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
