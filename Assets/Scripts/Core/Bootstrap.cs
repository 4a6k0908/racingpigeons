using SoapUtils.SceneSystem;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Inject] private ISceneService sceneService;

    [Inject]
    private void Inject(ISceneService sceneService)
    {
        this.sceneService = sceneService;
    }
    
    private void Start()
    {
        sceneService.DoLoadScene(0);
        
        Destroy(gameObject);
    }
}