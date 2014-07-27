using System;
using System.Collections.Generic;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Inventory.Slots;

namespace DarkEnergy.Inventory
{
    public class InventorySystem : ILoadable, ISaveable
    {
        protected List<IItem> itemList;
        protected List<int> idList;

        protected const int maximumCapacity = 255;
        protected const int maximumCoins = 2000000000;
        protected const int maximumDarkCrystals = 2000000000;
        protected int capacity;
        protected int coins;
        protected int darkCrystals;

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value <= maximumCapacity)
                {
                    capacity = value;
                }
                else capacity = maximumCapacity;
            }
        }

        public int Coins
        {
            get { return coins; }
            set
            {
                if (value <= maximumCoins)
                {
                    coins = value;
                }
                else coins = maximumCoins;
            }
        }

        public int DarkCrystals
        {
            get { return darkCrystals; }
            set
            {
                if (value <= maximumDarkCrystals)
                {
                    darkCrystals = value;
                }
                else darkCrystals = maximumDarkCrystals;
            }
        }

        public List<IItem> Items { get { return itemList; } }
        public int Count { get { return Items.Count; } }
        public Equipment Equipment { get; protected set; }

        public InventorySystem()
        {
            Equipment = new Equipment();
            Clear();
        }

        protected void addStack(int id, ref int count)
        {
            var item = DataManager.Load<IItem>(id);

            if (item == null)
            {
                ExceptionManager.Log("Cannot load an item with id " + id.ToString() + ".");
                count = 0;
                return;
            }
            else
            {
                item.Count += count;
                count -= item.StackLimit;
                idList.Add(id);
                itemList.Add(item);
            }
        }

        public bool Contains(int id)
        {
            return idList.Contains(id);
        }

        public bool Contains(IItem item)
        {
            return itemList.Contains(item);
        }

        public bool CanAdd(IItem item, int count = 1)
        {
            count -= (Capacity - Count) * item.StackLimit;

            if (count <= 0) return true;

            foreach (var i in itemList)
            {
                if (i == item)
                {
                    count -= (i.StackLimit - i.Count);
                    if (count <= 0) return true;
                }
            }

            return false;
        }

        public bool IsUnique(int id, int count = 1)
        {
            bool result = false;

            for (int i = 0; i < idList.Count; ++i)
            {
                if (idList[i] == id)
                {
                    if (result == false && itemList[i].Count == count)
                    {
                        result = true;
                    }
                    else return false;
                }
            }

            return result;
        }

        public bool IsUniqueStack(int id)
        {
            bool result = false;

            for (int i = 0; i < idList.Count; ++i)
            {
                if (idList[i] == id)
                {
                    if (result == false)
                    {
                        result = true;
                    }
                    else return false;
                }
            }

            return result;
        }

        public bool IsEquipped(EquippableItem item)
        {
            foreach (var i in Equipment.ToList())
            {
                if (i == item)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the item with the specified id.
        /// </summary>
        /// <param name="id">An id.</param>
        /// <returns></returns>
        public IItem GetItemWithId(int id)
        {
            for (var i = 0; i < Count; ++i)
            {
                if (idList[i] == id)
                {
                    return itemList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the id of the specified item.
        /// </summary>
        /// <param name="item">An item.</param>
        /// <returns></returns>
        public int GetId(IItem item)
        {
            for (var i = 0; i < Count; ++i)
            {
                if (itemList[i] == item)
                {
                    return idList[i];
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the item equipped in the specified slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public EquippableItem GetEquippedItem(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Back: return Equipment.Back;
                case EquipmentSlot.Chest: return Equipment.Chest;
                case EquipmentSlot.Feet: return Equipment.Feet;
                case EquipmentSlot.Finger: return Equipment.Finger;
                case EquipmentSlot.Hands: return Equipment.Hands;
                case EquipmentSlot.Head: return Equipment.Head;
                case EquipmentSlot.Legs: return Equipment.Legs;
                case EquipmentSlot.Neck: return Equipment.Neck;
                case EquipmentSlot.Relic: return Equipment.Relic;
                case EquipmentSlot.Weapon: return Equipment.Weapon;
                default: return null;
            }
        }

        public void Equip(Equipment target, params EquippableItem[] items)
        {
            foreach (var item in items)
            {
                string itemSlot = item.GetType().Name;

                switch (itemSlot)
                {
                    case "Weapon": target.Weapon = item as Weapon; break;
                    case "Relic": target.Relic = item as Relic; break;
                    case "Head": target.Head = item as Head; break;
                    case "Neck": target.Neck = item as Neck; break;
                    case "Chest": target.Chest = item as Chest; break;
                    case "Back": target.Back = item as Back; break;
                    case "Hands": target.Hands = item as Hands; break;
                    case "Finger": target.Finger = item as Finger; break;
                    case "Legs": target.Legs = item as Legs; break;
                    case "Feet": target.Feet = item as Feet; break;
                }
            }
        }

        public void Equip(Equipment target, params int[] ids)
        {
            foreach (var id in ids)
            {
                if (id != -1)
                {
                    var index = idList.IndexOf(id);
                    if (index > -1)
                    {
                        var item = itemList[index];
                        string itemSlot = item.GetType().Name;

                        switch (itemSlot)
                        {
                            case "Weapon": target.Weapon = item as Weapon; break;
                            case "Relic": target.Relic = item as Relic; break;
                            case "Head": target.Head = item as Head; break;
                            case "Neck": target.Neck = item as Neck; break;
                            case "Chest": target.Chest = item as Chest; break;
                            case "Back": target.Back = item as Back; break;
                            case "Hands": target.Hands = item as Hands; break;
                            case "Finger": target.Finger = item as Finger; break;
                            case "Legs": target.Legs = item as Legs; break;
                            case "Feet": target.Feet = item as Feet; break;
                        }
                    }
                    else
                    {
                        ExceptionManager.Log("Cannot equip item of id " + id.ToString() + " because the character does not possess it.");
                    }
                }
            }
        }

        public void Clear()
        {
            idList = new List<int>();
            itemList = new List<IItem>();
            Coins = 0;
            DarkCrystals = 0;
            Equipment.Clear();
        }

        public void Add(int id, int count = 1)
        {
            if (id > 0)
            {
                for (var i = 0; i < Count; ++i)
                {
                    if (idList[i] == id)
                    {
                        if (itemList[i].IsStackable)
                        {
                            itemList[i].Count += count;
                            count -= itemList[i].StackLimit;
                            if (count < 1) return;
                        }
                    }
                }

                while (count > 0 && Count < Capacity)
                {
                    addStack(id, ref count);
                }
            }
            else
            {
                ExceptionManager.Log("Cannot add an item of id " + id.ToString() + " to the inventory.");
            }
        }

        public bool Remove(IItem item)
        {
            var id = GetId(item);
            var count = item.Count;
            return Remove(id, count);
        }

        public bool Remove(int id, int count = 1)
        {
            if (id > 0)
            {
                for (var i = Count - 1; i >= 0; --i)
                {
                    if (idList[i] == id)
                    {
                        var item = itemList[i];
                        
                        if (item is EquippableItem && IsEquipped(item as EquippableItem))
                        {
                            continue;
                        }

                        var difference = count - itemList[i].Count;

                        item.Count -= count;
                        if (item.Count < 1)
                        {
                            idList.RemoveAt(i);
                            itemList.RemoveAt(i);
                        }

                        count = difference;
                        if (count < 1) return true;
                    }
                }
            }
            else
            {
                ExceptionManager.Log("Cannot remove an item of id " + id.ToString() + " from the inventory.");
            }

            return false;
        }

        public void LoadData()
        {
            Clear();
            Capacity = DataStorageManager.Load<int>("InventoryCapacity");
            Coins = DataStorageManager.Load<int>("InventoryCurrency");
            DarkCrystals = DataStorageManager.Load<int>("InventoryDarkCrystals");
            idList = DataStorageManager.Load<List<int>>("InventoryIdList");

            idList.ForEach(id =>
            {
                itemList.Add(DataManager.Load<IItem>(id));
            });

            Equipment.LoadData();
        }

        public void SaveData()
        {
            DataStorageManager.Save(idList, "InventoryIdList");
            DataStorageManager.Save(Capacity, "InventoryCapacity");
            DataStorageManager.Save(Coins, "InventoryCurrency");
            DataStorageManager.Save(DarkCrystals, "InventoryDarkCrystals");
            Equipment.SaveData();
        }
    }
}
