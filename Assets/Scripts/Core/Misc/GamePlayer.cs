using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Database;
using UnityEngine;

//------------------------------------------------------------------------------
//使用者資訊
//------------------------------------------------------------------------------
[Serializable]
public class UserInfo
{
    public string address;     //地址
    public string avatar_url;
    public string birthdate;   //生日
    public string default_pigeonary_id; //默認鴿舍ID(暫無)
    public string email;       //Email
    public int    gender;      //性別
    public string identity_id; //身分證字號
    public string name;        //姓名
    public string nickname;    //玩家暱稱
    public string phone_number;//手機號碼
    public string user_code;   //使用者代碼
    public string username;    //帳號
}

[Serializable]
public class GQL_getUserInfo
{
    [Serializable]
    public class Data
    {
        
        public UserInfo getUserInfo;

    }
    public Data data;

}


//------------------------------------------------------------------------------
//使用者錢包
//------------------------------------------------------------------------------
[Serializable]
public class UserWallet
{
    public int balance; //儲值金(金果幣)
    public int coin;    //遊戲金幣(賽鴿幣)
    public int ticket;  //票
}

[Serializable]
public class GQL_getUserWallet
{
    [Serializable]
    public class Data
    {
        public UserWallet getUserWallet;
    }
    public Data data;
}



//------------------------------------------------------------------------------
//玩家資料
//------------------------------------------------------------------------------
public class GamePlayer
{
    public static int _TYPE_NOR     = 0; //所有鴿子
    public static int _TYPE_MALE    = 1; //公鴿
    public static int _TYPE_FEMALE  = 2; //母鴿
    public static int _TYPE_FERAL   = 3; //自來鴿


    bool isInit; //是否初始化過

    public string NickName; //玩家匿稱
    public int Lv;  //等級
    public int Exp; //經驗

    //
    public int MemberVer; //會員條款版本（>0 代表巳同意過)

    //使用者資訊
    public UserInfo userInfo;

    //使用者錢包
    public UserWallet userWallet;

    //我的鴿舍
    public int PigeonListQ;   //總鴿數
    public int PigeonMaleQ;   //雄鴿數
    public int PigeonFemaleQ; //母鴿數
    public int PigeonFeralQ;  //自來鴿數
   
    //鴿子資料
    public List<PigeonStat> PigeonList;
    //public PigeonStat[] PigeonList;


    //取得使用者資訊
    public async Task<bool> get_user_info()
    {
        var query = "{\"query\":\"" +
                    "query MyQuery {\\n" +
                    "getUserInfo {\\n" +

                    "address\\n" +
                    "avatar_url\\n" +
                    "birthdate\\n" +
                    "default_pigeonary_id\\n" +
                    "email\\n" +
                    "gender\\n" +
                    "identity_id\\n" +
                    "name\\n" +
                    "nickname\\n" +
                    "phone_number\\n" +
                    "user_code\\n" +
                    "username\\n" +

                    "}\\n" +
                    "}" +
                    "\",\"variables\":{}}";

        AwsGraphQL gql = new AwsGraphQL();
        string json = await gql.Post(query, AwsCognito.userData.accessToken);

        var data = JsonUtility.FromJson<GQL_getUserInfo>(json);
        if (data != null)
        {
            userInfo = data.data.getUserInfo;
            return true;
        }
        return false;
    }


    //取得使用者錢包
    public async Task<bool> get_user_wallet()
    {
        var query = "{\"query\":\"" +
                    "query MyQuery {\\n" +
                    "getUserWallet {\\n" +

                    "balance\\n" +
                    "coin\\n" +
                    "ticket\\n" +

                    "}\\n" +
                    "}" +
                    "\",\"variables\":{}}";

        AwsGraphQL gql = new AwsGraphQL();
        string json = await gql.Post(query, AwsCognito.userData.accessToken);

        var data = JsonUtility.FromJson<GQL_getUserWallet>(json);
        if (data != null)
        {
            userWallet = data.data.getUserWallet;
            return true;
        }
        return false;
    }

    public async Task<bool> get_pigeon_list(int takeQ)
    {

        var query = "{\"query\":\"" +
                    "query MyQuery {\\n" +
                    "getPigeonList(page: {take: " + takeQ + "})" + "{\\n" +
                      "items {\\n" +   

                          "age\\n" +
                          "age_feature\\n" +
                          "breed_id\\n" +
                          "breed_name\\n" +
                          
                          "constitution\\n" +
                          "exp\\n" +

                          "father_pigeon_id\\n" +
                          "fatigue\\n" +
                          "feather_quality\\n" +
                          "feather_size\\n" +
                          "feature_a\\n" +
                          "feature_b\\n" +
                          "feature_c\\n" +
                          "free_points\\n" +

                          "gender\\n" +
                          "grand_father_pigeon_id\\n" +
                          "grand_mother_pigeon_id\\n" +
                          "iq\\n" +
                          "level\\n" +

                          "max_constitution\\n" +
                          "max_feather_quality\\n" +
                          "max_feather_size\\n" +
                          "max_iq\\n" +
                          "max_muscle\\n" +
                          "max_speed\\n" +
                          "max_vision\\n" +
                          "max_vitality\\n" +
                          "mother_pigeon_id\\n" +
                          "muscle\\n" +
                          
                          "pigeon_id\\n" +
                          "pigeon_name\\n" +
                          "pigeon_pigeonry\\n" +
                          "speed\\n" +
                          "vision\\n" +
                          "vitality\\n" +
                          
                      "}\\n" +

                    "next_cursor\\n" +

                    "}\\n" +
                    "}" +

                    "\",\"variables\":{}}";

        AwsGraphQL gql = new AwsGraphQL();
        string json = await gql.Post(query, AwsCognito.userData.accessToken);

        var data = JsonUtility.FromJson<GQL_getPigeonList>(json);
        if (data != null)
        {
            PigeonList = data.data.getPigeonList.items;

            PigeonListQ = data.data.getPigeonList.items.Count;
            PigeonMaleQ = compute_pigeon_Q(_TYPE_MALE);
            PigeonFemaleQ = compute_pigeon_Q(_TYPE_FEMALE);

            return true;
        }
        return false;
    }


    //計算所有鴿子數量
    int compute_pigeon_Q(int type)
    {
        //全部鴿子
        if (type == _TYPE_NOR)
            return PigeonListQ;
        //自來鴿(暫未處理)
        else if (type == _TYPE_FERAL)
            return 0;

        int q = 0;
        for (int i = 0; i < PigeonListQ; i++)
        {

            if (type == _TYPE_MALE)
            {
                if (PigeonList[i].gender == 0)
                    q++;
            }
            else if (type == _TYPE_FEMALE)
            {
                if (PigeonList[i].gender == 1)
                    q++;
            }
        }

        return q;
    }

    //取得目前玩家身上鴿子總數
    public int get_pigeon_Q()
    {
        return PigeonListQ;
    }

    //取得公鴿數量
    public int get_pigeon_male_Q()
    {
        return PigeonMaleQ;
    }

    //取得母鴿數量
    public int get_pigeon_female_Q()
    {
        return PigeonFemaleQ;
    }

    //取得自來鴿數量
    public int get_pigeon_feral_Q()
    {
        return PigeonFeralQ;
    }



    //初始化並取得玩家所有資料
    public void Init()
    {
        if (isInit)
            return;

        isInit = true; //標記初始化
        
        _ = get_user_info();
        _ = get_user_wallet();
        _ = get_pigeon_list(10000);
    }

}
