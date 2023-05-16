using System.Linq;
using AnimeTask;
using Core.CameraSystem;
using Core.LobbyScene;
using Core.MainScene;
using Core.Player;
using Core.UI.Lobby.PigeonList;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.UI.Lobby
{
    public class UI_Lobby : MonoBehaviour
    {
        private SignalBus         signalBus;
        private LobbyStateHandler lobbyStateHandler;
        private ICameraService    cameraService;

        private CanvasGroup   canvasGroup;

        [SerializeField] private TextMeshProUGUI pigeonTotalCountText;
        [SerializeField] private TextMeshProUGUI pigeonMaleCountText;
        [SerializeField] private TextMeshProUGUI pigeonFemaleCountText;
        [SerializeField] private TextMeshProUGUI pigeonFeralCountText;
        
        [SerializeField] private UI_PigeonList   pigeonListUI;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        public void Inject(SignalBus signalBus, LobbyStateHandler lobbyStateHandler, ICameraService cameraService, PlayerData playerData)
        {
            this.signalBus         = signalBus;
            this.lobbyStateHandler = lobbyStateHandler;
            this.cameraService     = cameraService;

            OnPigeonListUpdate(new OnPigeonListUpdate(playerData.GetPigeonList()));
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnLobbyStateChange>(OnLobbyStateChange);
            signalBus.Subscribe<OnPigeonListUpdate>(OnPigeonListUpdate);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnLobbyStateChange>(OnLobbyStateChange);
            signalBus.Unsubscribe<OnPigeonListUpdate>(OnPigeonListUpdate);
        }

        // 到鴿子檢視
        public void Button_To_PigeonList(int filter)
        {
            pigeonListUI.ChangeFilter(filter);
            lobbyStateHandler.ChangeState(LobbyState.PigeonList);
        }

        public void Button_Camera_ChangeView()
        {
            cameraService.DoChangePigeonHouseView();
        }

        private void OnPigeonListUpdate(OnPigeonListUpdate e)
        {
            // TODO: 自來鴿還沒有資訊所以暫時無計算
            pigeonTotalCountText.text = e.pigeonStats.Count.ToString();

            var maleCount   = 0;
            var femaleCount = 0;
            
            for (int i = 0; i < e.pigeonStats.Count; i++)
            {
                switch (e.pigeonStats[i].gender)
                {
                    case 0:
                        maleCount++;
                        break;
                    case 1:
                        femaleCount++;
                        break;
                }
            }

            pigeonMaleCountText.text   = maleCount.ToString();
            pigeonFemaleCountText.text = femaleCount.ToString();
        }
        
        private void OnLobbyStateChange(OnLobbyStateChange e)
        {
            if (e.currentState == LobbyState.Lobby)
            {
                SetActive(true);
            }
            else if (e.preState == LobbyState.Lobby)
            {
                SetActive(false);
            }
        }

        private void SetActive(bool IsActive)
        {
            canvasGroup.blocksRaycasts = canvasGroup.interactable = IsActive;
            Easing.Create<Linear>(IsActive ? 1 : 0, 0.15f).ToColorA(canvasGroup);
        }
    }
}