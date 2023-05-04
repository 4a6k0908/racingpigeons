namespace Core.TitleScene
{
    // Title 場景的遊戲狀態事件結構
    public struct OnTitleStateChange
    {
        public readonly TitleState preState;
        public readonly TitleState state;

        public OnTitleStateChange(TitleState preState, TitleState state)
        {
            this.preState = preState;
            this.state    = state;
        }
    }
}