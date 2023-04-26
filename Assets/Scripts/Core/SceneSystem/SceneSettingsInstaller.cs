using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Core.SceneSystem
{
    [CreateAssetMenu(fileName = "SceneSettings", menuName = "Soap/SceneSettings")]
    public class SceneSettingsInstaller : ScriptableObjectInstaller<SceneSettingsInstaller>
    {
        public Settings settings;
        
        public override void InstallBindings()
        {
            Container.BindInstance(settings).IfNotBound();
        }
        
        [Serializable]
        public class Settings
        {
            public AssetReference[] sceneAssets;
        }
    }
}