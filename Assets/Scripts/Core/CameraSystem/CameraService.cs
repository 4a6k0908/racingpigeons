namespace Core.CameraSystem
{
    public class CameraService : ICameraService
    {
        private readonly CameraFollowHandler cameraFollowHandler;

        public CameraService(CameraFollowHandler cameraFollowHandler)
        {
            this.cameraFollowHandler = cameraFollowHandler;
        }

        public void DoChangePigeonHouseView(CameraViewType viewType) => cameraFollowHandler.ChangeViewType(viewType);
        public void DoChangePigeonHouseView() => cameraFollowHandler.ChangeViewType();
    }
}