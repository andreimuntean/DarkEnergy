using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DarkEnergy.Characters;

namespace UnitTests
{
    [TestClass]
    public class ResistancesTest
    {
        [TestMethod]
        public void CorrectlyComputesResistances()
        {
            var modifier = Resistances.GetModifier(Element.Light, Element.Darkness);
            Assert.IsTrue(modifier == 2.0f, "Resistance to very harmful elements is calculated incorrectly.");

            modifier = Resistances.GetModifier(Element.Light, Element.Water);
            Assert.IsTrue(modifier == 1.5f, "Resistance to harmful elements is calculated incorrectly.");

            modifier = Resistances.GetModifier(Element.Light, Element.Fire);
            Assert.IsTrue(modifier == 1.0f, "Resistance to neutral elements is calculated incorrectly.");

            modifier = Resistances.GetModifier(Element.Light, Element.Air);
            Assert.IsTrue(modifier == 0.5f, "Resistance to similar elements is calculated incorrectly.");

            modifier = Resistances.GetModifier(Element.Light, Element.Light);
            Assert.IsTrue(modifier == 0.1f, "Resistance to self is calculated incorrectly.");
        }
    }
}
