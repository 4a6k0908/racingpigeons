using System;
using Core.Database;
using Core.Database.Models;
using Core.User.GraphQL;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.User.Models
{
    [Serializable]
    public class UserInfoModel
    {
        private readonly AwsGraphQL awsGraphQL;

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

        public UserInfoModel(AwsGraphQL awsGraphQL)
        {
            this.awsGraphQL = awsGraphQL;
        }

        public async UniTask<bool> GetUserInfo(AwsUserModel awsUserModel)
        {
            var query = "{\"query\":\""           +
                        "query MyQuery {\\n"      +
                        "getUserInfo {\\n"        +
                        "address\\n"              +
                        "avatar_url\\n"           +
                        "birthdate\\n"            +
                        "default_pigeonary_id\\n" +
                        "email\\n"                +
                        "gender\\n"               +
                        "identity_id\\n"          +
                        "name\\n"                 +
                        "nickname\\n"             +
                        "phone_number\\n"         +
                        "user_code\\n"            +
                        "username\\n"             +
                        "}\\n"                    +
                        "}"                       +
                        "\",\"variables\":{}}";

            var responseContent = await awsGraphQL.Post(query, awsUserModel.accessToken);
            var data            = JsonUtility.FromJson<GQL_GetUserInfo>(responseContent);
            
            if (data == null)
                return false;

            Debug.Log($"User Info: \n {JsonUtility.ToJson(data.data.getUserInfo)}");
            
            var userInfo = data.data.getUserInfo;

            address              = userInfo.address;
            avatar_url           = userInfo.avatar_url;
            birthdate            = userInfo.birthdate;
            default_pigeonary_id = userInfo.default_pigeonary_id;
            email                = userInfo.email;
            gender               = userInfo.gender;
            identity_id          = userInfo.identity_id;
            name                 = userInfo.name;
            nickname             = userInfo.nickname;
            phone_number         = userInfo.phone_number;
            user_code            = userInfo.user_code;
            username             = userInfo.username;
            
            return true;
        }
    }
}