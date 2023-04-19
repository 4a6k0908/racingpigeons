using System.Threading.Tasks;
using Core.Database;
using Core.Database.Login;
using Core.Database.Models;
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
            awsUserModel = new AwsUserModel();
            loginSystem  = new LoginSystem(awsUserModel);
            awsGraphQL   = new AwsGraphQL();
            
            testAccount = new Account("test99", "Aa+123456789");
        }

        private AwsUserModel awsUserModel;
        private LoginSystem  loginSystem;
        private AwsGraphQL   awsGraphQL;
        private Account      testAccount;

        [Test]
        public async Task _01_Should_Guest_Sign_In_Success()
        {
            var guestGetAwsUser = new GuestGetAwsUser(awsGraphQL);
            await loginSystem.Login(guestGetAwsUser);

            UsernameShouldBeGuest(awsUserModel.account.username);
            IdTokenShouldNotEmpty();
            ProviderShouldBe("guest", awsUserModel.provider);
        }

        [Test]
        public async Task _02_Should_Member_Sign_In_Success()
        {
            var memberAwsUser = new MemberAwsUser(testAccount);

            await loginSystem.Login(memberAwsUser);

            UsernameShouldBeEqual(testAccount.username, awsUserModel.account.username);
            IdTokenShouldNotEmpty();
            ProviderShouldBe("", awsUserModel.provider);
        }

        [Test]
        public async Task _03_Should_Get_User_Info_Success()
        {
            var memberAwsUser = new MemberAwsUser(testAccount);
            var userInfoModel = new UserInfoModel(awsGraphQL);

            await loginSystem.Login(memberAwsUser);

            await userInfoModel.GetUserInfo(awsUserModel);

            NicknameShouldBe("test", userInfoModel.nickname);
            EmailShouldBe("taboi40145@gmail.com", userInfoModel.email);
        }

        [Test]
        public async Task _04_Should_Get_User_Wallet_Success()
        {
            var memberAwsUser   = new MemberAwsUser(testAccount);
            var userWalletModel = new UserWalletModel(awsGraphQL);

            await loginSystem.Login(memberAwsUser);

            await userWalletModel.GetWalletInfo(awsUserModel);

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

        private void IdTokenShouldNotEmpty()
        {
            Assert.AreNotEqual("", awsUserModel.idToken);
        }

        private void ProviderShouldBe(string expected, string provider)
        {
            Assert.AreEqual(expected, provider);
        }
    }
}