using Core.SceneSystem;
using UnityEngine;
using Zenject;

// 在一開始讀取初始場景並銷毀
public class Bootstrap : MonoBehaviour
{
    private ISceneService sceneService;

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