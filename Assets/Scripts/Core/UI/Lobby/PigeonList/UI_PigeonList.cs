using System;
using System.Threading;
using AnimeTask;
using Core.LobbyScene;
using Core.NotifySystem;
using Core.Player.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Lobby.PigeonList
{
    public enum PigeonListViewMode
    {
        List       = 0, // 2-1-1
        HalfPigeon = 1, // 2-1-2
        FullPigeon = 2  // 2-1-3
    }

    // 鴿子的過濾種類
    public enum PigeonListFilter
    {
        None     = 0, // 無過濾
        Favorite = 1, // 最愛
        Male     = 2, // 雄
        Female   = 3, // 雌
        Feral    = 4, // 外來
    }

    // 鴿子的排序模式
    public enum PigeonListSort
    {
        IQ             = 0, // 智商
        Vision         = 1, // 眼睛
        Speed          = 2, // 體型
        FeatherSize    = 3, // 羽型
        Vitality       = 4, // 心肺
        Muscle         = 5, // 肌力
        FeatherQuality = 6, // 羽質
        Sick           = 7, // 生病
        Other          = 8, // 其他
    }

    // 升序、降序
    public enum PigeonListOrder
    {
        Descending = 0,
        Ascending  = 1
    }

    public class UI_PigeonList : MonoBehaviour
    {
        private SignalBus         signalBus;
        private LobbyStateHandler lobbyStateHandler;
        private INotifyService    notifyService;
        private PlayerData        playerData;

        private PigeonListViewMode currentViewMode = PigeonListViewMode.List;
        private PigeonListSort     currentSort     = PigeonListSort.IQ;
        private PigeonListFilter   currentFilter   = PigeonListFilter.None;
        private PigeonListOrder    currentOrder    = PigeonListOrder.Descending;

        private CanvasGroup canvasGroup;

        [SerializeField] private UI_PigeonStat_Scroller pigeonStatScroller; // 鴿子滾動物件

        [SerializeField] private RectTransform pigeonBookTrans;   // 能力背景的位置
        [SerializeField] private Image         pigeonBookImg;     // 能力背景的圖
        [SerializeField] private Sprite[]      pigeonBookSprites; // 能力背景的更換圖片

        [SerializeField] private GameObject    pigeon3DRenderObj;      // 3D RenderTexture 得物件 
        [SerializeField] private RectTransform pigeon3DRenderTexTrans; // 3D RenderTexture 的位置 

        [SerializeField] private RectTransform expandBtnTrans; // 左上角擴展的按鈕位置
        [SerializeField] private GameObject    toListBtnObj;   // 返回檢視模式的按鈕物件

        [SerializeField] private Toggle[] filterToggles; // 左邊四個的過濾器 Toggle

        private CancellationTokenSource pigeonBookTaskToken;     // 能力背景的移動動畫 Token
        private CancellationTokenSource pigeon3DRenderTaskToken; // 3D RenderTexture 移動動畫 Token

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [Inject]
        public void Inject(SignalBus signalBus, LobbyStateHandler lobbyStateHandler, INotifyService notifyService, PlayerData playerData)
        {
            this.signalBus         = signalBus;
            this.lobbyStateHandler = lobbyStateHandler;
            this.notifyService     = notifyService;
            this.playerData        = playerData;
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

        // 到 2-1-3
        public void Button_To_Half()
        {
            ChangeMode(PigeonListViewMode.HalfPigeon);
        }

        // 如果目前為 2-1-3 那會到 2-1-2，其餘狀況到 2-1-1
        public void Button_Expand()
        {
            ChangeMode(currentViewMode == PigeonListViewMode.HalfPigeon ? PigeonListViewMode.FullPigeon : PigeonListViewMode.List);
        }

        // 回到 2-1-1
        public void Button_To_List()
        {
            ChangeMode(PigeonListViewMode.List);
        }

        // 更改能力排序
        public void Button_Change_Ability_Sort(int sort)
        {
            pigeonBookImg.sprite = pigeonBookSprites[sort];

            PigeonListSort nextSort = (PigeonListSort)sort;

            if (currentSort == nextSort)
            {
                currentOrder = (currentOrder == PigeonListOrder.Descending) ? PigeonListOrder.Ascending : PigeonListOrder.Descending;
            }
            else
            {
                currentSort  = nextSort;
                currentOrder = PigeonListOrder.Descending;
            }

            pigeonStatScroller.ChangePresent(currentFilter, currentSort, currentOrder);
        }

        // 更改過濾
        public void Toggle_Change_Filter(int filter)
        {
            ChangeFilter(filter);

            pigeonStatScroller.ChangePresent(currentFilter, currentSort, currentOrder);
        }

        public void ChangeFilter(int filter)
        {
            PigeonListFilter nextFilter = (PigeonListFilter)filter;

            if (currentFilter == nextFilter)
            {
                currentFilter = PigeonListFilter.None;

                if (nextFilter != PigeonListFilter.None)
                    filterToggles[filter - 1].SetIsOnWithoutNotify(false);
            }
            else
            {
                currentFilter = nextFilter;

                if (nextFilter != PigeonListFilter.None)
                    filterToggles[filter - 1].SetIsOnWithoutNotify(true);
            }
        }

        // 更換瀏覽模式時，執行移動動畫跟物件顯示
        public void ChangeMode(PigeonListViewMode viewMode)
        {
            if (currentViewMode == viewMode)
                return;

            currentViewMode = viewMode;

            pigeonBookTaskToken?.Cancel();
            pigeonBookTaskToken = new CancellationTokenSource();

            pigeon3DRenderTaskToken?.Cancel();
            pigeon3DRenderTaskToken = new CancellationTokenSource();

            switch (currentViewMode)
            {
                case PigeonListViewMode.List:
                    Easing.Create<OutQuint>(20, 0.5f).ToAnchoredPositionX(pigeonBookTrans).ToCancellationToken(pigeonBookTaskToken.Token);
                    Easing.Create<OutQuint>(-600, 0.5f).ToAnchoredPositionX(pigeon3DRenderTexTrans).ToCancellationToken(pigeon3DRenderTaskToken.Token);
                    break;
                case PigeonListViewMode.HalfPigeon:
                    Easing.Create<OutQuint>(700, 0.5f).ToAnchoredPositionX(pigeonBookTrans).ToCancellationToken(pigeonBookTaskToken.Token);
                    Easing.Create<OutQuint>(-600, 0.5f).ToAnchoredPositionX(pigeon3DRenderTexTrans).ToCancellationToken(pigeon3DRenderTaskToken.Token);

                    expandBtnTrans.localEulerAngles = new Vector3(0, 0, 180);
                    break;
                case PigeonListViewMode.FullPigeon:
                    Easing.Create<OutQuint>(2000, 0.5f).ToAnchoredPositionX(pigeonBookTrans).ToCancellationToken(pigeonBookTaskToken.Token);
                    Easing.Create<OutQuint>(0, 0.5f).ToAnchoredPositionX(pigeon3DRenderTexTrans).ToCancellationToken(pigeon3DRenderTaskToken.Token);

                    expandBtnTrans.localEulerAngles = new Vector3(0, 0, 0);
                    break;
            }

            pigeon3DRenderObj.SetActive(currentViewMode                 != PigeonListViewMode.List);
            pigeon3DRenderTexTrans.gameObject.SetActive(currentViewMode != PigeonListViewMode.List);

            expandBtnTrans.gameObject.SetActive(currentViewMode != PigeonListViewMode.List);
            toListBtnObj.SetActive(currentViewMode              == PigeonListViewMode.HalfPigeon);
        }

        // 遊戲狀態更改時觸發
        private void OnLobbyStateChange(OnLobbyStateChange e)
        {
            if (e.currentState == LobbyState.PigeonList)
            {
                SetActive(true);
            }
            else if (e.preState == LobbyState.PigeonList)
            {
                SetActive(false);
            }
        }

        private async void SetActive(bool IsActive)
        {
            if (IsActive)
            {
                try
                {
                    // TODO: 暫定只找10隻
                    notifyService.DoNotify("取得鴿子資料中");
                    await playerData.SyncPigeonList(10);
                    notifyService.DoClose();
                    
                    pigeonStatScroller.SetOriginData(playerData.GetPigeonList());
                    // pigeonStatScroller.SetTestOriginData();
                }
                catch (Exception e)
                {
                    notifyService.DoNotify(e.Message, () => { });
                    lobbyStateHandler.ChangeToPreState();
                    return;
                }
            }

            canvasGroup.blocksRaycasts = canvasGroup.interactable = IsActive;
            await Easing.Create<Linear>(IsActive ? 1 : 0, 0.15f).ToColorA(canvasGroup);

            if (!IsActive)
                return;

            pigeonStatScroller.Open(currentFilter, currentSort, currentOrder);
            
            pigeonBookImg.sprite = pigeonBookSprites[(int) currentSort];
        }
    }
}