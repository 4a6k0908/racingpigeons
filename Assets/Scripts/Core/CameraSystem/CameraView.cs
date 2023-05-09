using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.CameraSystem
{
    // 取得跟隨、虛擬攝影機的物件
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform[]              followTrans;

        public CinemachineVirtualCamera GetVirtualCamera() => virtualCamera;

        public Transform GetFollowTrans(int index) => followTrans[index];
    }
}