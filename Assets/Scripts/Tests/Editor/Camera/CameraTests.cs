using Core.CameraSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.Editor.Camera
{
    [TestFixture]
    public class CameraTests
    {
        [SetUp]
        public void SetUp()
        {
            cameraView        = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/CameraView.prefab").GetComponent<CameraView>();
            cameraViewHandler = new CameraFollowHandler(cameraView);
            cameraService     = new CameraService(cameraViewHandler);
        }

        private CameraView          cameraView;
        private CameraFollowHandler cameraViewHandler;
        private CameraService       cameraService;

        [Test]
        public void _01_Should_Call_Change_Camera_View_Success()
        {
            cameraService.DoChangePigeonHouseView(CameraViewType.LobbyCage);

            CameraViewTypeShouldBe(CameraViewType.LobbyCage);
            FollowTransShouldBe(cameraView.GetFollowTrans(1).name);
        }

        [Test]
        public void _02_Should_Call_Change_Camera_View_Sequence_Success()
        {
            cameraService.DoChangePigeonHouseView();
            CameraViewTypeShouldBe(CameraViewType.LobbyCage);
            FollowTransShouldBe(cameraView.GetFollowTrans(1).name);
            
            cameraService.DoChangePigeonHouseView();
            CameraViewTypeShouldBe(CameraViewType.LobbyNormal);
            FollowTransShouldBe(cameraView.GetFollowTrans(0).name);
        }


        private void FollowTransShouldBe(string expected)
        {
            Assert.AreEqual(expected, cameraViewHandler.GetCurrentFollowTrans().name);
        }

        private void CameraViewTypeShouldBe(CameraViewType expected)
        {
            Assert.AreEqual(expected, cameraViewHandler.GetCurrentViewType());
        }
    }
}