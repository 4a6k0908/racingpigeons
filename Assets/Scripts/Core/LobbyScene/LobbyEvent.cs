namespace Core.LobbyScene
{
    public struct OnLobbyStateChange
    {
        public LobbyState preState;
        public LobbyState currentState;

        public OnLobbyStateChange(LobbyState preState, LobbyState currentState)
        {
            this.preState     = preState;
            this.currentState = currentState;
        }
    }
}