using System;

namespace Core.User.GraphQL
{
    [Serializable]
    public class GQL_GetUserWallet
    {
        [Serializable]
        public struct Data
        {
            public UserWallet getUserWallet;
        }

        public Data data;
    }
}