using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarkEnergy.Inventory;

namespace UnitTests
{
    public class InventoryTest
    {
        [TestMethod]
        public void CanInitializeInventory()
        {
            var inventory = new InventorySystem();
            Assert.IsNotNull(inventory.Items, "Inventory is incorrectly initialized.");
        }

        [TestMethod]
        public void CanManageItems()
        {
            var inventory = new InventorySystem();
            inventory.Capacity = 10;

            inventory.Add(100000);
            Assert.IsTrue(inventory.Items.Count == 1, "Items are incorrectly added.");

            inventory.Add(100001, 5);
            Assert.IsTrue(inventory.Items.Count == 6, "Items are incorrectly added.");

            inventory.Add(100001, 99);
            Assert.IsTrue(inventory.Items.Count == 10, "Items are incorrectly added.");

            inventory.Remove(100000);
            Assert.IsTrue(inventory.Items.Count == 9, "Items are incorrectly removed.");
            Assert.IsFalse(inventory.IdList.Contains(100000), "Items are incorrectly removed.");

            inventory.Remove(100001, 7);
            Assert.IsTrue(inventory.Items.Count == 2, "Items are incorrectly removed.");
            Assert.IsTrue(inventory.IdList[0] == 100001 && inventory.IdList[1] == 100001, "Items are incorrectly removed.");

            inventory.Remove(199999);
            Assert.IsTrue(inventory.Items.Count == 2, "Items are incorrectly removed.");

            inventory.Remove(199999, 99);
            Assert.IsTrue(inventory.Items.Count == 2, "Items are incorrectly removed.");

            inventory.Remove(100001, 99);
            Assert.IsTrue(inventory.Items.Count == 0, "Items are incorrectly removed.");

            inventory.Add(100000, 5);
            inventory.Clear();
            Assert.IsTrue(inventory.Items.Count == 0, "Inventory is incorrectly cleared.");
        }

        [TestMethod]
        public void CanModifyCapacity()
        {
            var inventory = new InventorySystem();
            var initialCapacity = inventory.Capacity;

            inventory.Capacity += 10;
            Assert.IsTrue(initialCapacity + 10 == inventory.Capacity, "Capacity is incorrectly modified.");

            inventory.Capacity -= 10;
            Assert.IsTrue(initialCapacity == inventory.Capacity, "Capacity is incorrectly modified.");

            inventory.Capacity = 20;
            Assert.IsTrue(inventory.Capacity == 20, "Capacity is incorrectly modified.");
        }

        [TestMethod]
        public void CanModifyCoins()
        {
            var inventory = new InventorySystem();
            var initialCoins = inventory.Coins;
            
            inventory.Coins += 10;
            Assert.IsTrue(initialCoins + 10 == inventory.Coins, "Coins are incorrectly modified.");

            inventory.Coins -= 10;
            Assert.IsTrue(initialCoins == inventory.Coins, "Coins are incorrectly modified.");

            inventory.Coins = 20;
            Assert.IsTrue(inventory.Coins == 20, "Coins are incorrectly modified.");
        }

        [TestMethod]
        public void CanModifyDarkCrystals()
        {
            var inventory = new InventorySystem();
            var initialDarkCrystals = inventory.DarkCrystals;

            inventory.DarkCrystals += 10;
            Assert.IsTrue(initialDarkCrystals + 10 == inventory.DarkCrystals, "Dark Crystals are incorrectly modified.");

            inventory.DarkCrystals -= 10;
            Assert.IsTrue(initialDarkCrystals == inventory.DarkCrystals, "Dark Crystals are incorrectly modified.");

            inventory.DarkCrystals = 20;
            Assert.IsTrue(inventory.DarkCrystals == 20, "Dark Crystals are incorrectly modified.");
        }
    }
}
