using UnityEngine;

namespace Core.UI
{
    public class UI_Pip : MonoBehaviour
    {
        private enum FocusMode
        {
            Map,
            Pigeon,
        }

        private FocusMode focusMode = FocusMode.Map;

        [SerializeField] private RectTransform childRectTrans;
        [SerializeField] private RectTransform parentRectTrans;

        [SerializeField] private RectTransform pigeonRTTrans;
        [SerializeField] private RectTransform mapRTTrans;

        public void Button_Change_Pip()
        {
            focusMode = (focusMode == FocusMode.Map) ? FocusMode.Pigeon : FocusMode.Map;

            switch (focusMode)
            {
                case FocusMode.Map:
                    SetParentScreen(mapRTTrans);
                    SetChildScreen(pigeonRTTrans);
                    break;
                case FocusMode.Pigeon:
                    SetParentScreen(pigeonRTTrans);
                    SetChildScreen(mapRTTrans);
                    break;
            }
        }

        private void SetParentScreen(RectTransform rectTransform)
        {
            rectTransform.SetParent(parentRectTrans);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
        }

        private void SetChildScreen(RectTransform rectTransform)
        {
            rectTransform.SetParent(childRectTrans);
            rectTransform.offsetMin = new Vector2(1, 1);
            rectTransform.offsetMax = new Vector2(-1, -1);
        }
    }
}