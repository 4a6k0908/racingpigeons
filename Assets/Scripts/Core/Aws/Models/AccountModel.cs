using System;

namespace Core.Aws.Models
{
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