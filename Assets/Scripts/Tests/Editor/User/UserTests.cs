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
        }

        private AwsUserModel awsUserModel;
        private LoginSystem  loginSystem;
        private AwsGraphQL   awsGraphQL;

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
            var testAccount   = new Account("test99", "Aa+123456789");
            var memberAwsUser = new MemberAwsUser(testAccount);

            await loginSystem.Login(memberAwsUser);

            UsernameShouldBeEqual(testAccount.username, awsUserModel.account.username);
            IdTokenShouldNotEmpty();
            ProviderShouldBe("", awsUserModel.provider);
        }

        [Test]
        public async Task _03_Should_User_Get_Info_Success()
        {
            var testAccount   = new Account("test99", "Aa+123456789");
            var memberAwsUser = new MemberAwsUser(testAccount);
            var userInfoModel = new UserInfoModel(awsGraphQL);

            await loginSystem.Login(memberAwsUser);

            await userInfoModel.GetUserInfo(awsUserModel);

            NicknameShouldBe("test", userInfoModel.nickname);
            EmailShouldBe("taboi40145@gmail.com", userInfoModel.email);
        }

        private void EmailShouldBe(string expected, string compare)
        {
            Assert.AreEqual(expected, compare);
        }

        private void NicknameShouldBe(string expected, string compare)
        {
            Assert.AreEqual(expected, compare);
        }


        private void UsernameShouldBeEqual(string expected, string compare)
        {
            Assert.AreEqual(expected, compare);
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