using System;
using Cinemachine;
using UnityEngine;

namespace Core.CameraSystem
{
    public enum CameraViewType
    {
        LobbyNormal = 0,
        LobbyCage   = 1
    }

    // 處理攝影機跟隨的狀態
    public class CameraFollowHandler
    {
        private readonly CameraView               cameraView;
        private readonly CinemachineVirtualCamera virtualCamera;

        private CameraViewType currentViewType = CameraViewType.LobbyNormal;

        public CameraFollowHandler(CameraView cameraView)
        {
            this.cameraView      = cameraView;
            virtualCamera        = cameraView.GetVirtualCamera();
            virtualCamera.Follow = this.cameraView.GetFollowTrans(0);
        }

        // 更改目前視角的類型
        public void ChangeViewType(CameraViewType viewType)
        {
            currentViewType = viewType;

            virtualCamera.Follow = cameraView.GetFollowTrans((int)currentViewType);
        }

        public void ChangeViewType()
        {
            switch (currentViewType)
            {
                case CameraViewType.LobbyNormal:
                    currentViewType = CameraViewType.LobbyCage;
                    break;
                case CameraViewType.LobbyCage:
                    currentViewType = CameraViewType.LobbyNormal;
                    break;
            }

            virtualCamera.Follow = cameraView.GetFollowTrans((int)currentViewType);
        }

        // 取得目前的視角類型
        public CameraViewType GetCurrentViewType() => currentViewType;

        // 取得目前跟隨的物件
        public Transform GetCurrentFollowTrans() => virtualCamera.Follow;
    }
}