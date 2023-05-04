using System;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.NotifySystem;
using Core.Player.Models;
using UnityEngine;
using Zenject;

namespace Core
{
    // Lobby 測試用，用來跳過登入
    public class ONLY_FOR_TEST : MonoBehaviour
    {
        [Inject]
        public async void Inject(PlayerData playerData, INotifyService notifyService)
        {
            try
            {
                var awsUserModel = playerData.GetAwsUserModel();

                IGetAwsUser getAwsUser = null;

                if (awsUserModel.provider == null || !awsUserModel.provider.Equals("guest"))
                {
                    getAwsUser = new GuestAwsUser(playerData.GetGraphQL());
                }
                else
                {
                    var account = new Account(awsUserModel.account.username, awsUserModel.account.password);
                    getAwsUser = new MemberAwsUser(account);
                }

                notifyService.DoNotify("登入中，請稍候");
                await playerData.Login(getAwsUser);

                notifyService.DoNotify("取得玩家資訊中");
                await playerData.SyncUserInfo();

                notifyService.DoNotify("取得玩家錢包資訊中");
                await playerData.SyncUserWallet();

                notifyService.DoNotify("取得鴿子資訊中");
                await playerData.SyncPigeonList(10);

                notifyService.DoClose();
            }
            catch (Exception e)
            {
                notifyService.DoNotify(e.Message, () => { });
            }
        }
    }
}