using System;
using UnityEngine.UI.Extensions;

namespace Core.UI.Lobby.PigeonList
{
    public class Context : FancyScrollRectContext
    {
        public int         selectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}