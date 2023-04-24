using Zenject;

namespace Core.Title
{
    public enum State
    {
        Title    = 0,
        Login    = 1,
        AwsLogin = 2,
    }

    public class StateHandler
    {
        private readonly SignalBus signalBus;

        private State currentState = State.Title;

        public StateHandler(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        public void ChangeState(State nextState)
        {
            if (currentState == nextState)
                return;

            State preState = currentState;

            currentState = nextState;

            signalBus.Fire(new OnStateChange(preState, nextState));
        }

        public State GetCurrentState()
        {
            return currentState;
        }
    }
}