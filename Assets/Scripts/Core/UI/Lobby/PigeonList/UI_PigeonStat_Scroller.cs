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
    }
}