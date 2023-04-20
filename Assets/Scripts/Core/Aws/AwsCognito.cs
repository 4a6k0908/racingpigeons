using System;
using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Amazon.Extensions.CognitoAuthentication;
using UnityEngine.Networking;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core.Aws;
using Core.Utils.Algorithm;
using UnityEngine.SceneManagement;

public class AwsCognito : MonoBehaviour
{
    //Cognito 新加坡
    //static string _ClientID = "2je98tckqete6fd92q6urvpg7f";
    //static string _UserPoolID = "ap-southeast-1_GWhzgdMgB";
    //static RegionEndpoint _CognitoRegion = RegionEndpoint.APSoutheast1;

    //Cognito 東京
    static string _ClientID = "6bkme5fjo0qksgd6jhmqr1e41g";
    static string _UserPoolID = "ap-northeast-1_07PSOmDNc";
    static RegionEndpoint _CognitoRegion = RegionEndpoint.APNortheast1;

    string mToken = null;
    
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    public bool _isSignIn;      //是否登錄 

    //----------------------------------------------------------------
    //guest 登入資訊
    [Serializable]
    public class Account
    {
        public string password;
        public string username;
    }

    [Serializable]
    public class GuestAccount
    {
        public Account getGuestAccount;
    }

    [Serializable]
    public class GQL_getGuestAccount
    {
        public GuestAccount data;
    }

    

    //----------------------------------------------------------------
    //用戶資料
    [Serializable]
    public class UserData : ISaveable
    {
        public string  deviceID;     //用戶唯一編碼(創號用) 
        public Account account;      //aws 登入帳號,密碼
        //
        public string  idToken;      //
        public string  userID;       //
        
        public string  accessToken;  //連線aws api 用
        public string  refreshToken;
        public string  provider;     //guest(遊客) ,google,line
                                     //此參數不為guest或空白,代表巳綁定


        //ISaveable interface
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
            return "user.dat";
        }
    }

    public static UserData userData = new UserData();

    private void Awake()
    {
        SaveDataManager.setPath(Application.persistentDataPath);
        SaveDataManager.LoadJsonData(userData);
        _isSignIn = false;
    }
    
    void Start()
    {
     

    }

    void Update()
    {
    }

    public bool isLogin()
    {
        return _isSignIn;
    }

    //登入
    public async void SignIn(UserData userData)
    {
        _isSignIn = false;

        var provider = new AmazonCognitoIdentityProviderClient(null, _CognitoRegion);
        CognitoUserPool userPool = new CognitoUserPool(
            _UserPoolID,
            _ClientID,
            provider
        );
        CognitoUser user = new CognitoUser(
            userData.account.username,
            _ClientID,
            userPool,
            provider
        );

        AuthFlowResponse context = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest()
        {
            Password = userData.account.password
        }).ConfigureAwait(false);

        userData.idToken = user.SessionTokens.IdToken;
        userData.accessToken = user.SessionTokens.AccessToken;
        userData.refreshToken = user.SessionTokens.RefreshToken;
        

        Debug.Log("DeviceID :" + userData.deviceID);
        Debug.Log("UserName : " + userData.account.username);
        Debug.Log("Password : " + userData.account.password);
        Debug.Log("IdToken : " + userData.idToken);
        Debug.Log("AccessToken : " + userData.accessToken);
        Debug.Log("RefreshToken : " + userData.refreshToken);


        mToken = user.SessionTokens.IdToken;
        if (mToken != null)
        {
            SaveDataManager.SaveJsonData(userData);

            _isSignIn = true;
        }
    }


    //建立用戶
    public void SignUp()
    {
        Debug.Log("Start SignUp");

        var client = new AmazonCognitoIdentityProviderClient(null, Amazon.RegionEndpoint.APSoutheast1);
        var sr = new SignUpRequest
        {
            ClientId = _ClientID,
            Username = emailField.text,
            Password = passwordField.text,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType
                {
                    Name = "email",
                    Value = emailField.text
                }
            }
        };

        try
        {
            var result = client.SignUpAsync(sr);
            Debug.Log(result);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }



    //登入
    public async void SignIn()
    {
        _isSignIn = false;
        var provider = new AmazonCognitoIdentityProviderClient(null, RegionEndpoint.APSoutheast1);
        CognitoUserPool userPool = new CognitoUserPool(
            _UserPoolID,
            _ClientID,
            provider
        );
        CognitoUser user = new CognitoUser(
            emailField.text,
            _ClientID,
            userPool,
            provider
        );

        AuthFlowResponse context = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest()
        {
            Password = passwordField.text
        }).ConfigureAwait(false);

        Debug.Log("Token : "+user.SessionTokens.IdToken);

        mToken = user.SessionTokens.IdToken;
        if(mToken!=null)
            _isSignIn = true;
    }


    //遊客登錄
    public async Task GuestLogin()
    {
        //判斷帳號是否存在
        //SaveDataManager.LoadJsonData(userData);
        if (userData.provider != null && userData.provider.Equals("guest"))
        {
            SignIn(userData);
            return;
        }

        //使用雪花演算法,算出唯一ID
        Snowflake snowFake = new Snowflake(1L, 1L);
        var id = snowFake.GetSnowId();
        Debug.Log("Device ID: " + id);
        userData.deviceID = "" + id;

        var acc = await get_guest_account(userData.deviceID);
        if (acc != null)
        {
            //儲存帳密
            if (acc.username != null && acc.username!="")
            {
                userData.account   = acc;
                userData.provider  = "guest";
                userData.idToken   = "";
                SaveDataManager.SaveJsonData(userData);

                SignIn(userData);
            }

        }
    }


    //取得遊客帳號
    public async Task<Account> get_guest_account(string deviceID)
    {
   
        var query = "{\"query\":\"" +
                    "query MyQuery {\\n" +
                    "getGuestAccount(device_id: "+deviceID+")" + "{\\n" +
                    "password\\n" +
                    "username\\n" +
                    "}\\n" +
                    "}" +
                    "\",\"variables\":{}}";

        AwsGraphQL gql = new AwsGraphQL();
        string json = await gql.Post(query, null);

        GQL_getGuestAccount data = JsonUtility.FromJson<GQL_getGuestAccount>(json);
        if (data == null)
            return null;

        return data.data.getGuestAccount;
        
    }

    //取得目前用戶資訊
    public static UserData get_user_data()
    {
        return userData;
    }

    public bool auto_login()
    {
        if (userData.provider != null && userData.provider.Equals("guest"))
        {
            SignIn(userData);
            return true;
        }

        return false;
    }

}
