using System;
using AnimeTask;
using UnityEngine;
using Animator = UnityEngine.Animator;
using Random = UnityEngine.Random;

namespace Core.Effects.Lobby.Show
{
    public class PigeonRoofAndGroundShow : MonoBehaviour
    {
        // 表演項目
        private enum ShowType
        {
            Walk        = 0,
            WingWave    = 1,
            WingRefresh = 2,
            Eat         = 3,
            LookAround  = 4
        }

        // Animator 參數
        private const string ANIMATOR_WALK         = "IsWalking";
        private const string ANIMATOR_WING_WAVE    = "WingWave";
        private const string ANIMATOR_WING_REFRESH = "WingRefresh";
        private const string ANIMATOR_EAT          = "Eat";
        private const string ANIMATOR_LOOK_AROUND  = "LookAround";

        private float nextAnimationTime; // 播放下一次動畫的時間

        [SerializeField] private Percent percent; // 決定播哪個動畫

        [SerializeField] private Animator pigeonAnim; // 鴿子的 Animator
        [SerializeField] private string   raycastTag;
        [SerializeField] private Vector3  raycastOriginPos;
        [SerializeField] private Vector2  raycastRegion;

        [SerializeField] private Vector2 animationRate; // 幾秒播放下一次動畫

        private void Awake()
        {
            nextAnimationTime = Time.realtimeSinceStartup + Random.Range(animationRate.x, animationRate.y);

            Animator.StringToHash(ANIMATOR_WALK);
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
                case ShowType.Walk:
                    Move();
                    break;
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
        
        private async void Move()
        {
            pigeonAnim.SetBool(ANIMATOR_WALK, true);
            
            var movePosition = GetMovePosition();

            Vector3 dir = movePosition - transform.position;

            // 轉向
            dir.y                   = 0;
            transform.localRotation = Quaternion.LookRotation(dir);

            await Easing.Create<Linear>(movePosition, Vector3.Distance(transform.position, movePosition) * 1.5f).ToGlobalPosition(transform);
            
            pigeonAnim.SetBool(ANIMATOR_WALK, false);
        }

        private Vector3 GetMovePosition()
        {
            var originPos = raycastOriginPos + new Vector3(Random.Range(-raycastRegion.x, raycastRegion.x), 0, Random.Range(-raycastRegion.y, raycastRegion.y));
            
            if (Physics.Raycast(originPos, Vector3.down, out RaycastHit hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("ShowCollider")))
            {
                return hit.collider.CompareTag(raycastTag)
                    ? hit.point
                    : transform.position;
            }

            return transform.position;
        }

        [Serializable]
        private class Percent
        {
            public int walk        = 20;
            public int wingWave    = 20;
            public int wingRefresh = 20;
            public int eat         = 20;
            public int lookAround  = 20;

            public ShowType GetShowType()
            {
                int random = Random.Range(1, 101);

                ShowType type;

                if (random > 0 && random <= walk)
                {
                    type = ShowType.Walk;
                }
                else if (random > walk && random <= walk + wingWave)
                {
                    type = ShowType.WingWave;
                }
                else if (random > walk + wingWave && random <= walk + wingWave + wingRefresh)
                {
                    type = ShowType.WingRefresh;
                }
                else if (random > walk + wingWave + wingRefresh && random <= walk + wingWave + wingRefresh + eat)
                {
                    type = ShowType.Eat;
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