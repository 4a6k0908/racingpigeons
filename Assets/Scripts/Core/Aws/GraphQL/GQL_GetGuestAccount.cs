using System;
using Core.Aws.Models;

namespace Core.Aws.GraphQL
{
    // 呼叫訪客登入的資料結構
    [Serializable]
    public class GQL_GetGuestAccount
    {
        public GuestAccount data;
    }
}