using System;

namespace Core.Aws.Models
{
    // 登入帳密的資料
    [Serializable]
    public class Account
    {
        public string username;
        public string password;

        public Account(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    [Serializable]
    public class GuestAccount
    {
        public Account getGuestAccount;
    }
}