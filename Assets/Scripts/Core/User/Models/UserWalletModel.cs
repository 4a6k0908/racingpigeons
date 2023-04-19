using System;

namespace Core.User.Models
{
    [Serializable]
    public class UserWalletModel
    {
        public int balance; //儲值金(金果幣)
        public int coin;    //遊戲金幣(賽鴿幣)
        public int ticket;  //票
    }
}