using Core.Title;
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