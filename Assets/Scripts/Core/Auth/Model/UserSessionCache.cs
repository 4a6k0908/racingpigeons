using UnityEngine;

[System.Serializable]
public class UserSessionCache : ISaveable
{
   public string _idToken;
   public string _refreshToken;
   public string _accessToken;  //第3方令牌
   public string _userId;
   

   public UserSessionCache() { }

   public UserSessionCache(BADAuthenticationResultType authenticationResultType, string userId)
   {
      _idToken = authenticationResultType.id_token;
      _refreshToken = authenticationResultType.refresh_token;
      _userId = userId;
   }

    public void resetSessionCache()
    {
       _idToken ="";
       _refreshToken = "";
       _accessToken = "";  //第3方令牌
       _userId = "";

    }

   public string getIdToken()
   {
      return _idToken;
   }

   public string getRefreshToken()
   {
      return _refreshToken;
   }

   public string getAccessToken()
   {
      return _accessToken;
   }

   

    public string getUserId()
   {
      return _userId;
   }

   public string ToJson()
   {
      return JsonUtility.ToJson(this);
   }

   public void LoadFromJson(string jsonToLoadFrom)
   {
      JsonUtility.FromJsonOverwrite(jsonToLoadFrom, this);
   }

   public string FileNameToUseForData()
   {
      return "auth_cache.dat";
   }
}
