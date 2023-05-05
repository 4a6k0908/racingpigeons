using System;
using AnimeTask;
using Core.Aws.Login;
using Core.Aws.Models;
using Core.NotifySystem;
using Core.Player.Models;
using Core.SceneSystem;
using Core.SoundSystem;
using Core.TitleScene;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Core.UI.Title
{
    public class UI_Login : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<AudioClip> clip_Click;

        private CanvasGroup       canvasGroup;
        private PlayerData        playerData;        // 玩家資料
        private SignalBus         signalBus;         // 事件發送器
        private TitleStateHandler titleStateHandler; // Title 場景的遊戲狀態
        private INotifyService    notifyService;     // 彈窗功能
        private ISceneService     sceneService;      // 場景轉換功能
        private ISoundService     soundService;      // 播放音效功能

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnTitleStateChange>(OnStateChange);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnTitleStateChange>(OnStateChange);
        }

        // 注入 class
        [Inject]
        private void Inject(
            ISceneService sceneService, ISoundService soundService, INotifyService notifyService,
            SignalBus signalBus, TitleStateHandler titleStateHandler, PlayerData playerData)
        {
            this.notifyService     = notifyService;
            this.sceneService      = sceneService;
            this.soundService      = soundService;
            this.signalBus         = signalBus;
            this.titleStateHandler = titleStateHandler;
            this.playerData        = playerData;
        }

        // 訪客登入
        public void Button_Guest_Login()
        {
            soundService.DoPlaySound(clip_Click);

            var guestGetAwsUser = new GuestAwsUser(playerData.GetGraphQL());

            Login(guestGetAwsUser);
        }

        // Google 登入
        public void Button_Google_Login()
        {
            soundService.DoPlaySound(clip_Click);

            notifyService.DoNotify("尚未開放", () => { });
        }

        // Line 登入
        public void Button_Line_Login()
        {
            soundService.DoPlaySound(clip_Click);

            notifyService.DoNotify("尚未開放", () => { });
        }

        // 確認是否玩家已登入過
        private void CheckAutoLogin()
        {
            var awsUserModel = playerData.GetAwsUserModel();

            if (awsUserModel.provider == null || !awsUserModel.provider.Equals("guest"))
                return;

            var account = new Account(awsUserModel.account.username, awsUserModel.account.password);

            var memberGetAwsUser = new MemberAwsUser(account);

            Login(memberGetAwsUser);
        }

        // 處理登入的功能
        private async void Login(IGetAwsUser guestGetAwsUser)
        {
            if (titleStateHandler.GetCurrentState() != TitleState.Login)
                return;

            titleStateHandler.ChangeState(TitleState.AwsLogin);

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
                titleStateHandler.ChangeState(TitleState.Login);
            }
        }

        // 收到狀態更改事件後，依條件淡出s
        private async void OnStateChange(OnTitleStateChange e)
        {
            switch (e.preState)
            {
                case TitleState.Title when e.state == TitleState.Login:
                    CheckAutoLogin();
                    canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
                    await Easing.Create<Linear>(1, 0.25f).ToColorA(canvasGroup);
                    break;
            }
        }
    }
}