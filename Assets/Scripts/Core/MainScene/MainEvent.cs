using System.Collections.Generic;
using Core.Pigeon.Models;
using Core.User.Models;

namespace Core.MainScene
{
    public struct OnUserWalletUpdate
    {
        public readonly UserWalletModel userWalletModel;

        public OnUserWalletUpdate(UserWalletModel userWalletModel)
        {
            this.userWalletModel = userWalletModel;
        }
    }

    public struct OnUserInfoUpdate
    {
        public readonly UserInfoModel userInfoModel;

        public OnUserInfoUpdate(UserInfoModel userInfoModel)
        {
            this.userInfoModel = userInfoModel;
        }
    }

    public struct OnPigeonListUpdate
    {
        public readonly List<PigeonStat> pigeonStats;

        public OnPigeonListUpdate(List<PigeonStat> pigeonStats)
        {
            this.pigeonStats = pigeonStats;
        }
    }
}