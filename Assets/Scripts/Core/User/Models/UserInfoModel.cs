using System;

namespace Core.User.Models
{
    [Serializable]
    public class UserInfoModel
    {
        public string address; //地址
        public string avatar_url;
        public string birthdate;            //生日
        public string default_pigeonary_id; //默認鴿舍ID(暫無)
        public string email;                //Email
        public int    gender;               //性別
        public string identity_id;          //身分證字號
        public string name;                 //姓名
        public string nickname;             //玩家暱稱
        public string phone_number;         //手機號碼
        public string user_code;            //使用者代碼
        public string username;             //帳號
    }
}