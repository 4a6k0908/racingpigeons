using Core.TitleScene;
using Zenject;

namespace Core.Installers
{
    // 安裝 Title 場景的相關 class
    public class TitleInstaller: MonoInstaller<TitleInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnTitleStateChange>();

            Container.Bind<TitleStateHandler>().AsSingle();
        }
    }
}