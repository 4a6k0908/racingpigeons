using System;
using AnimeTask;
using Core.Database;
using Core.Database.Login;
using Core.Title;
using UnityEngine;
using Zenject;

namespace UI.Title
{
    public class UI_Login : MonoBehaviour
    {
        private SignalBus   signalBus;
        private LoginSystem loginSystem;
        private AwsGraphQL  awsGraphQl;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        private void Inject(SignalBus signalBus, LoginSystem loginSystem, AwsGraphQL awsGraphQl)
        {
            this.loginSystem = loginSystem;
            this.signalBus   = signalBus;
            this.awsGraphQl  = awsGraphQl;
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnStateChange>(OnStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnStateChange>(OnStateChange);
        }

        private async void OnStateChange(OnStateChange e)
        {
            if (e.preState == State.Title && e.state == State.Login)
            {
                canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
                await Easing.Create<Linear>(1, 0.25f).ToColorA(canvasGroup);
            }
        }

        public async void Button_Guest_Login()
        {
            await loginSystem.Login(new GuestGetAwsUser(awsGraphQl));
        }
    }
}