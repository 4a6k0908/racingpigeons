using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AWJURPStandardBackHelper : MonoBehaviour
{
    Shader keyCheckShader;
    public Material specifyMat;
    public GameObject target;
    public AWJ_Core asjc;

    public MB3_TextureBaker[] meshBakers;
    [System.Serializable]
    public struct RendererGroup
    {
        public string tag;
        public GameObject[] objects;
        public MB3_TextureBaker meshBaker;
    }

    public RendererGroup[] groups;
    public Renderer[] errorObjects;
    public Renderer[] multMaterialObjects;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Check()
    {
        Renderer[] rnds = target.GetComponentsInChildren<Renderer>(true);
        Debug.Log("find " + rnds.Length + " renderers");
        asjc = new AWJ_Core();
        asjc.Sort(target, rnds);
        errorObjects = asjc.GetErrorRenderer();
        multMaterialObjects = asjc.GetMultMatRenderer();
    }
    public void Sort()
    {
        keyCheckShader = Shader.Find("Universal Render Pipeline/Lit");
        Renderer[] rnds = target.GetComponentsInChildren<Renderer>(true);
        Debug.Log("find " + rnds.Length + " renderers");

        groups = new RendererGroup[4];
        groups[0].tag = "Opaque";
        groups[1].tag = "Transparent";
        groups[2].tag = "Other";
        groups[3].tag = "Specify";

        List<GameObject> kop = new List<GameObject>();
        List<GameObject> ktr = new List<GameObject>();
        List<GameObject> kot = new List<GameObject>();
        List<GameObject> ksp = new List<GameObject>();

        for (int i = 0; i < rnds.Length; i++)
        {
            rnds[i].gameObject.SetActive(true);
            Material[] mats = rnds[i].sharedMaterials;
            for (int j = 0; j < mats.Length; j++)
            {
                if (mats[j] == specifyMat)
                {
                    ksp.Add(rnds[i].gameObject);
                    break;
                }
            }
            if (ksp.Contains(rnds[i].gameObject))
                continue;
            if (mats[0].shader == keyCheckShader)
            {
                bool b = IsKeyCheckShaderTransparent(mats[0]);
                if (b)
                    ktr.Add(rnds[i].gameObject);
                else
                    kop.Add(rnds[i].gameObject);
            }
            else
                kot.Add(rnds[i].gameObject);
        }

        groups[0].objects = kop.ToArray();
        groups[1].objects = ktr.ToArray();
        groups[2].objects = kot.ToArray();
        groups[3].objects = ksp.ToArray();

        if (meshBakers == null || meshBakers.Length == 0)
            return;

        for (int i = 0; i < groups.Length; i++)
            groups[i].meshBaker = meshBakers[i];

        groups[0].meshBaker.objsToMesh = kop;
        groups[1].meshBaker.objsToMesh = ktr;
        groups[2].meshBaker.objsToMesh = kot;
        groups[3].meshBaker.objsToMesh = ksp;
    }




    public static Material SetURPStandardToTransparent(Material _mat)
    {
        _mat.shader = Shader.Find("Universal Render Pipeline/Lit");
        //_mat.SetFloat("_Surface", (float)UnityEditor.BaseShaderGUI.SurfaceType.Transparent);
        _mat.SetFloat("_Surface", (float)1);
        _mat.SetOverrideTag("RenderType", "Transparent");
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //_mats[i].SetInt("_ZWrite", 1);
        _mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        //_mats[i].SetShaderPassEnabled("ShadowCaster", false);
        return _mat;
    }

    public static bool IsKeyCheckShaderTransparent(Material _mat)
    {
        if (_mat.GetInt("_SrcBlend") == (int)UnityEngine.Rendering.BlendMode.SrcAlpha &&
            _mat.GetInt("_DstBlend") == (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha)
            return true;
        else
            return false;
    }

    public void ToggleActive(GameObject[] _objs)
    {
        for (int i = 0; i < _objs.Length; i++)
            _objs[i].SetActive(!_objs[i].activeSelf);
    }
}
