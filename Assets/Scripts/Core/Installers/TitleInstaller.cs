using Core.Aws.Login;
using Core.Aws.Models;
using Core.Title;
using Core.User.Models;
using Zenject;

namespace Core.Installers
{
    public class TitleInstaller: MonoInstaller<TitleInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnStateChange>();

            Container.Bind<StateHandler>().AsSingle();
        }
    }
}