using Core.Player.Models;
using Core.User.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace SoapUtils.Utils.Lobby
{
    public class UI_Wallet : MonoBehaviour
    {
        private PlayerData playerData;

        [SerializeField] private TextMeshProUGUI pigeonCashText;
        [SerializeField] private TextMeshProUGUI goldenCashText;

        [Inject]
        public void Inject(PlayerData playerData)
        {
            this.playerData = playerData;

            SetWallet(this.playerData.GetUserWalletModel());
        }

        private void SetWallet(UserWalletModel userWalletModel)
        {
            pigeonCashText.text = userWalletModel.coin.ToString();
            goldenCashText.text = userWalletModel.balance.ToString();
        }
    }
}