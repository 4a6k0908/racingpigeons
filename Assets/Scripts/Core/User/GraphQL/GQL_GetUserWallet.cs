using System;
using Core.User.Models;

namespace Core.User.GraphQL
{
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