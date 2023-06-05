using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Effects.Lobby.Show
{
    public class PigeonPaddleShow : MonoBehaviour
    {
        // 表演項目
        private enum ShowType
        {
            WingWave    = 0,
            Eat         = 1,
            WingRefresh = 2,
            LookAround  = 3
        }

        // Animator 參數
        private const string ANIMATOR_WING_WAVE    = "WingWave";
        private const string ANIMATOR_WING_REFRESH = "WingRefresh";
        private const string ANIMATOR_EAT          = "Eat";
        private const string ANIMATOR_LOOK_AROUND  = "LookAround";

        private float nextAnimationTime; // 播放下一次動畫的時間

        [SerializeField] private Percent percent; // 決定播哪個動畫

        [SerializeField] private Animator pigeonAnim;
        [SerializeField] private Vector2 animationRate; // 幾秒播放下一次動畫
        
        private void Awake()
        {
            nextAnimationTime = Time.realtimeSinceStartup + Random.Range(animationRate.x, animationRate.y);

            Animator.StringToHash(ANIMATOR_WING_WAVE);
            Animator.StringToHash(ANIMATOR_WING_REFRESH);
            Animator.StringToHash(ANIMATOR_EAT);
            Animator.StringToHash(ANIMATOR_LOOK_AROUND);
        }

        private void Update()
        {
            // TODO: 待優化 
            if (!(Time.realtimeSinceStartup >= nextAnimationTime))
                return;

            nextAnimationTime = Time.realtimeSinceStartup + Random.Range(animationRate.x, animationRate.y);

            ShowType type = percent.GetShowType();

            switch (type)
            {
                case ShowType.WingWave:
                    pigeonAnim.SetTrigger(ANIMATOR_WING_WAVE);
                    break;
                case ShowType.WingRefresh:
                    pigeonAnim.SetTrigger(ANIMATOR_WING_REFRESH);
                    break;
                case ShowType.Eat:
                    pigeonAnim.SetTrigger(ANIMATOR_EAT);
                    break;
                case ShowType.LookAround:
                    pigeonAnim.SetTrigger(ANIMATOR_LOOK_AROUND);
                    break;
            } 
        }

        [Serializable]
        private class Percent
        {
            public int wingWave    = 25;
            public int eat         = 25;
            public int wingRefresh = 25;
            public int lookAround  = 25;

            public ShowType GetShowType()
            {
                int random = Random.Range(1, 101);

                ShowType type;

                if (random > 0 && random <= wingWave)
                {
                    type = ShowType.WingWave;
                }
                else if (random > wingWave && random <= wingWave + eat)
                {
                    type = ShowType.Eat;
                }
                else if (random > wingWave + eat && random <= wingWave + eat + wingRefresh)
                {
                    type = ShowType.WingRefresh;
                }
                else
                {
                    type = ShowType.LookAround;
                }

                return type;
            }
        }
    }
}