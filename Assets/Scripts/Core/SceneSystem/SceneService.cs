using Zenject;

namespace Core.SceneSystem
{
    public class SceneService : ISceneService
    {
        [Inject] private readonly SceneLoadHandler loadHandler;

        public void DoLoadScene(int sceneIndex, bool IsFadeOut = true) => loadHandler.LoadScene(sceneIndex, IsFadeOut);
        public void DoFadeOut() => loadHandler.FadeOut();
    }
}