using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginProc : MonoBehaviour
{
    public Button LoginSignupButton;
    public Button GuestSignupButton;
    public Button GoogleSignupButton;
    public Button LineSignupButton;
    public Button LogoutButton;

    private GameObject _loginText;
    private GameObject _loadingText;

    private AwsAuth _authManager;
    private AwsCognito _awsCognito;

   
    int _COM1 = 0; //流程



    public async void ProcessDeepLink(string deepLinkUrl)
    {
        bool exchangeSuccess = await _authManager.ExchangeAuthCodeForAccessToken(deepLinkUrl);

        if (exchangeSuccess)
        {

        }
    }


    private void onLoginClicked()
    {
        Debug.Log("onLoginClicked ");
        string loginUrl = _authManager.GetLoginUrl();

        Application.OpenURL(loginUrl);
    }

    private void onGuestLoginClicked()
    {
        if (_COM1 > 0)
            return;

        Debug.Log("onGuestLoginClicked ");
        _ = _awsCognito.GuestLogin();

        if (_loginText != null)
            _loginText.SetActive(false);
        if (_loadingText != null)
            _loadingText.SetActive(true);

        //等待guest登入 
        _COM1 = 10;
    }


    private void onGoogleLoginClicked()
    {
        if (_COM1 > 0)
            return;

        Debug.Log("onGoogleLoginClicked ");
        string loginUrl = _authManager.GetAuthUrl("Google");
        Application.OpenURL(loginUrl);

        if (_loginText != null)
            _loginText.SetActive(false);
        if (_loadingText != null)
            _loadingText.SetActive(true);

        //等待 第3方 登入
        _COM1 = 20;
    }

    private void onLineLoginClicked()
    {
        if (_COM1 > 0)
            return;

        Debug.Log("onLineLoginClicked ");
        string loginUrl = _authManager.GetAuthUrl("Line");
        Application.OpenURL(loginUrl);

        if (_loginText != null)
            _loginText.SetActive(false);
        if (_loadingText != null)
            _loadingText.SetActive(true);

        //等待 第3方 登入
        _COM1 = 20;
    }


    private void onLogoutClick()
    {
        _authManager.Logout();
    }

 
    private async void RefreshToken()
    {
        bool successfulRefresh = await _authManager.CallRefreshTokenEndpoint();
        if (successfulRefresh)
        {

        }
    }

    void Start()
    {
        RefreshToken();

        if (LoginSignupButton)
            LoginSignupButton.onClick.AddListener(onLoginClicked);

        if (GuestSignupButton)
            GuestSignupButton.onClick.AddListener(onGuestLoginClicked);

        if (GoogleSignupButton)
            GoogleSignupButton.onClick.AddListener(onGoogleLoginClicked);

        if (LineSignupButton)
            LineSignupButton.onClick.AddListener(onLineLoginClicked);

   
        if (LogoutButton)
            LogoutButton.onClick.AddListener(onLogoutClick);

    }

    void Awake()
    {
        _COM1 = 0; //流程 


        _authManager = FindObjectOfType<AwsAuth>();
        _awsCognito = FindObjectOfType<AwsCognito>();
        _loginText  = GameObject.Find("Login_Text");
        _loadingText = GameObject.Find("Loading_Text");
        if(_loginText!=null)
            _loginText.SetActive(true);
        if (_loadingText!=null)
            _loadingText.SetActive(false);

    }





    // Update is called once per frame
    void Update()
    {
        switch (_COM1)
        {
            //判斷是否巳有帳號存在
            case 0:
                if (_authManager == null || _awsCognito==null )
                    break;
                
                if (_authManager.getRefreshToken()!=null && _authManager.getRefreshToken() != "")
                {
                    if (_loginText != null)
                        _loginText.SetActive(false);
                    if (_loadingText != null)
                        _loadingText.SetActive(true);

                    //等待 第3方 登入
                    _COM1 = 20;
                    break;
                }

                if (_awsCognito.auto_login())
                {
                    if (_loginText != null)
                        _loginText.SetActive(false);
                    if (_loadingText != null)
                        _loadingText.SetActive(true);

                    //等待 guest 登入
                    _COM1 = 10;
                    break;
                }

                break;


            //等待 guest 登入
            case 10:
                if (_awsCognito.isLogin())
                {
 
                    //載入玩家資料
                    _COM1 = 100;
                }
                break;


            //等待 第3方 登入
            case 20:
                if (_authManager.isLogin())
                {
                    AwsCognito.userData.idToken = _authManager.GetIdToken();
                    AwsCognito.userData.accessToken = _authManager.GetAccessToken();
                    AwsCognito.userData.userID = _authManager.GetUserId();

                    //載入玩家資料
                    _COM1 = 100;
                }
                break;

        
            //載入玩家資料
            case 100:
                //連線取得玩家資料
                GameType.gamePlayer.Init();

                //玩家資料讀取中
                _COM1 = 110;
                break;

            //玩家資料讀取中
            case 110:

                SceneManager.LoadScene("LobbyScene");

                //載入場景中
                _COM1 = 200;
                break;


            //載入場景中
            case 200:
                //loading....
                break;


        }

    }


}
