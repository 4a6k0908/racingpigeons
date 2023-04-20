using Core.Aws;
using Core.Player.Models;
using SoapUtils.NotifySystem;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
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