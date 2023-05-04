using System;
using AnimeTask;
using Core.TitleScene;
using UnityEngine;
using Zenject;

namespace UI.Title
{
    public class UI_Title : MonoBehaviour
    {
        private SignalBus         signalBus;         // 事件發射器
        private TitleStateHandler titleStateHandler; // Title 場景的遊戲狀態

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // 注入 class
        [Inject]
        private void Inject(SignalBus signalBus, TitleStateHandler titleStateHandler)
        {
            this.signalBus         = signalBus;
            this.titleStateHandler = titleStateHandler;
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnTitleStateChange>(OnStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnTitleStateChange>(OnStateChange);
        }

        public void Button_To_Login()
        {
            titleStateHandler.ChangeState(TitleState.Login);
        }

        // 當接收到狀態變更時處理淡出功能
        private async void OnStateChange(OnTitleStateChange e)
        {
            if (e.preState == TitleState.Title && e.state == TitleState.Login)
            {
                canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
                await Easing.Create<Linear>(0, 0.25f).ToColorA(canvasGroup);
            }
        }
    }
}