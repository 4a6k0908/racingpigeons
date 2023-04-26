using Zenject;

namespace Core.Title
{
    public enum State
    {
        Title    = 0,
        Login    = 1,
        AwsLogin = 2,
    }

    public class TitleStateHandler
    {
        private readonly SignalBus signalBus; // 事件發送器

        private State currentState = State.Title;

        public TitleStateHandler(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        // 更改事件
        public void ChangeState(State nextState)
        {
            if (currentState == nextState)
                return;

            State preState = currentState;

            currentState = nextState;

            signalBus.Fire(new OnTitleStateChange(preState, nextState)); // 發送更改狀態事件
        }

        // 取得當前狀態
        public State GetCurrentState()
        {
            return currentState;
        }
    }
}