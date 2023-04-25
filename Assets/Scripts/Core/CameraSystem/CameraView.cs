using Cinemachine;
using UnityEngine;

namespace Core.CameraSystem
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform[]              followTrans;

        public CinemachineVirtualCamera GetVirtualCamera() => virtualCamera;
        
        public Transform GetFollowTrans(int index) => followTrans[index];
    }
}