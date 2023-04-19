using Zenject;

namespace Core.Title
{
    public enum State
    {
        Title = 0,
        Login = 1,
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
            if(currentState == nextState)
                return;

            signalBus.Fire(new OnStateChange(currentState, nextState));
            
            currentState = nextState;
        }

        public State GetCurrentState()
        {
            return currentState;
        }
    }
}