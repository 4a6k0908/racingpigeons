namespace Core.Title
{
    public struct OnStateChange
    {
        public readonly State preState;
        public readonly State state;

        public OnStateChange(State preState, State state)
        {
            this.preState = preState;
            this.state    = state;
        }
    }
}