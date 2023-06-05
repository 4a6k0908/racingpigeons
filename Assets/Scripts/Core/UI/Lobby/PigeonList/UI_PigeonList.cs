using System;
using System.Threading;
using AnimeTask;
using Core.LobbyScene;
using Core.NotifySystem;
using Core.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Lobby.PigeonList
{
    public class UI_PigeonList : MonoBehaviour
    {
        private SignalBus         signalBus;
        private LobbyStateHandler lobbyStateHandler;
        private INotifyService    notifyService;
        private PlayerData        playerData;
        /*
        private PigeonListViewMode currentViewMode = PigeonListViewMode.List;
        private PigeonListSort     currentSort     = PigeonListSort.IQ;
        private PigeonListFilter   currentFilter   = PigeonListFilter.None;
        private PigeonListOrder    currentOrder    = PigeonListOrder.Descending;
        */
        private CanvasGroup canvasGroup;

        [SerializeField] public Core.UI.Lobby.PigeonListViewer.PigeonListViewer pigeonViewerPanel; // 鴿子滾動物件
        [SerializeField] Button btnColse;
        [SerializeField] Button btnBack;
        [SerializeField] Button btnSignRace;
        [SerializeField] Button btnPigeonShed;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            btnColse.onClick.RemoveAllListeners();
            btnBack.onClick.RemoveAllListeners();
            btnSignRace.onClick.RemoveAllListeners();
            btnPigeonShed.onClick.RemoveAllListeners();

            btnColse.onClick.AddListener(delegate { Button_Close(); });
            btnBack.onClick.AddListener(delegate { Button_Previous(); });
            //btnSignRace.onClick.AddListener(delegate { Button_Close(); });
            btnPigeonShed.onClick.AddListener(delegate { Button_Close(); });
        }

        [Inject]
        public void Inject(SignalBus signalBus, LobbyStateHandler lobbyStateHandler, INotifyService notifyService, PlayerData playerData)
        {
            this.signalBus         = signalBus;
            this.lobbyStateHandler = lobbyStateHandler;
            this.notifyService     = notifyService;
            this.playerData        = playerData;
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnLobbyStateChange>(OnLobbyStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnLobbyStateChange>(OnLobbyStateChange);
        }

        // 回到 1-1-1
        public void Button_Close()
        {
            lobbyStateHandler.ChangeState(LobbyState.Lobby);
        }

        // 返回上一層
        public void Button_Previous()
        {
            lobbyStateHandler.ChangeToPreState();
        }

        // 遊戲狀態更改時觸發
        private void OnLobbyStateChange(OnLobbyStateChange e)
        {
            if (e.currentState == LobbyState.PigeonList)
            {
                SetActive(true);
            }
            else if (e.preState == LobbyState.PigeonList)
            {
                SetActive(false);
            }
        }

        private async void SetActive(bool IsActive)
        {
            if (IsActive)
            {
                try
                {
                    // TODO: 暫定只找10隻
                    notifyService.DoNotify("取得鴿子資料中");
                    await playerData.SyncPigeonList(10);
                    notifyService.DoClose();
                    
                    pigeonViewerPanel.InitView(playerData.GetPigeonList());
                    // pigeonStatScroller.SetTestOriginData();
                }
                catch (Exception e)
                {
                    notifyService.DoNotify(e.Message, () => { });
                    lobbyStateHandler.ChangeToPreState();
                    return;
                }
            }

            canvasGroup.blocksRaycasts = canvasGroup.interactable = IsActive;
            await Easing.Create<Linear>(IsActive ? 1 : 0, 0.15f).ToColorA(canvasGroup);

            if (!IsActive)
                return;
        }
    }
}