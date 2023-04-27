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


        // TODO: Delete this After test
        #region For Test Need To Delete

        private ICameraService cameraService;

        [Inject]
        private void Inject(ICameraService cameraService)
        {
            this.cameraService = cameraService;
        }
        
        private void Update()
        {
            if (Keyboard.current.numpad1Key.isPressed)
            {
                cameraService.ChangePigeonHouseView(CameraViewType.LobbyNormal);
            }
            else if (Keyboard.current.numpad2Key.isPressed)
            {
                cameraService.ChangePigeonHouseView(CameraViewType.LobbyCage);
            }
        }

        #endregion
    }
}