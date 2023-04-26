using System;
using Core.User.Models;

namespace Core.User.GraphQL
{
    // 用於取得玩家資訊的 class
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