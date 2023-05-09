using System;
using System.Collections.Generic;
using System.Linq;
using Core.Pigeon.Models;
using Core.Player.Models;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI.Extensions.EasingCore;
using Zenject;

namespace Core.UI.Lobby.PigeonList
{
    public class UI_PigeonStat_Scroller : FancyScrollRect<PigeonStat, Context>
    {
        [SerializeField] private Scroller   scroller;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float      cellSize;

        protected override GameObject CellPrefab => cellPrefab;
        protected override float      CellSize   => cellSize;

        private List<PigeonStat> originPigeonStatList = new();

        private void Awake()
        {
            Relayout();
        }

        // private void Start()
        // {
            // TODO: 之後要換成 PlayerData 來
            // originPigeonStatList = GetTestPigeons();
            // ChangePresent(PigeonListFilter.None, PigeonListSort.IQ, PigeonListOrder.Descending);
        // }

        public void SetOriginData(List<PigeonStat> pigeonStats)
        {
            originPigeonStatList = pigeonStats;
            
            Debug.Log("Set Origin");
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

        // 更改排序、過濾
        public void ChangePresent(PigeonListFilter filter, PigeonListSort sort, PigeonListOrder order)
        {
            Debug.Log("Start filter");
            
            // TODO: 這方法是先以有為主，後續可更換演算法
            var pigeonStats = new List<PigeonStat>();

            // 過濾
            switch (filter)
            {
                case PigeonListFilter.None:
                case PigeonListFilter.Feral:
                case PigeonListFilter.Favorite:
                    pigeonStats = originPigeonStatList;
                    break;
                case PigeonListFilter.Male:
                    pigeonStats.AddRange(originPigeonStatList.Where(pigeonStat => pigeonStat.gender == 0));
                    break;
                case PigeonListFilter.Female:
                    pigeonStats.AddRange(originPigeonStatList.Where(pigeonStat => pigeonStat.gender == 1));
                    break;
            }


            if (pigeonStats.Count > 0)
            {
                // 依能力排序
                switch (sort)
                {
                    case PigeonListSort.IQ:
                    case PigeonListSort.Sick:
                    case PigeonListSort.Other:
                        pigeonStats = pigeonStats.OrderByDescending(pigeonStat => pigeonStat.iq).ToList();
                        break;
                    case PigeonListSort.Vision:
                        pigeonStats = pigeonStats.OrderByDescending(pigeonStat => pigeonStat.vision).ToList();
                        break;
                    case PigeonListSort.Speed:
                        pigeonStats = pigeonStats.OrderByDescending(pigeonStat => pigeonStat.speed).ToList();
                        break;
                    case PigeonListSort.FeatherSize:
                        pigeonStats = pigeonStats.OrderByDescending(pigeonStat => pigeonStat.feather_size).ToList();
                        break;
                    case PigeonListSort.Vitality:
                        pigeonStats = pigeonStats.OrderByDescending(pigeonStat => pigeonStat.vitality).ToList();
                        break;
                    case PigeonListSort.Muscle:
                        pigeonStats = pigeonStats.OrderByDescending(pigeonStat => pigeonStat.muscle).ToList();
                        break;
                    case PigeonListSort.FeatherQuality:
                        pigeonStats = pigeonStats.OrderByDescending(pigeonStat => pigeonStat.feather_quality).ToList();
                        break;
                }

                if (order == PigeonListOrder.Ascending)
                    pigeonStats.Reverse();
            }

            UpdateData(pigeonStats);
        }

        // TODO: 測試專用，之後刪掉

        #region Test

        public List<PigeonStat> GetTestPigeons()
        {
            return new List<PigeonStat> {
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
                    pigeon_id              = null,
                    pigeon_name            = "Test",
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
                    pigeon_id              = null,
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
                    pigeon_id              = null,
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
                    pigeon_id              = null,
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
                    pigeon_id              = null,
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
                    pigeon_id              = null,
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
                    pigeon_id              = null,
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
                    pigeon_id              = null,
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
                    pigeon_id              = null,
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