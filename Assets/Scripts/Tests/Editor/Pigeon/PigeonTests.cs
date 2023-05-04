using System.Threading.Tasks;
using Core.Aws;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.MainScene;
using Core.Player.Models;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Zenject;

namespace Tests.Editor.Pigeon
{
    [TestFixture]
    public class PigeonTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<OnPigeonListUpdate>();

            signalBus = Container.Resolve<SignalBus>();

            awsGraphQL = new AwsGraphQL();
            playerData = new PlayerData(signalBus, awsGraphQL);
        }

        private Account    testAccount = new("test99", "Aa+123456789");
        private AwsGraphQL awsGraphQL;
        private PlayerData playerData;
        private SignalBus  signalBus;

        [Test]
        public async Task _01_Should_Get_User_Pigeons_Success()
        {
            await PlayerLogin();
            var eventCount = 0;
            signalBus.Subscribe<OnPigeonListUpdate>(_ => eventCount++);

            await playerData.SyncPigeonList(10);

            var pigeonList = playerData.GetPigeonList();

            ShouldReceiveEventOnce(eventCount);
            PigeonCountShouldBeSame(3, pigeonList.Count);
            PigeonNameShouldBeSame("Jonathan Dennis", pigeonList[0].pigeon_name);
            PigeonNameShouldBeSame("Vernon Manning", pigeonList[1].pigeon_name);
        }

        private static void ShouldReceiveEventOnce(int eventCount)
        {
            Assert.AreEqual(1, eventCount);
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