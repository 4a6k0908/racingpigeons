using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections;

public class AwsAuth : MonoBehaviour
{
    public static string CachePath;

    //新加坡
    //private const string AppClientID = "2je98tckqete6fd92q6urvpg7f";
    //private const string Region = "ap-southeast-1";

    //東京
    private const string AppClientID = "6bkme5fjo0qksgd6jhmqr1e41g";
    private const string Region = "ap-northeast-1";

    private const string AuthCognitoDomainPrefix = "pigeon2023";
    private const string RedirectUrl = "app://com.Goldencherry.Pigeon";


    private const string AuthCodeGrantType = "authorization_code";
    private const string RefreshTokenGrantType = "refresh_token";
    private const string CognitoAuthUrl = ".auth." + Region + ".amazoncognito.com";
    private const string TokenEndpointPath = "/oauth2/token";

    private static string _userid = "";

    public static UserSessionCache _userSessionCache = new UserSessionCache();


    bool _isSignIn = false;

	void Awake()
	{
        _isSignIn = false;
        CachePath = Application.persistentDataPath;
        SaveDataManager.setPath(Application.persistentDataPath);
        SaveDataManager.LoadJsonData(_userSessionCache);

        // Debug.Log("CachePath: " + CachePath);
    }


    public async Task<bool> ExchangeAuthCodeForAccessToken(string rawUrlWithGrantCode)
    {
        // Debug.Log("rawUrlWithGrantCode: " + rawUrlWithGrantCode);

        // raw url looks like https://somedomain.com/?code=c91d8bf4-1cb6-46e5-b43a-8def466f3c55
        string allQueryParams = rawUrlWithGrantCode.Split('?')[1];

        // it's likely there won't be more than one param
        string[] paramsSplit = allQueryParams.Split('&');

        foreach (string param in paramsSplit)
        {
            // Debug.Log("param: " + param);

            // find the code parameter and its value
            if (param.StartsWith("code"))
            {
                string grantCode = param.Split('=')[1];
                string grantCodeCleaned = grantCode.removeAllNonAlphanumericCharsExceptDashes(); // sometimes the url has a # at the end of the string
                return await CallCodeExchangeEndpoint(grantCodeCleaned);
            }
            else
            {
                Debug.Log("Code not found");
            }
        }

        return false;
    }

    // exchanges grant code for tokens
    private async Task<bool> CallCodeExchangeEndpoint(string grantCode)
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", AuthCodeGrantType);
        form.AddField("client_id", AppClientID);
        form.AddField("code", grantCode);
        form.AddField("redirect_uri", RedirectUrl);

        // DOCS: https://docs.aws.amazon.com/cognito/latest/developerguide/token-endpoint.html
        string requestPath = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl + TokenEndpointPath;

        UnityWebRequest webRequest = UnityWebRequest.Post(requestPath, form);
        await webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Code exchange failed: " + webRequest.error + "\n" + webRequest.result + "\n" + webRequest.responseCode);
            webRequest.Dispose();
        }
        else
        {
            Debug.Log("Success, Code exchange complete!");

            BADAuthenticationResultType authenticationResultType = JsonUtility.FromJson<BADAuthenticationResultType>(webRequest.downloadHandler.text);
            // Debug.Log("ID token: " + authenticationResultType.id_token);

            _userid = AuthUtilities.GetUserSubFromIdToken(authenticationResultType.id_token);

            // update session cache
            SaveDataManager.SaveJsonData(new UserSessionCache(authenticationResultType, _userid));
            webRequest.Dispose();

            _isSignIn = true;
            return true;
        }
        return false;
    }

    //刷新令牌
    public async Task<bool> CallRefreshTokenEndpoint()
    {
        //SaveDataManager.LoadJsonData(userSessionCache);

        string preservedRefreshToken = "";

        if (_userSessionCache != null && _userSessionCache._refreshToken != null && _userSessionCache._refreshToken != "")
        {
            // DOCS: https://docs.aws.amazon.com/cognito/latest/developerguide/token-endpoint.html
            string refreshTokenUrl = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl + TokenEndpointPath;
            // Debug.Log(refreshTokenUrl);

            preservedRefreshToken = _userSessionCache._refreshToken;

            WWWForm form = new WWWForm();
            form.AddField("grant_type", RefreshTokenGrantType);
            form.AddField("client_id", AppClientID);
            form.AddField("refresh_token", _userSessionCache._refreshToken);

            UnityWebRequest webRequest = UnityWebRequest.Post(refreshTokenUrl, form);
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            await webRequest.SendWebRequest();


            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Refresh token call failed: " + webRequest.error + "\n" + webRequest.result + "\n" + webRequest.responseCode);
                // clear out invalid user session data to force re-authentication
                ClearUserSessionData();
                webRequest.Dispose();
            }
            else
            {
                Debug.Log("Success, Refresh token call complete!");
                // Debug.Log(webRequest.downloadHandler.text);

                BADAuthenticationResultType authenticationResultType = JsonUtility.FromJson<BADAuthenticationResultType>(webRequest.downloadHandler.text);

                // token endpoint to get refreshed access token does NOT return the refresh token, so manually save it from before.
                authenticationResultType.refresh_token = preservedRefreshToken;
                
                _userid = AuthUtilities.GetUserSubFromIdToken(authenticationResultType.id_token);


                // update session cache
                SaveDataManager.SaveJsonData(new UserSessionCache(authenticationResultType, _userid));
                webRequest.Dispose();

                _isSignIn = true;

                return true;
            }
        }
        return false;
    }


    //撤消令牌
    private async Task<bool> RevokeRefreshToken()
    {
    
        if (_userSessionCache != null && _userSessionCache._refreshToken != null && _userSessionCache._refreshToken != "")
        {
            string revokeTokenEndpoint = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl + "/oauth2/revoke";
            // Debug.Log(revokeTokenEndpoint);

            WWWForm form = new WWWForm();
            form.AddField("client_id", AppClientID);
            form.AddField("token", _userSessionCache._refreshToken);

            UnityWebRequest webRequest = UnityWebRequest.Post(revokeTokenEndpoint, form);
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Revoke token call failed: " + webRequest.error + "\n" + webRequest.result + "\n" + webRequest.responseCode);
                webRequest.Dispose();
            }
            else
            {
                Debug.Log("Success, Revoke token call complete!");
                webRequest.Dispose();
                return true;
            }
        }
        return false;
    }

    public async void Logout()
    {
        bool logoutSuccess = await RevokeRefreshToken();

        // Important! Make sure to remove the local stored tokens.
        ClearUserSessionData();
        Debug.Log("user logged out.");

        _isSignIn = false;

    }

    // Saves an empty user session object that will clear out all locally saved tokens.
    private void ClearUserSessionData()
    {
        ///UserSessionCache userSessionCache = new UserSessionCache();
        _userSessionCache.resetSessionCache();
        SaveDataManager.SaveJsonData(_userSessionCache);
    }

    public string GetUsersId()
    {
        // Debug.Log("GetUserId: [" + _userid + "]");
        if (_userid == null || _userid == "")
        {
            // load userid from cached session 
            UserSessionCache userSessionCache = new UserSessionCache();
            SaveDataManager.LoadJsonData(userSessionCache);
            _userid = userSessionCache.getUserId();
        }
        return _userid;
    }

    public string GetIdToken()
    {
        //UserSessionCache userSessionCache = new UserSessionCache();
        //SaveDataManager.LoadJsonData(userSessionCache);
        return _userSessionCache.getIdToken();
    }

    public string GetAccessToken()
    {
        return _userSessionCache.getAccessToken();
    }

    public string getRefreshToken()
    {
        return _userSessionCache.getRefreshToken();
    }    

    public string GetUserId()
    {
        //UserSessionCache userSessionCache = new UserSessionCache();
        //SaveDataManager.LoadJsonData(userSessionCache);
        return _userSessionCache.getUserId();
    }

    public string GetLoginUrl()
    {
        // DOCS: https://docs.aws.amazon.com/cognito/latest/developerguide/login-endpoint.html
        string loginUrl = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl
           + "/login?response_type=code&client_id="
           + AppClientID + "&redirect_uri=" + RedirectUrl;
        return loginUrl;
    }

    //
    public string GetAuthUrl(string idp)
    {
        // DOCS: https://docs.aws.amazon.com/cognito/latest/developerguide/login-endpoint.html
        string loginUrl = "https://" + AuthCognitoDomainPrefix + CognitoAuthUrl
            + "/authorize?response_type=code&client_id="
            + AppClientID + "&redirect_uri=" + RedirectUrl
            + "&identity_provider=" + idp
            + "&scope=email+openid+phone+profile";


        return loginUrl;
    }


    public bool isLogin()
    {
        return _isSignIn;
    }

}
