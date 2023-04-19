using System;

namespace Core.User.GraphQL
{
    [Serializable]
    public class GQL_GetUserInfo
    {
        [Serializable]
        public struct Data
        {
            public UserInfo getUserInfo;
        }

        public Data data;
    }
}