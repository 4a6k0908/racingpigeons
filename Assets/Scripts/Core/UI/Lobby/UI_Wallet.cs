using Core.MainScene;
using Core.Player.Models;
using Core.User.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.UI.Lobby
{
    public class UI_Wallet : MonoBehaviour
    {
        private SignalBus  signalBus;
        private PlayerData playerData;

        [SerializeField] private TextMeshProUGUI pigeonCashText;
        [SerializeField] private TextMeshProUGUI goldenCashText;

        [Inject]
        public void Inject(PlayerData playerData, SignalBus signalBus)
        {
            this.playerData = playerData;
            this.signalBus  = signalBus;

            SetWallet(this.playerData.GetUserWalletModel());
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnUserWalletUpdate>(OnUserWalletUpdate);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnUserWalletUpdate>(OnUserWalletUpdate);
        }

        private void OnUserWalletUpdate(OnUserWalletUpdate e)
        {
            SetWallet(e.userWalletModel);
        }
        
        private void SetWallet(UserWalletModel userWalletModel)
        {
            pigeonCashText.text = userWalletModel.coin.ToString();
            goldenCashText.text = userWalletModel.balance.ToString();
        }
    }
}