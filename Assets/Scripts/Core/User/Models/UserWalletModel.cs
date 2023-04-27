using System;
using Core.Aws;
using Core.Aws.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.User.Models
{
    [Serializable]
    public class UserWalletModel
    {
        private readonly AwsGraphQL awsGraphQL; // 呼叫 GraphQL

        public int balance; //儲值金(金果幣)
        public int coin;    //遊戲金幣(賽鴿幣)
        public int ticket;  //票

        public UserWalletModel(AwsGraphQL awsGraphQL)
        {
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
        }


        // 用於取得玩家錢包的結構
        [Serializable]
        public class GQL_GetUserWallet
        {
            [Serializable]
            public struct Data
            {
                public UserWalletModel getUserWallet;
            }

            public Data data;
        }
    }
}