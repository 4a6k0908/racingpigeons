using System;
using System.Net.Http;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Core.User.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Database.Login
{
    public interface IGetAwsUser
    {
        UniTask Execute(AwsUserModel awsUserModel);
    }

    public class LoginSystem
    {
        //Cognito 新加坡
        //static string _ClientID = "2je98tckqete6fd92q6urvpg7f";
        //static string _UserPoolID = "ap-southeast-1_GWhzgdMgB";
        //static RegionEndpoint _CognitoRegion = RegionEndpoint.APSoutheast1;

        private readonly AwsUserModel awsUserModel;

        // Cognito 東京
        private string         cognitoClientKey = "6bkme5fjo0qksgd6jhmqr1e41g";
        private string         cognitoPoolKey   = "ap-northeast-1_07PSOmDNc";
        private RegionEndpoint cognitoRegion    = RegionEndpoint.APNortheast1;

        public LoginSystem(AwsUserModel awsUserModel)
        {
            this.awsUserModel = awsUserModel;
        }

        public async UniTask Login(IGetAwsUser getAwsUser)
        {
            try
            {
                await getAwsUser.Execute(awsUserModel);

                var provider = new AmazonCognitoIdentityProviderClient(null, cognitoRegion);
                var pool     = new CognitoUserPool(cognitoPoolKey, cognitoClientKey, provider);
                var user     = new CognitoUser(awsUserModel.account.username, cognitoClientKey, pool, provider);

                await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest {
                    Password = awsUserModel.account.password,
                }).ConfigureAwait(false);

                awsUserModel.idToken      = user.SessionTokens.IdToken;
                awsUserModel.accessToken  = user.SessionTokens.AccessToken;
                awsUserModel.refreshToken = user.SessionTokens.RefreshToken;

                Debug.Log($"Aws User: \n{JsonUtility.ToJson(awsUserModel)}");
            }
            catch (HttpRequestException e)
            {
                // TODO: Error Handler
            }
        }
    }
}