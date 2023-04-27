namespace Core.SceneSystem
{
    public interface ISceneService
    {
        void DoLoadScene(int sceneIndex, bool IsFadeOut = true); // 更換場景
        void DoFadeOut(); // 淡出讀取畫面
    }
}