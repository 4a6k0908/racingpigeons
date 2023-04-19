﻿using System;
using Core.Database;
using Core.Database.Models;
using Core.User.GraphQL;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.User.Models
{
    [Serializable]
    public class UserWalletModel
    {
        private readonly AwsGraphQL awsGraphQL;

        public int balance; //儲值金(金果幣)
        public int coin;    //遊戲金幣(賽鴿幣)
        public int ticket;  //票

        public UserWalletModel(AwsGraphQL awsGraphQL)
        {
            this.awsGraphQL = awsGraphQL;
        }

        public async UniTask<bool> GetWalletInfo(AwsUserModel awsUserModel)
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

            try
            {
                var responseContent = await awsGraphQL.Post(query, awsUserModel.accessToken);
                var data            = JsonUtility.FromJson<GQL_GetUserWallet>(responseContent);

                if (data == null)
                    return false;

                Debug.Log($"User Wallet: \n {JsonUtility.ToJson(data.data.getUserWallet)}");
                
                var userWallet = data.data.getUserWallet;
                balance = userWallet.balance;
                coin    = userWallet.coin;
                ticket  = userWallet.ticket;

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}