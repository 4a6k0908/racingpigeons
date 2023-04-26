namespace Core.Title
{
    // Title 場景的遊戲狀態事件結構
    public struct OnTitleStateChange
    {
        public readonly State preState;
        public readonly State state;

        public OnTitleStateChange(State preState, State state)
        {
            this.preState = preState;
            this.state    = state;
        }
    }
}