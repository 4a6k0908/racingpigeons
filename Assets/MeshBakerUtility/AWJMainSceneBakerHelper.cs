
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static AWJ_Core;

public class AWJMainSceneBakerHelper : MonoBehaviour
{

    public GameObject target;
    public AWJ_Core asjc;

    [Header("Output: expand the list and check result")]
    public Renderer[] errorObjects;
    public Renderer[] multMaterialObjects;
    public RendererGroup[] outputs;
    public Renderer[] isolateObjs;
    public MB3_MeshBaker meshBaker;

    public virtual void Init()
    {
        if (target == null)
            return;

        asjc = new AWJ_Core();
        Renderer[] rnds = target.GetComponentsInChildren<Renderer>();
        Debug.Log("find " + rnds.Length + " renderers");
        //MainSceneObject mainScene = target.GetComponent<MainSceneObject>();
        //Renderer[] sceneRnds = mainScene.GetStaticRenderers();
        //asjc.Sort(this.gameObject, sceneRnds);
        asjc.Sort(this.gameObject, rnds);
        outputs = asjc.GetResult();
        isolateObjs = asjc.GetIsolateRenderer();
        errorObjects = asjc.GetErrorRenderer();
        multMaterialObjects = asjc.GetMultMatRenderer();
        meshBaker = this.gameObject.GetComponent<MB3_MeshBaker>();
        meshBaker = (meshBaker) ? meshBaker : this.gameObject.AddComponent<MB3_MeshBaker>();
        meshBaker.hideFlags = HideFlags.HideInInspector;
    }

    public AWJ_Core GetInstant()
    {
        return asjc;
    }

    public GameObject GenerateScene()
    {
        //MainSceneObject mainScene = target.GetComponent<MainSceneObject>();
        //mainScene.SetCombinedObjects(asjc.GetDestroyedObjs());
        //mainScene.SetIsolateObjects(isolateObjs);
        GameObject gj = Instantiate(target);

        //mainScene = gj.GetComponent<MainSceneObject>();
        //mainScene.DestroyUselessGameObjects();
        gj.name = gj.name.Replace("(Clone)", "_Compress");
        return gj;
    }
}
