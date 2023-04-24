using System.Collections.Generic;
using Core.Aws;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.Pigeon.Models;
using Core.Save;
using Core.User.Models;
using Cysharp.Threading.Tasks;

namespace Core.Player.Models
{
    public class PlayerData
    {
        private readonly SaveSystem saveSystem;

        private readonly AwsGraphQL   awsGraphQL;
        private readonly AwsUserModel awsUserModel;

        private readonly PigeonModel pigeonModel;

        private readonly UserInfoModel   userInfoModel;
        private readonly UserWalletModel userWalletModel;

        public PlayerData(AwsGraphQL awsGraphQL)
        {
            this.awsGraphQL = awsGraphQL;

            saveSystem = new SaveSystem();

            awsUserModel = new AwsUserModel(saveSystem);
            awsUserModel.LoadCache();

            userInfoModel   = new UserInfoModel(awsGraphQL);
            userWalletModel = new UserWalletModel(awsGraphQL);

            pigeonModel = new PigeonModel(awsGraphQL);
        }

        public AwsGraphQL GetGraphQL()
        {
            return awsGraphQL;
        }

        public async UniTask Login(IGetAwsUser getAwsUser)
        {
            await awsUserModel.Login(getAwsUser, awsUserModel);
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