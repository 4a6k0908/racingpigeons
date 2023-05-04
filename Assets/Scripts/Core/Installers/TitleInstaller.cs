using Core.TitleScene;
using Zenject;

namespace Core.Installers
{
    // 安裝 Title 場景的相關 class
    public class TitleInstaller: MonoInstaller<TitleInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<TitleStateHandler>().AsSingle();
        }
    }
}