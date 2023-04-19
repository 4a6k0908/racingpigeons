using System;
using Core.Database.Models;
using Core.User.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Database.Login
{
    public class MemberAwsUser : AwsUserBase, IGetAwsUser
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
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}