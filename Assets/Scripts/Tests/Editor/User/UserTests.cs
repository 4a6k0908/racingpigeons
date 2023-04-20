using System.Threading.Tasks;
using Core.Database;
using Core.Database.Login;
using Core.Database.Models;
using Core.Player.Models;
using Core.User.Models;
using NUnit.Framework;

namespace Tests.Editor.User
{
    [TestFixture]
    public class UserTests
    {
        [SetUp]
        public void SetUp()
        {
            awsGraphQL  = new AwsGraphQL();
            playerData  = new PlayerData(awsGraphQL);
            
            loginSystem = new LoginSystem(playerData.GetAwsUserModel());
        }

        private Account     testAccount = new("test99", "Aa+123456789");
        private AwsGraphQL  awsGraphQL;
        private PlayerData  playerData;
        private LoginSystem loginSystem;

        [Test]
        public async Task _01_Should_Guest_Sign_In_Success()
        {
            var guestGetAwsUser = new GuestGetAwsUser(awsGraphQL);
            
            await loginSystem.Login(guestGetAwsUser);

            var awsUserModel = playerData.GetAwsUserModel();
            
            UsernameShouldBeGuest(awsUserModel.account.username);
            IdTokenShouldNotEmpty(awsUserModel.idToken);
            ProviderShouldBe("guest", awsUserModel.provider);
        }

        [Test]
        public async Task _02_Should_Member_Sign_In_Success()
        {
            var memberAwsUser = new MemberAwsUser(testAccount);

            await loginSystem.Login(memberAwsUser);

            var awsUserModel = playerData.GetAwsUserModel();
            
            UsernameShouldBeEqual(testAccount.username, awsUserModel.account.username);
            IdTokenShouldNotEmpty(awsUserModel.idToken);
            ProviderShouldBe("", awsUserModel.provider);
        }

        [Test]
        public async Task _03_Should_Get_User_Info_Success()
        {
            var memberAwsUser = new MemberAwsUser(testAccount);

            await loginSystem.Login(memberAwsUser);

            await playerData.SyncUserInfo();
            
            var userInfoModel = playerData.GetUserInfoModel();

            NicknameShouldBe("test", userInfoModel.nickname);
            EmailShouldBe("taboi40145@gmail.com", userInfoModel.email);
        }

        [Test]
        public async Task _04_Should_Get_User_Wallet_Success()
        {
            var memberAwsUser   = new MemberAwsUser(testAccount);

            await loginSystem.Login(memberAwsUser);

            await playerData.SyncUserWallet();

            var userWalletModel = playerData.GetUserWalletModel();
            
            BalanceShouldBe(10000, userWalletModel.balance);
            CoinShouldBe(10000, userWalletModel.coin);
            TicketShouldBe(0, userWalletModel.ticket);
        } 

        private static void TicketShouldBe(int expected, int ticket)
        {
            Assert.AreEqual(expected, ticket);
        }

        private static void CoinShouldBe(int expected, int coin)
        {
            Assert.AreEqual(expected, coin);
        }

        private static void BalanceShouldBe(int expected, int balance)
        {
            Assert.AreEqual(expected, balance);
        }

        private void EmailShouldBe(string expected, string email)
        {
            Assert.AreEqual(expected, email);
        }

        private void NicknameShouldBe(string expected, string nickname)
        {
            Assert.AreEqual(expected, nickname);
        }


        private void UsernameShouldBeEqual(string expected, string username)
        {
            Assert.AreEqual(expected, username);
        }

        private void UsernameShouldBeGuest(string username)
        {
            Assert.AreEqual(true, username.Contains("guest"));
        }

        private void IdTokenShouldNotEmpty(string idToken)
        {
            Assert.AreNotEqual("", idToken);
        }

        private void ProviderShouldBe(string expected, string provider)
        {
            Assert.AreEqual(expected, provider);
        }
    }
}