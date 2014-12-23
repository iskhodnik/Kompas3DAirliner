using Airliner.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{   
    /// <summary>
    /// Тестирование методов класса InfAirliner
    /// </summary>
    [TestClass]
    public class InfAirUT
    {
        /// <summary>
        /// Тестирование конструктора. Инициализация поля LengthOfAircraft
        /// </summary>
        [TestMethod]
        public void TestLengthOfAircraft()
        {
            var infAir = new InfAirliner(1, 0, 0, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.LengthOfAircraft, 1);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля FuselageDiameter
        /// </summary>
        [TestMethod]
        public void TestFuselageDiameter()
        {
            var infAir = new InfAirliner(0, 2, 0, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.FuselageDiameter, 2);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля Wingspan
        /// </summary>
        [TestMethod]
        public void TestWingspan()
        {
            var infAir = new InfAirliner(0, 0, 3, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.Wingspan, 3);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля HorizontalPositionWing
        /// </summary>
        [TestMethod]
        public void TestHorizontalPositionWing()
        {
            var infAir = new InfAirliner(0, 0, 0, 4, 0, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.HorizontalPositionWing, 4);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля VerticalPositionWing
        /// </summary>
        [TestMethod]
        public void TestVerticalPositionWing()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 5, 0, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.VerticalPositionWing, 5);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля SweepbackAngle
        /// </summary>
        [TestMethod]
        public void TestSweepbackAngle()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 6, 0, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.SweepbackAngle, 6);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля HeightOfKeel
        /// </summary>
        [TestMethod]
        public void TestHeightOfKeel()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 0, 7, 0, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.HeightOfKeel, 7);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля LengthOfHorizontalStabilizer
        /// </summary>
        [TestMethod]
        public void TestLengthOfHorizontalStabilizer()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 0, 0, 8, (TypesQuantityOfEngine)2);
            Assert.AreEqual(infAir.LengthOfHorizontalStabilizer, 8);
        }

        /// <summary>
        /// Тестирование конструктора. Инициализация поля TypeQuantityOfEngine
        /// </summary>
        [TestMethod]
        public void TestTypeQuantityOfEngine()
        {
            var infAir = new InfAirliner(0, 0, 0, 0, 0, 0, 0, 0, (TypesQuantityOfEngine)4);
            Assert.AreEqual(infAir.TypeQuantityOfEngine, (TypesQuantityOfEngine)4);
        }
    }
}
