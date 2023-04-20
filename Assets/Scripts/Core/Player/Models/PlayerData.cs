using Core.Database;
using Core.Database.Models;
using Core.User.Models;
using Cysharp.Threading.Tasks;

namespace Core.Player.Models
{
    public class PlayerData
    {
        private readonly AwsGraphQL   awsGraphQL;
        private readonly AwsUserModel awsUserModel;

        private readonly UserInfoModel   userInfoModel;
        private readonly UserWalletModel userWalletModel;

        public PlayerData(AwsGraphQL awsGraphQL)
        {
            this.awsGraphQL = awsGraphQL;

            awsUserModel = new AwsUserModel();

            userInfoModel   = new UserInfoModel(awsGraphQL);
            userWalletModel = new UserWalletModel(awsGraphQL);
        }

        public AwsUserModel GetAwsUserModel()
        {
            return awsUserModel;
        }

        public UserInfoModel GetUserInfoModel()
        {
            return userInfoModel;
        }

        public async UniTask<bool> SyncUserInfo()
        {
            return await userInfoModel.GetUserInfo(awsUserModel);
        }

        public UserWalletModel GetUserWalletModel()
        {
            return userWalletModel;
        }

        public async UniTask<bool> SyncUserWallet()
        {
            return await userWalletModel.GetWalletInfo(awsUserModel);
        }
    }
}