using Core.Database.Models;
using Core.Database.Utils;
using Core.User.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Database.Login
{
    public class GuestGetAwsUser : AwsUserBase, IGetAwsUser
    {
        private readonly AwsGraphQL awsGraphQL;

        public GuestGetAwsUser(AwsGraphQL awsGraphQl)
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

            // TODO: Fail Condition
            // if(data == null)
            
            var account = data.data.getGuestAccount;

            if (account.username != null && !string.IsNullOrEmpty(account.username))
            {
                awsUserModel.account  = account;
                awsUserModel.provider = "guest";
                awsUserModel.idToken  = "";

                // TODO: Save Local
            }
            
            SetUserToken(awsUserModel);
        }
    }
}