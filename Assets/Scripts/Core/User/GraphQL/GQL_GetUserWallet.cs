using System;
using Core.User.Models;

namespace Core.User.GraphQL
{
    // 用於取得玩家錢包的 class
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