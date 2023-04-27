namespace Core.SceneSystem
{
    public class SceneService : ISceneService
    {
        private readonly SceneLoadHandler loadHandler;

        public SceneService(SceneLoadHandler loadHandler)
        {
            this.loadHandler = loadHandler;
        }

        public void DoLoadScene(int sceneIndex, bool IsFadeOut = true) => loadHandler.LoadScene(sceneIndex, IsFadeOut); // 更換場景
        public void DoFadeOut() => loadHandler.FadeOut(); // 淡出讀取畫面
    }
}