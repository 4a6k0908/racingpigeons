using System;
using Core.User.Models;

namespace Core.User.GraphQL
{
    [Serializable]
    public class GQL_GetUserInfo
    {
        [Serializable]
        public struct Data
        {
            public UserInfoModel getUserInfo;
        }

        public Data data;
    }
}