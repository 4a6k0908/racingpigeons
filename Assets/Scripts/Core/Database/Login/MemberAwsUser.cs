using System.Net.Http;
using Core.User.Models;
using Cysharp.Threading.Tasks;

namespace Core.Database.Login
{
    public class MemberAwsUser : IGetAwsUser
    {
        private readonly AwsGraphQL awsGraphQL;

        public MemberAwsUser(AwsGraphQL awsGraphQl)
        {
            awsGraphQL = awsGraphQl;
        }

        public async UniTask Execute(AwsUserModel awsUserModel)
        {
            try
            {
                
            }
            catch (HttpRequestException e)
            {
                // TODO: Error Handler
            }
        }
    }
}