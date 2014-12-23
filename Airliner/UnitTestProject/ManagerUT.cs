using Airliner.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    /// <summary>
    /// Тестирование методов класса InfAirliner
    /// </summary>
    [TestFixture]
    public class ManagerUT
    {
        /// <summary>
        /// Тестирование метода OpenKompas3D. Открытие программы Компас3D
        /// </summary>
        [Test]
        public void TestOpenKompas()
        {
            var men = new Manager();
            Assert.DoesNotThrow(men.OpenKompas3D);
        }
    }
}