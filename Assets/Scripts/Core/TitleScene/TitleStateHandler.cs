using Zenject;

namespace Core.TitleScene
{
    public enum TitleState
    {
        Title    = 0,
        Login    = 1,
        AwsLogin = 2,
    }

    public class TitleStateHandler
    {
        private readonly SignalBus signalBus; // 事件發送器

        private TitleState currentState = TitleState.Title;

        public TitleStateHandler(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        // 更改狀態並發送事件
        public void ChangeState(TitleState nextState)
        {
            if (currentState == nextState)
                return;

            TitleState preState = currentState;

            currentState = nextState;

            signalBus.Fire(new OnTitleStateChange(preState, nextState)); // 發送更改狀態事件
        }

        // 取得當前狀態
        public TitleState GetCurrentState()
        {
            return currentState;
        }
    }
}