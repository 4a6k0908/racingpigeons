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
    public class AwsUserBase
    {
        private string         cognitoClientKey = "6bkme5fjo0qksgd6jhmqr1e41g";
        private string         cognitoPoolKey   = "ap-northeast-1_07PSOmDNc";
        private RegionEndpoint cognitoRegion    = RegionEndpoint.APNortheast1;

        protected async UniTask SetUserToken(AwsUserModel awsUserModel)
        {
            try
            {
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}