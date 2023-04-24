using Core.Aws.Models;
using Cysharp.Threading.Tasks;

namespace Core.Aws.Login
{
    public class MemberAwsUser : GetAwsUserBase, IGetAwsUser
    {
        private readonly Account account;

        public MemberAwsUser(Account account)
        {
            this.account = account;
        }

        public async UniTask Execute(AwsUserModel awsUserModel)
        {
            awsUserModel.account = account;

            await SetUserToken(awsUserModel);
        }
    }
}