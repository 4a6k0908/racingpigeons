﻿using System;
using Core.Database.Models;

namespace Core.User.Models
{
    [Serializable]
    public class AwsUserModel
    {
        public string  deviceID;     //用戶唯一編碼(創號用) 
        public Account account;      //aws 登入帳號,密碼
        
        public string  idToken;      //
        public string  userID;       //
        
        public string  accessToken;  //連線aws api 用
        public string  refreshToken;
        public string  provider;     //guest(遊客) ,google,line
                                     //此參數不為guest或空白,代表巳綁定
    }
}