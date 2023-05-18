using DigitalOpus.MB.MBEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using UnityEngine;

[CustomEditor(typeof(AWJURPStandardBackHelper))]
public class AWJURPStandardBackHelperEditor : Editor
{
    AWJURPStandardBackHelper so;
    void OnEnable()
    {
        so = (AWJURPStandardBackHelper)target;
    }

    GameObject newPrefab;
    public override void OnInspectorGUI()
    {
        if(so.target == null)
        {
            EditorGUILayout.HelpBox("Please assign the Target", MessageType.Error);
            GUI.enabled = false;
        }
        if (GUILayout.Button("Check Materials"))
            so.Check();
        if (so.errorObjects.Length != 0 || so.multMaterialObjects.Length != 0)
        {
            EditorGUILayout.HelpBox("Please fix that object otherwise it might be error", MessageType.Error);
            if (GUILayout.Button("Ignore... force Sort"))
                so.Sort();
        }
        else
        {
            if (GUILayout.Button("Help me Sort Materials"))
                so.Sort();
        }
        GUILayout.BeginHorizontal();
        int b = so.groups.Length;
        for (int i = 0; i < b; i++)
        {
            if (GUILayout.Button("Toggle " + i))
                so.ToggleActive(so.groups[i].objects);
            if (i % 2 == 1)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        GUILayout.EndHorizontal();


        EditorGUILayout.HelpBox("Generate compress prefab", MessageType.Info);
        if (GUILayout.Button("Compress!!"))
        {
            if (newPrefab)
                DestroyImmediate(newPrefab);
            newPrefab = new GameObject();
            newPrefab.name = so.target.name + "_(compress)";
            newPrefab.transform.localPosition = so.target.transform.localPosition;
            newPrefab.transform.localEulerAngles = so.target.transform.localEulerAngles;
            newPrefab.transform.localScale = so.target.transform.localScale;
            for (int i = 0; i < b; i++)
            {
                if (so.groups[i].objects.Length == 0)
                    continue;
                Bake(i);
                string name = so.groups[i].tag;
                ExportBake(i, name, newPrefab.transform);
            }
            so.target.SetActive(false);
            newPrefab.SetActive(true);
        }
        GUI.enabled = true;
        DrawDefaultInspector();
    }

    public void Bake(int _index)
    {
        MB3_MeshBakerEditorInternal.bake(so.groups[_index].meshBaker.GetComponentInChildren<MB3_MeshBakerCommon>());
    }

    public void ExportBake(int _index, string name, Transform _parent = null)
    {
        so.groups[_index].meshBaker.GetComponentInChildren<MB3_MeshBakerCommon>().meshCombiner.resultSceneObject.SetActive(false);
        so.groups[_index].meshBaker.GetComponentInChildren<MB3_MeshBakerCommon>().meshCombiner.resultSceneObject.name += name;

        string id = target.name + "_" + name;

        //string filePath = Path.Combine(Application.dataPath, id + ".fbx");
        string folder = "/Model/";
        string assetPath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        assetPath = assetPath.Replace(".unity", "") + folder + id + ".fbx";
        ModelExporter.ExportObject(assetPath, so.groups[_index].meshBaker.GetComponentInChildren<MB3_MeshBakerCommon>().meshCombiner.resultSceneObject);

        GameObject ast = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
        GameObject fbx = Instantiate(ast);
        fbx.transform.SetParent(_parent);
        fbx.transform.localPosition = Vector3.zero;
        fbx.transform.localEulerAngles = Vector3.zero;
        fbx.transform.localScale = Vector3.one;

        fbx.GetComponentInChildren<Renderer>().sharedMaterial = so.groups[_index].meshBaker.resultMaterial;
    }
}
