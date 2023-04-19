using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyProc : MonoBehaviour
{
    //介面相關
    //玩家錢包
    public TMP_Text UI_balance; //儲值金(金果幣)
    public TMP_Text UI_coin;    //遊戲金幣(賽鴿幣)

    //鴿子資訊
    public TMP_Text UI_PigeonTotalQ;   //總鴿數
    public TMP_Text UI_PigeonMaleQ;   //雄鴿數
    public TMP_Text UI_PigeonFemaleQ; //母鴿數
    public TMP_Text UI_PigeonFeralQ;  //自來鴿數

    //鴿子資訊按鈕    
    public Button BTN_PigeonTotalQ;
    public Button BTN_PigeonMaleQ;
    public Button BTN_PigeonFemaleQ;
    public Button BTN_PigeonFeralQ;

    //流程
    int _COM1;



    //
    private void onPigeonTotalQClicked()
    {
        if (_COM1 > 0)
            return;
        //SceneManager.LoadScene("ViewListScene");

        //載入場景中
        _COM1 = 200;
    }

    private void onPigeonMaleQClicked()
    {
        if (_COM1 > 0)
            return;
        //SceneManager.LoadScene("ViewListScene");

        //載入場景中
        _COM1 = 200;

    }

    private void onPigeonFemaleQClicked()
    {
        if (_COM1 > 0)
            return;
        //SceneManager.LoadScene("ViewListScene");

        //載入場景中
        _COM1 = 200;

    }

    private void onPigeonFeralQClicked()
    {
        if (_COM1 > 0)
            return;
        //SceneManager.LoadScene("ViewListScene");

        //載入場景中
        _COM1 = 200;

    }


    // Start is called before the first frame update
    void Start()
    {
        _COM1 = 0;

        UI_balance.text = "";
        UI_coin.text = "";
        UI_PigeonTotalQ.text = "0";
        UI_PigeonMaleQ.text = "0";
        UI_PigeonFemaleQ.text = "0";
        UI_PigeonFeralQ.text = "0";

        if (BTN_PigeonTotalQ)
            BTN_PigeonTotalQ.onClick.AddListener(onPigeonTotalQClicked);
        if (BTN_PigeonMaleQ)
            BTN_PigeonMaleQ.onClick.AddListener(onPigeonMaleQClicked);
        if (BTN_PigeonFemaleQ)
            BTN_PigeonFemaleQ.onClick.AddListener(onPigeonFemaleQClicked);
        if (BTN_PigeonFeralQ)
            BTN_PigeonFeralQ.onClick.AddListener(onPigeonFeralQClicked);


    }
    // Update is called once per frame
    void Update()
    {
        if (GameType.gamePlayer.userWallet != null)
        {
            UI_balance.text = GameType.gamePlayer.userWallet.balance.ToString();
            UI_coin.text = GameType.gamePlayer.userWallet.coin.ToString();
        }

        if (GameType.gamePlayer.PigeonList != null)
        {
            UI_PigeonTotalQ.text = GameType.gamePlayer.PigeonListQ.ToString();
            UI_PigeonMaleQ.text = GameType.gamePlayer.PigeonMaleQ.ToString();
            UI_PigeonFemaleQ.text = GameType.gamePlayer.PigeonFemaleQ.ToString();
            UI_PigeonFeralQ.text = GameType.gamePlayer.PigeonFeralQ.ToString();
        }

        //流程
        switch (_COM1)
        {
            case 0:
                break;

            //載入場景中
            case 200:
                break;

        }

    }


}
