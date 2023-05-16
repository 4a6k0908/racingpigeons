using DigitalOpus.MB.MBEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using UnityEngine;

[CustomEditor(typeof(AWJMainSceneBakerHelper))]
public class AWJMainSceneBakerHelperEditor : Editor
{
    public AWJMainSceneBakerHelper so;
    void OnEnable()
    {
        so = (AWJMainSceneBakerHelper)target;
    }
    bool exp;
    GameObject newPrefab;
    public override void OnInspectorGUI()
    {
        exp = true;
        EditorGUILayout.HelpBox("Welcome to AwesomeJohn tool, hope you enjoy it.", MessageType.Info);
        int b = 0;
        if (GUILayout.Button("Help me Sort Materials."))
            so.Init();
        if (so.GetInstant() == null)
        {
            EditorGUILayout.HelpBox("Please assign the Target and Sort Materials before bake", MessageType.Error);
            GUI.enabled = true;
            DrawDefaultInspector();
            return;
        }
        else
            b = so.GetInstant().GetTotalBakerGroupCounts();

        GUI.enabled = Convert.ToBoolean(b);
        if (so.errorObjects.Length > 0)
        {
            EditorGUILayout.HelpBox("Please check the ErrorObjects below, it might messing mesh or material, fix that and Sort Materials again", MessageType.Error);

            GUI.enabled = exp;
            if (GUILayout.Button("Auto fix"))
                fixError();
            GUI.enabled = true;
            DrawDefaultInspector();
            return;
        }
        if (so.multMaterialObjects.Length > 0)
            EditorGUILayout.HelpBox("Please check the MultMaterialObjects below, it contain 2 or more materials, , be careful, if the mesh contain multMaterial (subMesh) the real cost will more than here.\n Fix that (Option) and Sort Materials again", MessageType.Warning);
        EditorGUILayout.HelpBox("This target cause "+so.outputs.Length+" materials include " +so.asjc.GetTotalBakerGroupCounts()+" groups and "+so.isolateObjs.Length+" isolateObjs .\n If you want to preview result, please press Preview button to bake each element, I want to review that.", MessageType.Info);
        GUILayout.BeginHorizontal();
        for (int i=0; i<b; i++)
        {
            if (GUILayout.Button("Preview " + i))
                Bake(i);
            if(i%3 == 2)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("Generate compress prefab", MessageType.Info);
        GUI.enabled = exp;
        if (GUILayout.Button("Compress!!"))
        {
            if (newPrefab)
                DestroyImmediate(so);
            newPrefab = so.GenerateScene();
            for (int i = 0; i < b; i++)
            {
                Bake(i);
                string name = GetName(i);
                ExportBake(i, name, newPrefab.transform);
            }
            so.target.SetActive(false);
            newPrefab.SetActive(true);
        }
        GUI.enabled = true;
        DrawDefaultInspector();
    }

    void fixError()
    {
        for(int i=0; i<so.errorObjects.Length; i++)
        {
            MeshFilter mf = so.errorObjects[i].GetComponent<MeshFilter>();
            if (mf)
                DestroyImmediate(mf);
            DestroyImmediate(so.errorObjects[i]);
        }
    }
    public void Bake(int _index)
    {
        so.meshBaker.objsToMesh = so.GetInstant().GetBakeOutput(_index).objects;
        MB3_MeshBakerEditorInternal.bake(so.meshBaker);
    }

    public void ExportBake(int _index,string name, Transform _parent = null)
    {
        so.meshBaker.meshCombiner.resultSceneObject.SetActive(false);
        so.meshBaker.meshCombiner.resultSceneObject.name += name;

        string id = target.name + "_" + name;

        //string filePath = Path.Combine(Application.dataPath, id + ".fbx");
        string folder = "/Model/";
        string assetPath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
        assetPath = assetPath.Replace(".unity", "") + folder + id + ".fbx";
        ModelExporter.ExportObject(assetPath, so.meshBaker.meshCombiner.resultSceneObject);

        GameObject ast = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
        GameObject fbx = Instantiate(ast);
        fbx.transform.SetParent(_parent);
        fbx.transform.localPosition = Vector3.zero;
        fbx.transform.localEulerAngles = Vector3.zero;
        fbx.transform.localScale = Vector3.one;

        fbx.GetComponentInChildren<Renderer>().sharedMaterial = so.GetInstant().GetBakeOutput(_index).revealMat;
    }

    public virtual string GetName(int _index)
    {
        return so.GetInstant().GetBakeOutput(_index).revealMat.name;
    }
}
