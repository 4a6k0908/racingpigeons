using System;

namespace Core.Database.Models
{
    [Serializable]
    public class UserModel
    {
        public string userID;
        public string deviceID;
        
        public string idToken;
        public string accessToken;
        public string refreshToken;
        public string provider;
    }
}