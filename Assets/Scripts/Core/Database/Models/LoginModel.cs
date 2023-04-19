using System;

namespace Core.Database.Models
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

    [Serializable]
    public class GQL_GetGuestAccount
    {
        public GuestAccount data;
    }
}