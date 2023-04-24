using System;
using AnimeTask;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.Player.Models;
using Core.Save;
using Core.Title;
using SoapUtils.NotifySystem;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using StateHandler = Core.Title.StateHandler;

namespace UI.Title
{
    public class UI_Login : MonoBehaviour
    {
        private ISceneService  sceneService;
        private ISoundService  soundService;
        private INotifyService notifyService;
        private SignalBus      signalBus;
        private StateHandler   stateHandler;
        private PlayerData     playerData;

        private CanvasGroup canvasGroup;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Click;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        private void Inject(
            ISceneService sceneService, ISoundService soundService, INotifyService notifyService,
            SignalBus signalBus, StateHandler stateHandler, PlayerData playerData)
        {
            this.notifyService = notifyService;
            this.sceneService  = sceneService;
            this.soundService  = soundService;
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

        public void Button_Guest_Login()
        {
            soundService.DoPlaySound(clip_Click);

            var guestGetAwsUser = new GuestAwsUser(playerData.GetGraphQL());

            Login(guestGetAwsUser);
        }

        public void Button_Google_Login()
        {
            soundService.DoPlaySound(clip_Click);
        }

        public void Button_Line_Login()
        {
            soundService.DoPlaySound(clip_Click);
        }

        private void CheckAutoLogin()
        {
            var awsUserModel = playerData.GetAwsUserModel();

            Debug.Log($"Provider: {awsUserModel.provider}");

            if (awsUserModel.provider == null || !awsUserModel.provider.Equals("guest"))
                return;

            var account = new Account(awsUserModel.account.username, awsUserModel.account.password);

            var memberGetAwsUser = new MemberAwsUser(account);

            Login(memberGetAwsUser);
        }

        private async void Login(IGetAwsUser guestGetAwsUser)
        {
            if (stateHandler.GetCurrentState() != State.Login)
                return;

            stateHandler.ChangeState(State.AwsLogin);

            try
            {
                notifyService.DoNotify("登入中，請稍候");

                await playerData.Login(guestGetAwsUser);

                notifyService.DoNotify("取得玩家資訊中");
                await playerData.SyncUserInfo();

                notifyService.DoNotify("取得玩家錢包資訊中");
                await playerData.SyncUserWallet();

                notifyService.DoNotify("取得鴿子資訊中");
                await playerData.SyncPigeonList(10);

                notifyService.DoClose();

                sceneService.DoLoadScene(1);
            }
            catch (Exception e)
            {
                notifyService.DoNotify(e.Message, () => { });
                stateHandler.ChangeState(State.Login);
            }
        }

        private async void OnStateChange(OnStateChange e)
        {
            switch (e.preState)
            {
                case State.Title when e.state == State.Login:
                    CheckAutoLogin();
                    canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
                    await Easing.Create<Linear>(1, 0.25f).ToColorA(canvasGroup);
                    break;
            }
        }
    }
}