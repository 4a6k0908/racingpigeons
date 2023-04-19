using System.Net.Http;
using Amazon;
using Core.Database.Models;
using Core.User.Models;
using Cysharp.Threading.Tasks;

namespace Core.Database.Login
{
    public interface IGetAwsUser
    {
        UniTask Execute(AwsUserModel awsUserModel);
    }

    public class LoginSystem
    {
        private readonly AwsUserModel awsUserModel;

        public LoginSystem(AwsUserModel awsUserModel)
        {
            this.awsUserModel = awsUserModel;
        }

        public async UniTask Login(IGetAwsUser getAwsUser)
        {
            try
            {
                await getAwsUser.Execute(awsUserModel);
            }
            catch (HttpRequestException e)
            {
                // TODO: Error Handler
            }
        }
    }
}