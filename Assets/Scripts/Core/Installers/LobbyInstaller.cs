using Core.LobbyScene;
using Zenject;

namespace Core.Installers
{
    public class LobbyInstaller : MonoInstaller<LobbyInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<LobbyStateHandler>().AsSingle();
        }
    }
}