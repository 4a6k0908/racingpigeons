using System.Net.Http;
using Amazon;
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

        // Cognito 東京
        private string         cognitoClientKey = "6bkme5fjo0qksgd6jhmqr1e41g";
        private string         cognitoPoolKey   = "ap-northeast-1_07PSOmDNc";
        private RegionEndpoint cognitoRegion    = RegionEndpoint.APNortheast1;

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