using UnityEngine;
using Zenject;

namespace Core.Pigeon.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/PigeonData", fileName = "PigeonData")]
    public class PigeonDataScriptableObject : ScriptableObjectInstaller<PigeonDataScriptableObject>
    {
    }
}