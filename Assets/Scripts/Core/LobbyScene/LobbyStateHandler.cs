using Zenject;

namespace Core.LobbyScene
{
    public enum LobbyState
    {
        Lobby      = 0,
        PigeonList = 1,
    }

    public class LobbyStateHandler
    {
        private readonly SignalBus signalBus;

        private LobbyState preState     = LobbyState.Lobby;
        private LobbyState currentState = LobbyState.Lobby;

        public LobbyStateHandler(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        public void ChangeState(LobbyState nextState)
        {
            if (currentState == nextState)
                return;

            preState     = currentState;
            currentState = nextState;

            signalBus.Fire(new OnLobbyStateChange(preState, currentState));
        }
    }
}