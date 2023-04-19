using Core.Database;
using Core.User.Models;
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
        }

        private void BindAws()
        {
            Container.Bind<AwsGraphQL>().AsSingle();
            Container.Bind<AwsUserModel>().AsSingle();
        }

        private void BindScene()
        {
            Container.Bind<StateHandler>().AsSingle();
            Container.Bind<LoadHandler>().AsSingle();
            Container.Bind<SceneView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
        }
    }
}