using UnityEngine;
using Zenject;

namespace Core.Effects
{
    public class PigeonView : MonoBehaviour
    {
        [SerializeField] private Animator  pigeonAnim;
        [SerializeField] private Transform followerTrans;

        public Transform GetTrans() => transform;
        public Animator GetPigeonAnim() => pigeonAnim;
        public Transform GetFollowerTrans() => followerTrans;
        
        public class Factory : PlaceholderFactory<PigeonView> { }
    }
}