using System;
using AnimeTask;
using Core.Lobby;
using Core.UI.Lobby.PigeonList;
using UnityEngine;
using Zenject;

namespace SoapUtils.Utils.Lobby
{
    public class UI_Lobby : MonoBehaviour
    {
        private SignalBus         signalBus;
        private LobbyStateHandler lobbyStateHandler;

        private CanvasGroup   canvasGroup;
        
        [SerializeField] private UI_PigeonList pigeonListUI;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        public void Inject(SignalBus signalBus, LobbyStateHandler lobbyStateHandler)
        {
            this.signalBus         = signalBus;
            this.lobbyStateHandler = lobbyStateHandler;
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnLobbyStateChange>(OnLobbyStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnLobbyStateChange>(OnLobbyStateChange);
        }

        // 到鴿子檢視
        public void Button_To_PigeonList(int filter)
        {
            lobbyStateHandler.ChangeState(LobbyState.PigeonList);
            pigeonListUI.Toggle_Change_Filter(filter);
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
            Easing.Create<Linear>(IsActive ? 1 : 0, 0.25f).ToColorA(canvasGroup);
        }
    }
}