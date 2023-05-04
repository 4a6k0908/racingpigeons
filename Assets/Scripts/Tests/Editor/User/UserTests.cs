using System.Threading.Tasks;
using Core.Aws;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.Player.Models;
using Core.Save;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Editor.User
{
    [TestFixture]
    public class UserTests
    {
        [SetUp]
        public void SetUp()
        {
            awsGraphQL = new AwsGraphQL();
            playerData = new PlayerData(awsGraphQL);
        }

        private SaveSystem saveSystem;
        private Account    testAccount = new("test99", "Aa+123456789");
        private AwsGraphQL awsGraphQL;
        private PlayerData playerData;

        [Test]
        public async Task _01_Should_Guest_Sign_In_Success()
        {
            var guestGetAwsUser = new GuestAwsUser(awsGraphQL);
            await PlayerLogin(guestGetAwsUser);

            var awsUserModel = playerData.GetAwsUserModel();

            UsernameShouldContainGuest(awsUserModel.account.username);
            IdTokenShouldNotEmpty();
            ProviderShouldBe("guest");
        }

        [Test]
        public async Task _02_Should_Member_Sign_In_Success()
        {
            var memberAwsUser = new MemberAwsUser(testAccount);

            await PlayerLogin(memberAwsUser);

            UsernameShouldBe(testAccount.username);
            IdTokenShouldNotEmpty();
        }

        [Test]
        public async Task _03_Should_Get_User_Info_Success()
        {
            var memberAwsUser = new MemberAwsUser(testAccount);
            await PlayerLogin(memberAwsUser);

            await playerData.SyncUserInfo();

            NicknameShouldBe("test");
            EmailShouldBe("taboi40145@gmail.com");
        }

        [Test]
        public async Task _04_Should_Get_User_Wallet_Success()
        {
            var memberAwsUser = new MemberAwsUser(testAccount);
            await PlayerLogin(memberAwsUser);

            await playerData.SyncUserWallet();

            BalanceShouldBe(9997);
            CoinShouldBe(10000);
            TicketShouldBe(0);
        }

        private async UniTask PlayerLogin(IGetAwsUser getAwsUser)
        {
            await playerData.Login(getAwsUser);
        }

        private void TicketShouldBe(int expected)
        {
            var wallet = playerData.GetUserWalletModel();

            Assert.AreEqual(expected, wallet.ticket);
        }

        private void CoinShouldBe(int expected)
        {
            var wallet = playerData.GetUserWalletModel();

            Assert.AreEqual(expected, wallet.coin);
        }

        private void BalanceShouldBe(int expected)
        {
            var wallet = playerData.GetUserWalletModel();

            Assert.AreEqual(expected, wallet.balance);
        }

        private void EmailShouldBe(string expected)
        {
            var info = playerData.GetUserInfoModel();

            Assert.AreEqual(expected, info.email);
        }

        private void NicknameShouldBe(string expected)
        {
            var info = playerData.GetUserInfoModel();

            Assert.AreEqual(expected, info.nickname);
        }

        private void UsernameShouldBe(string expected)
        {
            var awsUser = playerData.GetAwsUserModel();

            Assert.AreEqual(expected, awsUser.account.username);
        }

        private void UsernameShouldContainGuest(string username)
        {
            Assert.AreEqual(true, username.Contains("guest"));
        }

        private void IdTokenShouldNotEmpty()
        {
            var awsUser = playerData.GetAwsUserModel();

            Assert.AreNotEqual("", awsUser.idToken);
        }

        private void ProviderShouldBe(string expected)
        {
            var awsUser = playerData.GetAwsUserModel();

            Assert.AreEqual(expected, awsUser.provider);
        }
    }
}