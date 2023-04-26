using UnityEngine.AddressableAssets;

namespace Core.SceneSystem
{
    public interface ISceneService
    {
        void DoLoadScene(int sceneIndex, bool IsFadeOut = true);
        void DoFadeOut();
    }
}