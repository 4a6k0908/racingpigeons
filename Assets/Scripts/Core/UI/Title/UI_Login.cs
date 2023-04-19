using System;
using AnimeTask;
using Core.Database;
using Core.Database.Login;
using Core.Database.Models;
using Core.Title;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Title
{
    public class UI_Login : MonoBehaviour
    {
        private SignalBus    signalBus;
        private LoginSystem  loginSystem;
        private AwsGraphQL   awsGraphQL;
        private StateHandler stateHandler;

        private CanvasGroup canvasGroup;

        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private TMP_InputField  accountIF;
        [SerializeField] private TMP_InputField  passwordIF;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        private void Inject(SignalBus signalBus, LoginSystem loginSystem, AwsGraphQL awsGraphQL, StateHandler stateHandler)
        {
            this.loginSystem  = loginSystem;
            this.signalBus    = signalBus;
            this.awsGraphQL   = awsGraphQL;
            this.stateHandler = stateHandler;
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
            if (!CanLogin())
                return;

            var guestGetAwsUser = new GuestGetAwsUser(awsGraphQL);

            await loginSystem.Login(guestGetAwsUser);
        }

        public async void Button_Member_Login()
        {
            if(!CanLogin())
                return;

            var account       = new Account(accountIF.text, passwordIF.text);
            var memberAwsUser = new MemberAwsUser(account);

            await loginSystem.Login(memberAwsUser);
        }

        private bool CanLogin()
        {
            if (stateHandler.GetCurrentState() != State.Login)
                return false;

            stateHandler.ChangeState(State.AwsLogin);
            
            return true;
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