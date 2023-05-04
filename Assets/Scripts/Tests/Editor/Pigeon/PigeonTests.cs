using System.Threading.Tasks;
using Core.Aws;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.Player.Models;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Editor.Pigeon
{
    [TestFixture]
    public class PigeonTests
    {
        [SetUp]
        public void SetUp()
        {
            awsGraphQL = new AwsGraphQL();
            playerData = new PlayerData(awsGraphQL);
        }

        private Account    testAccount = new("test99", "Aa+123456789");
        private AwsGraphQL awsGraphQL;
        private PlayerData playerData;

        [Test]
        public async Task _01_Should_Get_User_Pigeons_Success()
        {
            await PlayerLogin();

            await playerData.SyncPigeonList(10);

            var pigeonList = playerData.GetPigeonList();
            PigeonCountShouldBeSame(3, pigeonList.Count);
            PigeonNameShouldBeSame("Jonathan Dennis", pigeonList[0].pigeon_name);
            PigeonNameShouldBeSame("Vernon Manning", pigeonList[1].pigeon_name);
        }

        private void PigeonNameShouldBeSame(string expected, string pigeonName)
        {
            Assert.AreEqual(expected, pigeonName);
        }

        private void PigeonCountShouldBeSame(int expected, int pigeonCount)
        {
            Assert.AreEqual(expected, pigeonCount);
        }

        private async UniTask PlayerLogin()
        {
            await playerData.Login(new MemberAwsUser(testAccount));
        }
    }
}