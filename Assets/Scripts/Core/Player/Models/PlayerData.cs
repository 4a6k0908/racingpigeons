using System.Collections.Generic;
using Core.Database;
using Core.Database.Models;
using Core.Pigeon.Models;
using Core.User.Models;
using Cysharp.Threading.Tasks;

namespace Core.Player.Models
{
    public class PlayerData
    {
        private readonly AwsGraphQL   awsGraphQL;
        private readonly AwsUserModel awsUserModel;
        private readonly PigeonModel  pigeonModel;

        private readonly UserInfoModel   userInfoModel;
        private readonly UserWalletModel userWalletModel;

        public PlayerData(AwsGraphQL awsGraphQL)
        {
            this.awsGraphQL = awsGraphQL;

            awsUserModel = new AwsUserModel();

            userInfoModel   = new UserInfoModel(awsGraphQL);
            userWalletModel = new UserWalletModel(awsGraphQL);

            pigeonModel = new PigeonModel(awsGraphQL);
        }

        public AwsGraphQL GetGraphQL()
        {
            return awsGraphQL;
        }

        public AwsUserModel GetAwsUserModel()
        {
            return awsUserModel;
        }

        public UserInfoModel GetUserInfoModel()
        {
            return userInfoModel;
        }

        public async UniTask SyncUserInfo()
        {
            await userInfoModel.GetUserInfo(awsUserModel);
        }

        public UserWalletModel GetUserWalletModel()
        {
            return userWalletModel;
        }

        public async UniTask SyncUserWallet()
        {
            await userWalletModel.GetWalletInfo(awsUserModel);
        }

        public List<PigeonStat> GetPigeonList()
        {
            return pigeonModel.GetPigeonStatsList();
        }

        public async UniTask SyncPigeonList(int queryCount)
        {
            await pigeonModel.GetPigeonList(queryCount, awsUserModel);
        }
    }
}