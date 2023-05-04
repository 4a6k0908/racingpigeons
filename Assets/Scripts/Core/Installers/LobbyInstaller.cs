using Core.Lobby;
using Zenject;

namespace Core.Installers
{
    public class LobbyInstaller : MonoInstaller<LobbyInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnLobbyStateChange>();
            
            Container.Bind<LobbyStateHandler>().AsSingle();
        }
    }
}