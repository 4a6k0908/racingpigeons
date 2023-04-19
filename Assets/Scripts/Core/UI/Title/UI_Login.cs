using System;
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

        private void OnStateChange(OnStateChange e) { }

        public async void Button_Guest_Login()
        {
            await loginSystem.Login(new GuestGetAwsUser(awsGraphQl));
        }
    }
}