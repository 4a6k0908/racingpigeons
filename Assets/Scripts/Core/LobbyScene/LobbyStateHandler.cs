using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.LobbyScene
{
    public enum LobbyState
    {
        Lobby      = 0,
        PigeonList = 1,
        Race       = 5,
    }

    public class LobbyStateHandler
    {
        private readonly SignalBus signalBus;

        private LobbyState        currentState     = LobbyState.Lobby;
        private Queue<LobbyState> allPreStateQueue = new Queue<LobbyState>();

        public LobbyStateHandler(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        public void ChangeState(LobbyState nextState)
        {
            if (currentState == nextState)
                return;

            LobbyState preState = currentState;
            currentState = nextState;

            allPreStateQueue.Enqueue(preState);
            if (currentState == LobbyState.Lobby)
                allPreStateQueue.Clear();

            signalBus.Fire(new OnLobbyStateChange(preState, currentState));
        }

        public void ChangeToPreState()
        {
            var nextState = allPreStateQueue.Dequeue();

            LobbyState preState = currentState;
            currentState = nextState;

            signalBus.Fire(new OnLobbyStateChange(preState, currentState));
        }
    }
}