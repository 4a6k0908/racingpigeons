using System;
using System.IO;
using Core.Save;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor.Save
{
    [TestFixture]
    public class SaveSystemTests
    {
        [SetUp]
        public void SetUp()
        {
            saveSystem = new SaveSystem();
        }

        private SaveSystem saveSystem;

        [Test]
        public void _01_Should_Save_Data_To_Json_In_Local_Storage_Success()
        {
            var testData = new TestData(1, 5, "test");

            saveSystem.Save(testData, "test.dat");

            var filePath = Path.Combine(Application.persistentDataPath, "test.dat");
            FileShouldExist(filePath);
        }

        [Test]
        public void _02_Should_Load_Data_From_Local_Storage_Success_And_Correct()
        {
            var testData = saveSystem.Load<TestData>("test.dat");

            MoneyShouldBe(1, testData.money);
            CountShouldBe(5, testData.count);
            NameShouldBe("test", testData.name);

            saveSystem.Delete("test.dat");
        }

        private static void NameShouldBe(string expected, string name)
        {
            Assert.AreEqual(expected, name);
        }

        private static void CountShouldBe(int expected, int count)
        {
            Assert.AreEqual(expected, count);
        }

        private static void MoneyShouldBe(int expected, int money)
        {
            Assert.AreEqual(expected, money);
        }

        private void FileShouldExist(string filePath)
        {
            Assert.That(File.Exists(filePath));
        }

        [Serializable]
        private class TestData
        {
            public int    money;
            public int    count;
            public string name;

            public TestData(int money, int count, string name)
            {
                this.money = money;
                this.count = count;
                this.name  = name;
            }
        }
    }
}