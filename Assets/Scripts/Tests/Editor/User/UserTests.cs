using System.Threading.Tasks;
using Core.Database;
using Core.Database.Login;
using Core.User.Models;
using NUnit.Framework;

namespace Tests.Editor.User
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public async Task _01_Should_Guest_Sign_In_Success()
        {
            var awsUserModel = new AwsUserModel();
            var loginSystem  = new LoginSystem(awsUserModel);

            await loginSystem.Login(new GuestGetAwsUser(new AwsGraphQL()));

            ShouldBeGuestUsername(awsUserModel);
            ProviderShouldBe("guest", awsUserModel.provider);
        }

        [Test]
        public async Task _02_Should_Member_Sign_In_Success()
        {
            var awsUserModel = new AwsUserModel();
            var loginSystem  = new LoginSystem(awsUserModel);

            await loginSystem.Login(new MemberAwsUser(new AwsGraphQL()));

            
        }

        private void ShouldBeGuestUsername(AwsUserModel awsUserModel)
        {
            Assert.AreEqual(true, awsUserModel.account.username.Contains("guest"));
        }

        private void ProviderShouldBe(string expected, string provider)
        {
            Assert.AreEqual(expected, provider);
        }
    }
}