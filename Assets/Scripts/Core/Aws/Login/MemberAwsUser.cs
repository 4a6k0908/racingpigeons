using Core.Aws.Models;
using Cysharp.Threading.Tasks;

namespace Core.Aws.Login
{
    // 會員登入的實作
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