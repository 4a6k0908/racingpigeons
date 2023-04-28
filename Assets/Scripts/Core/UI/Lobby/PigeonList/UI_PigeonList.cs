using System;
using System.Threading;
using AnimeTask;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.UI.Lobby.PigeonList
{
    public enum PigeonListMode
    {
        List       = 0,
        HalfPigeon = 1,
        FullPigeon = 2
    }

    public class UI_PigeonList : MonoBehaviour
    {
        private PigeonListMode currentMode = PigeonListMode.List;

        [SerializeField] private UI_PigeonStat_Scroller pigeonStatScroller;
        [SerializeField] private RectTransform          pigeonBookTrans;

        [SerializeField] private GameObject toExpandBtnObj;
        [SerializeField] private GameObject toListBtnObj;

        private CancellationTokenSource pigeonBookTaskToken;

        public void Button_To_Half()
        {
            ChangeMode(PigeonListMode.HalfPigeon);
        }

        public void Button_To_Full()
        {
            ChangeMode(PigeonListMode.FullPigeon);
        }

        public void Button_To_List()
        {
            ChangeMode(PigeonListMode.List);
        }

        private void ChangeMode(PigeonListMode mode)
        {
            if (currentMode == mode)
                return;

            currentMode = mode;

            pigeonBookTaskToken?.Cancel();
            pigeonBookTaskToken = new CancellationTokenSource();

            switch (currentMode)
            {
                case PigeonListMode.List:
                    Easing.Create<OutQuint>(20, 0.5f).ToAnchoredPositionX(pigeonBookTrans).ToCancellationToken(pigeonBookTaskToken.Token);
                    break;
                case PigeonListMode.HalfPigeon:
                    Easing.Create<OutQuint>(700, 0.5f).ToAnchoredPositionX(pigeonBookTrans).ToCancellationToken(pigeonBookTaskToken.Token);
                    break;
                case PigeonListMode.FullPigeon:
                    Easing.Create<OutQuint>(2000, 0.5f).ToAnchoredPositionX(pigeonBookTrans).ToCancellationToken(pigeonBookTaskToken.Token);
                    break;
            }

            toExpandBtnObj.SetActive(currentMode == PigeonListMode.HalfPigeon);
            toListBtnObj.SetActive(currentMode == PigeonListMode.HalfPigeon);
        }
    }
}