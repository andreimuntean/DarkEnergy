using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarkEnergy.Characters;

namespace UnitTests
{
    [TestClass]
    public class AttributesTest
    {
        [TestMethod]
        public void CorrectlyHandlesWeaponDamage()
        {
            var x = new Attributes(1, 1, 1, 1, 1, 1, 1);
            var y = new Attributes(2, 1, 1, 1, 1, 1, 1);

            Assert.IsTrue(x.CalculatePhysicalDamage() < y.CalculatePhysicalDamage(), "Weapon Damage does not work as intended.");
        }

        [TestMethod]
        public void CorrectlyHandlesArmor()
        {
            var x = new Attributes(1, 1, 1, 1, 1, 1, 1);
            var y = new Attributes(1, 2, 1, 1, 1, 1, 1);

            Assert.IsTrue(x.CalculateDefense() < y.CalculateDefense(), "Armor does not work as intended.");
        }

        [TestMethod]
        public void CorrectlyHandlesStrength()
        {
            var x = new Attributes(1, 1, 1, 1, 1);
            var y = new Attributes(2, 1, 1, 1, 1);

            Assert.IsTrue(x.CalculateDefense() < y.CalculateDefense(), "Strength does not work as intended.");
            Assert.IsTrue(x.CalculatePhysicalDamage() < y.CalculatePhysicalDamage(), "Strength does not work as intended.");
        }

        [TestMethod]
        public void CorrectlyHandlesIntuition()
        {
            var x = new Attributes(1, 1, 1, 1, 1);
            var y = new Attributes(1, 2, 1, 1, 1);

            Assert.IsTrue(x.CalculateMagicalPower() < y.CalculateMagicalPower(), "Intuition does not work as intended.");
        }

        [TestMethod]
        public void CorrectlyHandlesReflexes()
        {
            var x = new Attributes(1, 1, 1, 1, 1);
            var y = new Attributes(1, 1, 2, 1, 1);

            Assert.IsTrue(x.CalculateEvasion(1) < y.CalculateEvasion(1), "Reflexes does not work as intended.");
            Assert.IsTrue(x.CalculateSpeed() < y.CalculateSpeed(), "Reflexes does not work as intended.");
        }

        [TestMethod]
        public void CorrectlyHandlesVitality()
        {
            var x = new Attributes(1, 1, 1, 1, 1);
            var y = new Attributes(1, 1, 1, 2, 1);

            Assert.IsTrue(x.CalculateMaximumHealth() < y.CalculateMaximumHealth(), "Vitality does not work as intended.");
        }

        [TestMethod]
        public void CorrectlyHandlesVigor()
        {
            var x = new Attributes(1, 1, 1, 1, 1);
            var y = new Attributes(1, 1, 1, 1, 2);

            Assert.IsTrue(x.CalculateDarkEnergyGenerated() < y.CalculateDarkEnergyGenerated(), "Vigor does not work as intended.");
        }
    }
}
