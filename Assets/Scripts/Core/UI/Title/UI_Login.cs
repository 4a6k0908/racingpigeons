using System;
using AnimeTask;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.Player.Models;
using Core.Title;
using Cysharp.Threading.Tasks;
using SoapUtils.NotifySystem;
using SoapUtils.SceneSystem;
using TMPro;
using UnityEngine;
using Zenject;
using StateHandler = Core.Title.StateHandler;

namespace UI.Title
{
    public class UI_Login : MonoBehaviour
    {
        private ISceneService  sceneService;
        private INotifyService notifyService;
        private SignalBus      signalBus;
        private StateHandler   stateHandler;
        private PlayerData     playerData;

        private CanvasGroup canvasGroup;

        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private TMP_InputField  accountIF;
        [SerializeField] private TMP_InputField  passwordIF;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        private void Inject(ISceneService sceneService, INotifyService notifyService, SignalBus signalBus, StateHandler stateHandler, PlayerData playerData)
        {
            this.notifyService = notifyService;
            this.sceneService  = sceneService;
            this.signalBus     = signalBus;
            this.stateHandler  = stateHandler;
            this.playerData    = playerData;
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
                notifyService.DoNotify("登入中，請稍候");
                
                await playerData.Login(guestGetAwsUser);

                await playerData.SyncUserInfo();
                await playerData.SyncUserWallet();
                await playerData.SyncPigeonList(10);
                
                sceneService.DoLoadScene(1);
                
                notifyService.DoClose();
            }
            catch (Exception e)
            {
                notifyService.DoNotify($"發生錯誤: {e.Message}");
            }
        }

        private async void OnStateChange(OnStateChange e)
        {
            switch (e.preState)
            {
                case State.Title when e.state == State.Login:
                    canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
                    await Easing.Create<Linear>(1, 0.25f).ToColorA(canvasGroup);
                    break;
            }
        }

        public void Button_Google_Login() { }

        public void Button_Line_Login() { }
    }
}