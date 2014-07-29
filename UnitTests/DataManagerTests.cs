using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarkEnergy;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;
using DarkEnergy.Inventory;
using DarkEnergy.Trading;

namespace UnitTests
{
    public class DataManagerTests
    {
        [TestMethod]
        public void CanLoadAbility()
        {
            var ability = DataManager.Load<Ability>(100000);
            Assert.IsNotNull(ability, "DataManager could not load an ability.");

            ability = DataManager.Load<Ability>(-1);
            Assert.IsNull(ability, "DataManager incorrectly loads abilities.");
        }

        [TestMethod]
        public void CanLoadEnemy()
        {
            var enemy = DataManager.Load<Enemy>(10000);
            Assert.IsNotNull(enemy, "DataManager could not load an enemy.");

            enemy = DataManager.Load<Enemy>(-1);
            Assert.IsNull(enemy, "DataManager incorrectly loads enemies.");
        }

        [TestMethod]
        public void CanLoadItems()
        {
            var item = DataManager.Load<IItem>(100000);
            Assert.IsNotNull(item, "DataManager could not load a weapon.");

            item = DataManager.Load<IItem>(110000);
            Assert.IsNotNull(item, "DataManager could not load a relic.");

            item = DataManager.Load<IItem>(120000);
            Assert.IsNotNull(item, "DataManager could not load head armor.");

            item = DataManager.Load<IItem>(130000);
            Assert.IsNotNull(item, "DataManager could not load neck armor.");

            item = DataManager.Load<IItem>(140000);
            Assert.IsNotNull(item, "DataManager could not load chest armor.");

            item = DataManager.Load<IItem>(150000);
            Assert.IsNotNull(item, "DataManager could not load back armor.");

            item = DataManager.Load<IItem>(160000);
            Assert.IsNotNull(item, "DataManager could not load hand armor.");

            item = DataManager.Load<IItem>(170000);
            Assert.IsNotNull(item, "DataManager could not load finger armor.");

            item = DataManager.Load<IItem>(180000);
            Assert.IsNotNull(item, "DataManager could not load legs armor.");

            item = DataManager.Load<IItem>(190000);
            Assert.IsNotNull(item, "DataManager could not load feet armor.");
        }

        [TestMethod]
        public void CanLoadVendor()
        {
            var vendor = DataManager.Load<Vendor>(10000);
            Assert.IsNotNull(vendor, "Could not load vendor.");
        }
    }
}
