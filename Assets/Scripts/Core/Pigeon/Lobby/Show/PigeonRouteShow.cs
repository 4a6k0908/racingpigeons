using UnityEngine;

namespace Core.Pigeon.Lobby.Show
{
    public class PigeonRouteShow : MonoBehaviour
    {
        [SerializeField] private Animator pigeonAnim;

        private const string ANIMATOR_FLY          = "IsFlying";
        private const string ANIMATOR_WING_WAVE    = "WingWave";
        private const string ANIMATOR_WING_REFRESH = "WingRefresh";
        private const string ANIMATOR_EAT          = "Eat";
        private const string ANIMATOR_LOOK_AROUND  = "LookAround";
        private const string ANIMATOR_WALK         = "IsWalking";

        private void Awake()
        {
            Animator.StringToHash(ANIMATOR_FLY);
            Animator.StringToHash(ANIMATOR_WALK);
            Animator.StringToHash(ANIMATOR_WING_WAVE);
            Animator.StringToHash(ANIMATOR_WING_REFRESH);
            Animator.StringToHash(ANIMATOR_EAT);
            Animator.StringToHash(ANIMATOR_LOOK_AROUND);
        }

        public void Animation_Takeoff()
        {
            pigeonAnim.SetBool(ANIMATOR_FLY, true);
        }

        public void Animation_Land()
        {
            pigeonAnim.SetBool(ANIMATOR_FLY, false);
        }

        public void Animation_LookAround()
        {
            pigeonAnim.SetTrigger(ANIMATOR_LOOK_AROUND);
        }

        public void Animation_Eat()
        {
            pigeonAnim.SetTrigger(ANIMATOR_EAT);
        }

        public void Animation_Walk()
        {
            pigeonAnim.SetBool(ANIMATOR_WALK, true);
        }

        public void Animation_WalkStop()
        {
            pigeonAnim.SetBool(ANIMATOR_WALK, false);
        }
    }
}