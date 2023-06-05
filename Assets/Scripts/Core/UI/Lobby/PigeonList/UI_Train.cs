using System;
using System.Collections.Generic;
//using AnimeTask;
using Core.LobbyScene;
using Core.NotifySystem;
using Core.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;
using static Core.Effects.Models.EffectModel.GQL_GetEffectList.Data;

namespace Core.UI.Lobby.Train
{
    public enum ViewMode
    {
        view3_1 = 0,
        view3_2 = 1,
        view3_3 = 2,
        view3_4 = 3,
    }

    public class UI_Train : MonoBehaviour
    {
        private SignalBus signalBus;
        private LobbyStateHandler lobbyStateHandler;
        private INotifyService notifyService;
        private PlayerData playerData;

        private CanvasGroup canvasGroup;
        [Header("Pages: Equal to view model")]
        public CanvasGroup[] pages;
        [SerializeField] Button btnBack;

        [Header("3-1")]
        [SerializeField] private Sprite[] coverSprites;
        [SerializeField] private Image sprCover;
        [SerializeField] private UI_Train_Scroller trainScroller; // 鴿子滾動物件

        [Header("3-3")]
        [SerializeField] public Core.UI.Lobby.PigeonListViewer.PigeonListViewer pigeonViewerPanel; // 鴿子滾動物件


        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(delegate { Button_Previous(); });
        }

        public void Init_View()
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].alpha = 0;
                pages[i].blocksRaycasts = pages[i].interactable = false;
            }
        }

        [Inject]
        public void Inject(SignalBus signalBus, PlayerData playerData, LobbyStateHandler lobbyStateHandler, INotifyService notifyService)
        {
            this.signalBus = signalBus;
            this.playerData = playerData;
            this.notifyService = notifyService;
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

        public void Button_View(int _index)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].DOFade(i <= _index ? 1 : 0, 0.5f).SetEase(Ease.InExpo);
                pages[i].blocksRaycasts = pages[i].interactable = i <= _index ? true : false;
            }
        }

        void coverChange(int _index)
        {
            sprCover.sprite = coverSprites[_index];
        }

        public List<Effect> EffectsOutPut(List<Effect> _les)
        {
            for (int i = 0; i < _les.Count; i++)
            {
                int index = i;
                _les[i].add_delegates = new UnityAction(delegate
                {
                    Button_View(2);
                    Debug.Log(_les[index].effect_id);
                    coverChange(int.Parse(_les[index].effect_id) - 1);
                });
            }
            return _les;
        }

        // 遊戲狀態更改時觸發
        private void OnLobbyStateChange(OnLobbyStateChange e)
        {
            if (e.currentState == LobbyState.Train)
            {
                SetActive(true);
            }
            else if (e.preState == LobbyState.Train)
            {
                SetActive(false);
            }
        }

        private async void SetActive(bool IsActive)
        {
            Init_View();
            if (IsActive)
            {
                try
                {
                    notifyService.DoNotify("取得訓練資料中");
                    await playerData.SyncGetTrainList();
                    notifyService.DoClose();

                    trainScroller.UpdateData(EffectsOutPut(playerData.GetEffects()));
                    pigeonViewerPanel.InitView(playerData.GetPigeonList());

                    Button_View(0);
                }
                catch (Exception e)
                {
                    notifyService.DoNotify(e.Message, () => { });
                    lobbyStateHandler.ChangeToPreState();
                    return;
                }
            }

            /*canvasGroup.blocksRaycasts = canvasGroup.interactable = IsActive;
            await Easing.Create<Linear>(IsActive ? 1 : 0, 0.15f).ToColorA(canvasGroup);
            */

            canvasGroup.DOFade(IsActive ? 1 : 0, 0.2f).SetEase(Ease.InSine).onComplete = () =>
            {
                canvasGroup.blocksRaycasts = canvasGroup.interactable = IsActive;
            };

            if (!IsActive)
                return;

        }
    }
}