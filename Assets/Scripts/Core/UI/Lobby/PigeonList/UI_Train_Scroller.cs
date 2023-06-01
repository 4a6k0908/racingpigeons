using System;
using System.Collections.Generic;
using System.Linq;
using Core.Effects.Lobby;
using Core.Effects.Models;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI.Extensions.EasingCore;
using static Core.Effects.Models.EffectModel.GQL_GetEffectList.Data;

namespace Core.UI.Lobby.Train
{
    public class UI_Train_Scroller : FancyScrollRect<Effect, Context>
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

            //pigeon3DRenderTexture = FindObjectOfType<Pigeon3DRenderTexture>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            //Context.OnCellClicked += OnCellClick;
        }


        public void UpdateData(List<Effect> effects)
        {
            UpdateContents(effects);
            scroller.SetTotalCount(effects.Count);

            JumpTo(0);

            Debug.Log("FinishInit");
        }

        public void ScrollTo(int index)
        {
            ScrollTo(index, 0.25f, Ease.InOutQuint);
        }

        public void JumpTo(int index)
        {
            JumpTo(index, 0.5f);
        }
        /*
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
        */
    }
}