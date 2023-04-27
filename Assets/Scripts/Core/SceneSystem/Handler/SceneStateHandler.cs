namespace Core.SceneSystem
{
    public enum SceneState
    {
        Complete  = 0,
        Loading   = 1,
        Unloading = 2
    }

    // 更換場景的 State 處理
    public class SceneStateHandler
    {
        private SceneState state = SceneState.Complete;
        public SceneState GetState() => state;

        public void ChangeState(SceneState state) => this.state = state;
    }
}