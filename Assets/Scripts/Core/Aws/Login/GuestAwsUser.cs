﻿using System;
using Core.Aws.GraphQL;
using Core.Aws.Models;
using Core.Utils.Algorithm;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Aws.Login
{
    public class GuestAwsUser : GetAwsUserBase, IGetAwsUser
    {
        private readonly AwsGraphQL awsGraphQL;

        public GuestAwsUser(AwsGraphQL awsGraphQl)
        {
            awsGraphQL = awsGraphQl;
        }

        public async UniTask Execute(AwsUserModel awsUserModel)
        {
            var snowflake = new Snowflake(1L, 1L);

            var snowId = snowflake.GetSnowId();

            Debug.Log($"Device ID: {snowId}");

            awsUserModel.deviceID = snowId.ToString();

            var query = "{\"query\":\""               +
                        "query MyQuery {\\n"          +
                        "getGuestAccount(device_id: " + awsUserModel.deviceID + ")" + "{\\n" +
                        "password\\n"                 +
                        "username\\n"                 +
                        "}\\n"                        +
                        "}"                           +
                        "\",\"variables\":{}}";

            var responseContent = await awsGraphQL.Post(query, null);

            var data = JsonUtility.FromJson<GQL_GetGuestAccount>(responseContent);

            var account = data.data.getGuestAccount;

            if (account.username != null && !string.IsNullOrEmpty(account.username))
            {
                awsUserModel.account  = account;
                awsUserModel.provider = "guest";
                awsUserModel.idToken  = "";

                // TODO: Save Local
            }

            await SetUserToken(awsUserModel);
        }
    }
}