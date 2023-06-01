using System;
using System.Collections.Generic;
using System.Linq;
using Core.Effects.Lobby;
using Core.Effects.Models;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI.Extensions.EasingCore;

namespace Core.UI.Lobby.PigeonList
{
    public class UI_PigeonStat_Scroller : FancyScrollRect<PigeonStat, Context>
    {
        [SerializeField] private Scroller   scroller;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float      cellSize;

        protected override GameObject CellPrefab => cellPrefab;
        protected override float      CellSize   => cellSize;

        private List<PigeonStat> originPigeonStatList  = new();
        private List<PigeonStat> currentPigeonStatList = new();

        private string selectPigeonID = "";

        private Pigeon3DRenderTexture pigeon3DRenderTexture;

        private void Awake()
        {
            Relayout();

            pigeon3DRenderTexture = FindObjectOfType<Pigeon3DRenderTexture>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            Context.OnCellClicked += OnCellClick;
        }

        // 每次打開會執行的動作，
        public void Open(PigeonListFilter filter, PigeonListSort sort, PigeonListOrder order)
        {
            ChangePresent(filter, sort, order);

            if (currentPigeonStatList.Count > 0)
            {
                selectPigeonID = currentPigeonStatList[0].pigeon_id;
                Context.OnCellClicked?.Invoke(0);
            }
        }

        public void SetOriginData(List<PigeonStat> pigeonStats)
        {
            originPigeonStatList = pigeonStats;
        }

        // TODO: 測試用
        public void SetTestOriginData()
        {
            originPigeonStatList = GetTestPigeons();
        }

        public void UpdateData(List<PigeonStat> pigeonStats)
        {
            UpdateContents(pigeonStats);
            scroller.SetTotalCount(pigeonStats.Count);

            JumpTo(0);
        }

        public void ScrollTo(int index)
        {
            ScrollTo(index, 0.25f, Ease.InOutQuint);
        }

        public void JumpTo(int index)
        {
            JumpTo(index, 0.5f);
        }

        // 點擊 Cell 後該執行的動作
        public void OnCellClick(int index)
        {
            if (index < 0 || index >= ItemsSource.Count || index == Context.selectedIndex)
            {
                return;
            }

            Context.selectedIndex = index;
            selectPigeonID        = currentPigeonStatList[index].pigeon_id;
            pigeon3DRenderTexture.ChangeTexture(currentPigeonStatList[index].breed_id);
        }

        // 更改排序、過濾
        public void ChangePresent(PigeonListFilter filter, PigeonListSort sort, PigeonListOrder order)
        {
            // TODO: 這裡後續可以優化至 PigeonModel 處理
            // TODO: 這方法是先以有為主，後續可更換演算法
            currentPigeonStatList = new List<PigeonStat>();

            Context.selectedIndex = -1;
            int selectIndex = 0;
            
            // 過濾
            switch (filter)
            {
                case PigeonListFilter.None:
                case PigeonListFilter.Feral:
                case PigeonListFilter.Favorite:
                    currentPigeonStatList = originPigeonStatList;
                    break;
                case PigeonListFilter.Male:
                    currentPigeonStatList.AddRange(originPigeonStatList.Where(pigeonStat => pigeonStat.gender == 0));
                    break;
                case PigeonListFilter.Female:
                    currentPigeonStatList.AddRange(originPigeonStatList.Where(pigeonStat => pigeonStat.gender == 1));
                    break;
            }

            if (currentPigeonStatList.Count > 0)
            {
                // 依能力排序
                switch (sort)
                {
                    case PigeonListSort.IQ:
                    case PigeonListSort.Sick:
                    case PigeonListSort.Other:
                        currentPigeonStatList = currentPigeonStatList.OrderByDescending(pigeonStat => pigeonStat.iq).ToList();
                        break;
                    case PigeonListSort.Vision:
                        currentPigeonStatList = currentPigeonStatList.OrderByDescending(pigeonStat => pigeonStat.vision).ToList();
                        break;
                    case PigeonListSort.Speed:
                        currentPigeonStatList = currentPigeonStatList.OrderByDescending(pigeonStat => pigeonStat.speed).ToList();
                        break;
                    case PigeonListSort.FeatherSize:
                        currentPigeonStatList = currentPigeonStatList.OrderByDescending(pigeonStat => pigeonStat.feather_size).ToList();
                        break;
                    case PigeonListSort.Vitality:
                        currentPigeonStatList = currentPigeonStatList.OrderByDescending(pigeonStat => pigeonStat.vitality).ToList();
                        break;
                    case PigeonListSort.Muscle:
                        currentPigeonStatList = currentPigeonStatList.OrderByDescending(pigeonStat => pigeonStat.muscle).ToList();
                        break;
                    case PigeonListSort.FeatherQuality:
                        currentPigeonStatList = currentPigeonStatList.OrderByDescending(pigeonStat => pigeonStat.feather_quality).ToList();
                        break;
                }

                // 升序
                if (order == PigeonListOrder.Ascending)
                    currentPigeonStatList.Reverse();

                for (int i = 0; i < currentPigeonStatList.Count; i++)
                {
                    if (selectPigeonID != currentPigeonStatList[i].pigeon_id)
                        continue;
                    
                    Context.selectedIndex = selectIndex = i;
                    break;
                }
            }

            UpdateData(currentPigeonStatList);

            Context.OnCellClicked?.Invoke(selectIndex);
        }

        // TODO: 測試專用，之後刪掉

        #region Test

        public List<PigeonStat> GetTestPigeons()
        {
            return new List<PigeonStat> {
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = "5",
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 100,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 50,
                    level                  = 0,
                    max_constitution       = 960,
                    max_feather_quality    = 300,
                    max_feather_size       = 400,
                    max_iq                 = 500,
                    max_muscle             = 200,
                    max_speed              = 200,
                    max_vision             = 300,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 100,
                    pigeon_id              = "01",
                    pigeon_name            = "Test",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = "10",
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 30,
                    father_pigeon_id       = null,
                    fatigue                = 50,
                    feather_quality        = 150,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 0,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 60,
                    level                  = 0,
                    max_constitution       = 960,
                    max_feather_quality    = 200,
                    max_feather_size       = 400,
                    max_iq                 = 300,
                    max_muscle             = 100,
                    max_speed              = 100,
                    max_vision             = 100,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 100,
                    pigeon_id              = "02",
                    pigeon_name            = "Test2",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = null,
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 100,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 70,
                    level                  = 0,
                    max_constitution       = 450,
                    max_feather_quality    = 250,
                    max_feather_size       = 350,
                    max_iq                 = 410,
                    max_muscle             = 150,
                    max_speed              = 150,
                    max_vision             = 175,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 500,
                    pigeon_id              = "03",
                    pigeon_name            = "Test3",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = null,
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 100,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 0,
                    level                  = 0,
                    max_constitution       = 450,
                    max_feather_quality    = 250,
                    max_feather_size       = 350,
                    max_iq                 = 0,
                    max_muscle             = 150,
                    max_speed              = 150,
                    max_vision             = 175,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 100,
                    pigeon_id              = "04",
                    pigeon_name            = "Test4",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = null,
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 100,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 0,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 200,
                    level                  = 0,
                    max_constitution       = 250,
                    max_feather_quality    = 375,
                    max_feather_size       = 275,
                    max_iq                 = 250,
                    max_muscle             = 200,
                    max_speed              = 200,
                    max_vision             = 300,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 300,
                    pigeon_id              = "05",
                    pigeon_name            = "Test5",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = null,
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 100,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 120,
                    level                  = 0,
                    max_constitution       = 960,
                    max_feather_quality    = 300,
                    max_feather_size       = 400,
                    max_iq                 = 500,
                    max_muscle             = 200,
                    max_speed              = 200,
                    max_vision             = 300,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 10,
                    pigeon_id              = "06",
                    pigeon_name            = "Test6",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = null,
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 100,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 100,
                    level                  = 0,
                    max_constitution       = 960,
                    max_feather_quality    = 300,
                    max_feather_size       = 400,
                    max_iq                 = 500,
                    max_muscle             = 200,
                    max_speed              = 200,
                    max_vision             = 300,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 75,
                    pigeon_id              = "07",
                    pigeon_name            = "Test7",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = null,
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 100,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 95,
                    level                  = 0,
                    max_constitution       = 960,
                    max_feather_quality    = 300,
                    max_feather_size       = 400,
                    max_iq                 = 500,
                    max_muscle             = 200,
                    max_speed              = 200,
                    max_vision             = 300,
                    max_vitality           = 150,
                    mother_pigeon_id       = null,
                    muscle                 = 150,
                    pigeon_id              = "08",
                    pigeon_name            = "Test8",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
                new() {
                    age                    = 0,
                    age_feature            = 0,
                    breed_id               = null,
                    breed_name             = null,
                    constitution           = 200,
                    exp                    = 10,
                    father_pigeon_id       = null,
                    fatigue                = 20,
                    feather_quality        = 250,
                    feather_size           = 200,
                    feature_a              = 0,
                    feature_b              = 0,
                    feature_c              = 0,
                    free_points            = 0,
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 0,
                    level                  = 0,
                    max_constitution       = 960,
                    max_feather_quality    = 300,
                    max_feather_size       = 400,
                    max_iq                 = 0,
                    max_muscle             = 0,
                    max_speed              = 0,
                    max_vision             = 0,
                    max_vitality           = 0,
                    mother_pigeon_id       = null,
                    muscle                 = 0,
                    pigeon_id              = "09",
                    pigeon_name            = "Test9",
                    pigeon_pigeonry        = null,
                    speed                  = 0,
                    vision                 = 0,
                    vitality               = 0
                },
            };
        }

        #endregion
    }
}