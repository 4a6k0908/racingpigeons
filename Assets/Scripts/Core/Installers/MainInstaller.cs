using Core.Database;
using Core.Player.Models;
using SoapUtils.SceneSystem;
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