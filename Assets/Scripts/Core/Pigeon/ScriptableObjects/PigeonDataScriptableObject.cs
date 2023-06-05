using UnityEngine;
using Zenject;

namespace Core.Effects.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/PigeonData", fileName = "PigeonData")]
    public class PigeonDataScriptableObject : ScriptableObjectInstaller<PigeonDataScriptableObject>
    {
    }
}