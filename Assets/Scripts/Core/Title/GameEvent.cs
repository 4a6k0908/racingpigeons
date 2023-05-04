namespace Core.Title
{
    // Title 場景的遊戲狀態事件結構
    public struct OnTitleStateChange
    {
        public readonly TitleState PreTitleState;
        public readonly TitleState TitleState;

        public OnTitleStateChange(TitleState preTitleState, TitleState titleState)
        {
            this.PreTitleState = preTitleState;
            this.TitleState    = titleState;
        }
    }
}