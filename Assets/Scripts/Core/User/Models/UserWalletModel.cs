using System;
using Core.Aws;
using Core.Aws.Models;
using Core.MainScene;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.User.Models
{
    [Serializable]
    public class UserWalletModel
    {
        public           int        balance;    //儲值金(金果幣)
        public           int        coin;       //遊戲金幣(賽鴿幣)
        public           int        ticket;     //票
        private readonly AwsGraphQL awsGraphQL; // 呼叫 GraphQL
        private readonly SignalBus  signalBus;

        public UserWalletModel(SignalBus signalBus, AwsGraphQL awsGraphQL)
        {
            this.signalBus  = signalBus;
            this.awsGraphQL = awsGraphQL;
        }

        // 取得玩家錢包資訊
        public async UniTask GetWalletInfo(AwsUserModel awsUserModel)
        {
            var query = "{\"query\":\""      +
                        "query MyQuery {\\n" +
                        "getUserWallet {\\n" +
                        "balance\\n"         +
                        "coin\\n"            +
                        "ticket\\n"          +
                        "}\\n"               +
                        "}"                  +
                        "\",\"variables\":{}}";

            var responseContent = await awsGraphQL.Post(query, awsUserModel.accessToken);
            var data            = JsonUtility.FromJson<GQL_GetUserWallet>(responseContent);

            if (data == null)
                throw new Exception("無法獲取錢包資料");

            Debug.Log($"User Wallet: \n {JsonUtility.ToJson(data.data.getUserWallet)}");

            var userWallet = data.data.getUserWallet;
            balance = userWallet.balance;
            coin    = userWallet.coin;
            ticket  = userWallet.ticket;

            signalBus.Fire(new OnUserWalletUpdate(this));
        }


        // 用於取得玩家錢包的結構
        [Serializable]
        public class GQL_GetUserWallet
        {
            public Data data;

            [Serializable]
            public struct Data
            {
                public UserWalletModel getUserWallet;
            }
        }
    }
}