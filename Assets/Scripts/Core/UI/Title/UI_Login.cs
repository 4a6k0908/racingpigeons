﻿using System;
using System.Threading.Tasks;
using AnimeTask;
using Core.Database;
using Core.Database.Login;
using Core.Database.Models;
using Core.Player.Models;
using Core.Title;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Title
{
    public class UI_Login : MonoBehaviour
    {
        private SignalBus    signalBus;
        private LoginSystem  loginSystem;
        private StateHandler stateHandler;
        private PlayerData   playerData;

        private CanvasGroup canvasGroup;

        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private TMP_InputField  accountIF;
        [SerializeField] private TMP_InputField  passwordIF;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        private void Inject(SignalBus signalBus, LoginSystem loginSystem, StateHandler stateHandler, PlayerData playerData)
        {
            this.loginSystem  = loginSystem;
            this.signalBus    = signalBus;
            this.stateHandler = stateHandler;
            this.playerData   = playerData;
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnStateChange>(OnStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnStateChange>(OnStateChange);
        }

        public async void Button_Guest_Login()
        {
            var guestGetAwsUser = new GuestAwsUser(playerData.GetGraphQL());

            await Login(guestGetAwsUser);
        }

        public async void Button_Member_Login()
        {
            var account       = new Account(accountIF.text, passwordIF.text);
            var memberAwsUser = new MemberAwsUser(account);

            await Login(memberAwsUser);
        }

        private async UniTask Login(IGetAwsUser guestGetAwsUser)
        {
            if (stateHandler.GetCurrentState() != State.Login)
                return;

            stateHandler.ChangeState(State.AwsLogin);

            try
            {
                await loginSystem.Login(guestGetAwsUser);
            
                await playerData.SyncUserInfo();
                await playerData.SyncUserWallet();
            }
            catch (Exception e)
            {
                // TODO: Error Handle
                Debug.Log($"Error: {e.Message}");
            }
        }

        private async void OnStateChange(OnStateChange e)
        {
            switch (e.preState)
            {
                case State.Title when e.state == State.Login:
                    stateText.text = "登入遊戲";

                    canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
                    await Easing.Create<Linear>(1, 0.25f).ToColorA(canvasGroup);
                    break;
                case State.Login when e.state == State.AwsLogin:
                    stateText.text = "登入中...";
                    break;
                case State.AwsLogin when e.state == State.Login:
                    stateText.text = "登入遊戲";
                    break;
            }
        }

        public void Button_Google_Login() { }

        public void Button_Line_Login() { }
    }
}