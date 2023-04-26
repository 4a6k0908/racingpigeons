using Core.Aws;
using Core.CameraSystem;
using Core.NotifySystem;
using Core.Player.Models;
using Core.SceneSystem;
using Core.SoundSystem;
using Zenject;

namespace Core.Installers
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            BindScene();
            BindAws();
            BindPlayer();
            BindSound();
            BindNotify();
            BindCamera();
        }

        private void BindCamera()
        {
            Container.Bind<CameraView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraFollowHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();
        }

        private void BindSound()
        {
            Container.Bind<BGMHandler>().AsSingle();
            Container.Bind<EffectHandler>().AsSingle();
            Container.Bind<LoopHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundService>().AsSingle();
            Container.Bind<SoundView>().FromComponentInHierarchy().AsSingle();
        }

        private void BindNotify()
        {
            Container.Bind<NotifyView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<NotifyService>().AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<PlayerData>().AsSingle();
        }

        private void BindAws()
        {
            Container.Bind<AwsGraphQL>().AsSingle();
        }

        private void BindScene()
        {
            Container.Bind<SceneStateHandler>().AsSingle();
            Container.Bind<SceneLoadHandler>().AsSingle();
            Container.Bind<SceneView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
        }
    }
}