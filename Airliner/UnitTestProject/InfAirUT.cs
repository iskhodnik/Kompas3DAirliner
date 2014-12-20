using Airliner.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class InfAirUT
    {
        [TestMethod]
        public void TestLengthOfAircraft()
        {
            var infAir = new InfAirliner(1, 0, 0, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.LengthOfAircraft, 1);
        }

        [TestMethod]
        public void TestFuselageDiameter()
        {
            var infAir = new InfAirliner(0, 2, 0, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.FuselageDiameter, 2);
        }

        [TestMethod]
        public void TestWingspan()
        {
            var infAir = new InfAirliner(0, 0, 3, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.Wingspan, 3);
        }

        [TestMethod]
        public void TestHorizontalPositionWing()
        {
            var infAir = new InfAirliner(0, 0, 0, 4, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.HorizontalPositionWing, 4);
        }

        [TestMethod]
        public void TestVerticalPositionWing()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 5, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.VerticalPositionWing, 5);
        }

        [TestMethod]
        public void TestSweepbackAngle()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 6, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.SweepbackAngle, 6);
        }

        [TestMethod]
        public void TestHeightOfKeel()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 0, 7, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.HeightOfKeel, 7);
        }

        [TestMethod]
        public void TestLengthOfHorizontalStabilizer()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 0, 0, 8, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.LengthOfHorizontalStabilizer, 8);
        }

        [TestMethod]
        public void TestTypeQuantityOfEngine()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)4);
            Assert.AreEqual(infAir.TypeQuantityOfEngine, (TypesQuantityOfEngine)4);
        }
    }
}
