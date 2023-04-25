using Cinemachine;
using UnityEngine;

namespace Core.CameraSystem
{
    public enum CameraViewType
    {
        LobbyNormal = 0,
        LobbyCage   = 1
    }

    public class CameraFollowHandler
    {
        private readonly CameraView               cameraView;
        private readonly CinemachineVirtualCamera virtualCamera;

        private CameraViewType cameraViewType = CameraViewType.LobbyNormal;

        public CameraFollowHandler(CameraView cameraView)
        {
            this.cameraView      = cameraView;
            virtualCamera        = cameraView.GetVirtualCamera();
            virtualCamera.Follow = this.cameraView.GetFollowTrans(0);
        }

        public void ChangeViewType(CameraViewType viewType)
        {
            cameraViewType = viewType;

            virtualCamera.Follow = cameraView.GetFollowTrans((int)cameraViewType);
        }

        public CameraViewType GetCurrentViewType() => cameraViewType;

        public Transform GetCurrentFollowTrans() => virtualCamera.Follow;
    }
}