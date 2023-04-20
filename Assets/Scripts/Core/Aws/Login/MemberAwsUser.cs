using System;
using Core.Aws.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Aws.Login
{
    public class MemberAwsUser : GetAwsUserBase, IGetAwsUser
    {
        private readonly Account account;

        public MemberAwsUser(Account account)
        {
            this.account = account;
        }

        public async UniTask Execute(AwsUserModel awsUserModel)
        {
            try
            {
                awsUserModel.account = account;

                await SetUserToken(awsUserModel);
                
                // TODO: Save To Local
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}