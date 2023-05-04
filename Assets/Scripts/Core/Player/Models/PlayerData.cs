using System.Collections.Generic;
using Core.Aws;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.Pigeon.Models;
using Core.User.Models;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.Player.Models
{
    // 玩家整體資料的整合
    public class PlayerData
    {
        private readonly AwsGraphQL   awsGraphQL;   // AWS GraphQL 溝通
        private readonly AwsUserModel awsUserModel; // Aws User 資料

        private readonly PigeonModel pigeonModel; // 鴿子資料
        private readonly SignalBus   signalBus;

        private readonly UserInfoModel   userInfoModel;   // 玩家資訊
        private readonly UserWalletModel userWalletModel; // 玩家錢包

        public PlayerData(AwsGraphQL awsGraphQL)
        {
            this.awsGraphQL = awsGraphQL;

            awsUserModel = new AwsUserModel();
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