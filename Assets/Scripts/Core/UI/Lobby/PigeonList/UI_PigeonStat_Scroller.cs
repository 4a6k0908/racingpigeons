using System;
using System.Collections.Generic;
using Core.Pigeon.Models;
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

        private void Awake()
        {
            Relayout();
        }

        private void Start()
        {
            UpdateData(GetTestPigeons());
        }

        public void UpdateData(IList<PigeonStat> pigeonStats)
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
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 50,
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
                    iq                     = 50,
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
                    muscle                 = 100,
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
                    iq                     = 50,
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
                    gender                 = 1,
                    grand_father_pigeon_id = null,
                    grand_mother_pigeon_id = null,
                    iq                     = 50,
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
                    muscle                 = 100,
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
                    pigeon_name            = "Test6",
                    pigeon_pigeonry        = null,
                    speed                  = 20,
                    vision                 = 30,
                    vitality               = 43
                },
            };
        }

        #endregion
    }
}