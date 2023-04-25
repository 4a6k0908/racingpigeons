using System;
using Core.Save;
using NUnit.Framework;

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
        private string     fileName = "test.dat";

        [Test]
        public void _01_Should_Save_Data_To_Json_In_Local_Storage_Success()
        {
            var testData = new TestData(1, 5, "test");

            saveSystem.Save(testData, fileName);

            FileIsNeedExist(true, fileName);
        }

        [Test]
        public void _02_Should_Saved_Data_Exist()
        {
            FileIsNeedExist(true, fileName);
        }

        [Test]
        public void _03_Should_Load_Data_From_Local_Storage_Success_And_Correct()
        {
            var testData = saveSystem.Load<TestData>(fileName);

            MoneyShouldBeSame(1, testData.money);
            CountShouldBeSame(5, testData.count);
            NameShouldBeSame("test", testData.name);
        }

        [Test]
        public void _04_Should_Delete_File_From_Local_Storage_Success()
        {
            saveSystem.Delete(fileName);

            FileIsNeedExist(false, fileName);
        }

        private void FileIsNeedExist(bool expected, string fileName)
        {
            Assert.AreEqual(expected, saveSystem.IsExist(fileName));
        }

        private static void NameShouldBeSame(string expected, string name)
        {
            Assert.AreEqual(expected, name);
        }

        private static void CountShouldBeSame(int expected, int count)
        {
            Assert.AreEqual(expected, count);
        }

        private static void MoneyShouldBeSame(int expected, int money)
        {
            Assert.AreEqual(expected, money);
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