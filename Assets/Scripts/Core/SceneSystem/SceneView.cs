using AnimeTask;
using UniRx;
using UnityEngine;

namespace Core.SceneSystem
{
    // 取得 Unity 更換場景時的 UI 與設定效果等
    public class SceneView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup   canvasGroup;
        [SerializeField] private RectTransform loadingTrans;
        [SerializeField] private float         rotateSpeed;

        private CompositeDisposable updateDisposable;

        // 旋轉 Loading 圖片
        private void UpdateEvent(long frame)
        {
            loadingTrans.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime), Space.Self);
        }

        // 更換場景時旋轉 Loading 圖片
        public async void SetAppear(bool IsOn)
        {
            if (IsOn && loadingTrans != null)
            {
                updateDisposable = new CompositeDisposable();
                Observable.EveryUpdate().Subscribe(UpdateEvent).AddTo(updateDisposable);
            }

            canvasGroup.interactable = canvasGroup.blocksRaycasts = IsOn;

            await Easing.Create<Linear>(IsOn ? 1 : 0, 0.25f).ToColorA(canvasGroup);

            if (!IsOn)
                updateDisposable?.Dispose();
        }
    }
}