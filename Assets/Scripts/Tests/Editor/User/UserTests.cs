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
        }

        private AwsUserModel awsUserModel;
        private LoginSystem  loginSystem;

        [Test]
        public async Task _01_Should_Guest_Sign_In_Success()
        {
            await loginSystem.Login(new GuestGetAwsUser(new AwsGraphQL()));

            UsernameShouldBeGuest(awsUserModel.account.username);
            IdTokenShouldNotEmpty();
            ProviderShouldBe("guest", awsUserModel.provider);
        }

        [Test]
        public async Task _02_Should_Member_Sign_In_Success()
        {
            var testAccount = new Account("test99", "Aa+123456789");

            await loginSystem.Login(new MemberAwsUser(testAccount));

            UsernameShouldBeEqual(testAccount.username, awsUserModel.account.username);
            IdTokenShouldNotEmpty();
            ProviderShouldBe("", awsUserModel.provider);
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