using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWJ_Core
{
    [System.Serializable]
    public struct RendererGroup
    {
        public Material revealMat;
        public List<Renderer> rnds;
        public List<GameObject> objects;
        public string boc;
    }
    RendererGroup[] outputs;
    List<RendererGroup> bakeOutputs;
    int bakeCount;
    public List<Renderer> errorObjects;
    public List<Renderer> multMaterialObjects;
    public List<Renderer> isolateObjs;
    public List<Renderer> destroyObjs;
    public void Sort(GameObject target, Renderer[] _sceneRnds)
    {
        List<Material> sceneMats = new List<Material>();
        Renderer[] sceneRnds = _sceneRnds;
        for (int i = 0; i < sceneRnds.Length; i++)
        {
            Material tmp = sceneRnds[i].sharedMaterial;
            if (!sceneMats.Contains(tmp))
                sceneMats.Add(tmp);
        }

        errorCheck(sceneRnds);

        outputs = new RendererGroup[sceneMats.Count];
        for (int i = 0; i < outputs.Length; i++)
        {
            outputs[i].rnds = new List<Renderer>();
            outputs[i].objects = new List<GameObject>();
        }
        for (int i = 0; i < sceneRnds.Length; i++)
        {
            Material tmp = sceneRnds[i].sharedMaterial;
            for (int j = 0; j < sceneMats.Count; j++)
            {
                if (tmp == sceneMats[j])
                {
                    outputs[j].revealMat = tmp;
                    outputs[j].rnds.Add(sceneRnds[i]);
                    outputs[j].objects.Add(sceneRnds[i].gameObject);
                    continue;
                }
            }
        }

        bakeOutputs = new List<RendererGroup>(outputs);
        isolateObjs = new List<Renderer>();
        for (int i = 0; i < bakeOutputs.Count; i++)
        {
            RendererGroup tmp = bakeOutputs[i];
            if (tmp.objects.Count == 1)
            {
                bakeOutputs.Remove(tmp);
                isolateObjs.AddRange(tmp.rnds);
            }
        }
        bakeCount = bakeOutputs.Count;
        destroyObjs = new List<Renderer>();
        for (int i = 0; i < bakeCount; i++)
            destroyObjs.AddRange(GetBakeOutput(i).rnds);
        Debug.Log("Eligible to combine total: " + destroyObjs.Count);
    }
    public void Sort(GameObject target, Renderer[] _sceneRnds,int boundLimit, Material specify = null)
    {
        List<Material> sceneMats = new List<Material>();
        List<string> sceneBocs = new List<string>();
        Renderer[] sceneRnds = _sceneRnds;
        for (int i = 0; i < sceneRnds.Length; i++)
        {
            Material t = sceneRnds[i].sharedMaterial;
            string boc = getBoc(sceneRnds[i].gameObject.GetComponent<MeshFilter>(), boundLimit);
            if (t != specify)
                boc = "0";
            string tmp = t + boc;

            if (!sceneBocs.Contains(tmp))
                sceneBocs.Add(tmp);
        }

        errorCheck(sceneRnds);

        outputs = new RendererGroup[sceneBocs.Count];
        for (int i = 0; i < outputs.Length; i++)
        {
            outputs[i].rnds = new List<Renderer>();
            outputs[i].objects = new List<GameObject>();
        }
        for (int i = 0; i < sceneRnds.Length; i++)
        {
            Material t = sceneRnds[i].sharedMaterial;
            string boc = getBoc(sceneRnds[i].gameObject.GetComponent<MeshFilter>(), boundLimit);
            if (t != specify)
                boc = "0";
            string tmp = t + boc;

            for (int j = 0; j < sceneBocs.Count; j++)
            {
                if (tmp == sceneBocs[j])
                {
                    outputs[j].revealMat = t;
                    outputs[j].rnds.Add(sceneRnds[i]);
                    outputs[j].objects.Add(sceneRnds[i].gameObject);
                    outputs[j].boc = tmp;
                    continue;
                }
            }
        }

        bakeOutputs = new List<RendererGroup>(outputs);
        isolateObjs = new List<Renderer>();
        for (int i = 0; i < bakeOutputs.Count; i++)
        {
            RendererGroup tmp = bakeOutputs[i];
            if (tmp.objects.Count == 1)
            {
                bakeOutputs.Remove(tmp);
                isolateObjs.AddRange(tmp.rnds);
            }
        }
        bakeCount = bakeOutputs.Count;
        destroyObjs = new List<Renderer>();
        for (int i = 0; i < bakeCount; i++)
            destroyObjs.AddRange(GetBakeOutput(i).rnds);
        Debug.Log("Eligible to combine total: " + destroyObjs.Count + "and bound: "+sceneBocs.Count);
    }


    string getBoc(MeshFilter _mf, int boundLimit)
    {
        Mesh _m = _mf.sharedMesh;
        _m.RecalculateBounds();
        Vector3 bc = _m.bounds.center + _mf.transform.position;
        string boc = Mathf.FloorToInt(bc.x / boundLimit) + "_" + Mathf.FloorToInt(bc.z / boundLimit);
        return boc;
    }
    void errorCheck(Renderer[] sceneRnds)
    {
        errorObjects = new List<Renderer>();
        multMaterialObjects = new List<Renderer>();
        for (int i = 0; i < sceneRnds.Length; i++)
        {
            if (sceneRnds[i].sharedMaterials.Length > 1)
                multMaterialObjects.Add(sceneRnds[i]);
            if (sceneRnds[i].sharedMaterial == null)
                errorObjects.Add(sceneRnds[i]);

            MeshFilter mf = sceneRnds[i].GetComponent<MeshFilter>();
            if (mf)
            {
                if (mf.sharedMesh == null)
                    errorObjects.Add(sceneRnds[i]);
            }
        }
    }

    public RendererGroup[] GetResult()
    {
        return outputs;
    }
    public int GetTotalBakerGroupCounts()
    {
        return bakeCount;
    }
    public Renderer[] GetIsolateRenderer()
    {
        return isolateObjs.ToArray();
    }
    public Renderer[] GetErrorRenderer()
    {
        return errorObjects.ToArray();
    }
    public Renderer[] GetMultMatRenderer()
    {
        return multMaterialObjects.ToArray();
    }

    public Renderer[] GetDestroyedObjs()
    {
        return destroyObjs.ToArray();
    }

    public RendererGroup GetBakeOutput(int _index)
    {
        return bakeOutputs[_index];
    }
}
