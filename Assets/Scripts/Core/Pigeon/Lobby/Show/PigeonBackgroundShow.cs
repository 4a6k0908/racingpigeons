using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Pigeon.Lobby.Show
{
    public class PigeonBackgroundShow : MonoBehaviour
    {
        // 表演項目
        private enum ShowType 
        {
            Left_To_Right = 0,
            None          = 1,
            Right_To_Left = 2
        }

        // Animator 的參數
        private const string PATH_ANIMATOR_PATH_LEFT  = "StartLeft";
        private const string PATH_ANIMATOR_PATH_RIGHT = "StartRight";
        private const string PIGEON_ANIMATOR_FLY      = "IsFlying";

        private Vector3 originPos; // 原始位置
        private float   nextAnimationTime; // 播放下一次動畫的時間

        [SerializeField] private Percent percent; // 決定播哪個動畫

        [SerializeField] private Animator pathAnim; // 路徑的 Animator
        [SerializeField] private Animator pigeonAnim; // 鴿子的 Animator

        [SerializeField] private Vector2 animationRate; // 幾秒播放下一次動畫
        [SerializeField] private Vector2 yPosRange; // 高度 Offset

        private void Awake()
        {
            originPos = transform.position;

            nextAnimationTime = Time.realtimeSinceStartup + Random.Range(animationRate.x, animationRate.y);

            transform.localPosition = originPos + new Vector3(0, Random.Range(yPosRange.x, yPosRange.y), 0);

            Animator.StringToHash(PATH_ANIMATOR_PATH_LEFT);
            Animator.StringToHash(PATH_ANIMATOR_PATH_RIGHT);
            Animator.StringToHash(PIGEON_ANIMATOR_FLY);
        }

        private void Update()
        {
            // TODO: 待優化
            if (!(Time.realtimeSinceStartup >= nextAnimationTime))
                return;

            nextAnimationTime = Time.realtimeSinceStartup + Random.Range(animationRate.x, animationRate.y);

            transform.localPosition = originPos + new Vector3(0, Random.Range(yPosRange.x, yPosRange.y), 0);

            ShowType type = percent.GetShowType();

            switch (type)
            {
                case ShowType.Left_To_Right:
                    PlayPathAnimation(false);
                    break;
                case ShowType.None:
                    break;
                case ShowType.Right_To_Left:
                    PlayPathAnimation(true);
                    break;
            }
        }

        // 開始播放動畫
        private void PlayPathAnimation(bool IsFromRight)
        {
            pathAnim.SetTrigger(IsFromRight ? PATH_ANIMATOR_PATH_RIGHT : PATH_ANIMATOR_PATH_LEFT);
            pigeonAnim.transform.parent.localScale = new Vector3(IsFromRight ? 1 : -1, 1, 1);
            pigeonAnim.transform.parent.gameObject.SetActive(true);
            pigeonAnim.SetBool(PIGEON_ANIMATOR_FLY, true);
        }

        // 動畫結束事件
        public void Animation_End()
        {
            pigeonAnim.SetBool(PIGEON_ANIMATOR_FLY, false);
            pigeonAnim.transform.parent.gameObject.SetActive(false);
        }

        [Serializable]
        private class Percent
        {
            public int leftToRight = 30;
            public int none          = 40;
            public int rightToLeft = 30;

            public ShowType GetShowType()
            {
                int random = Random.Range(1, 101);

                ShowType type;

                if (random > 0 && random <= leftToRight)
                {
                    type = ShowType.Left_To_Right;
                }
                else if (random > leftToRight && random <= leftToRight + none)
                {
                    type = ShowType.None;
                }
                else
                {
                    type = ShowType.Right_To_Left;
                }

                return type;
            }
        }
    }
}