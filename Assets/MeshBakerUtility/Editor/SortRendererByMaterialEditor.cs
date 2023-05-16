using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SortRendererByMaterial))]
public class SortRendererByMaterialEditor : Editor
{
    SortRendererByMaterial so;
    void OnEnable()
    {
        so = (SortRendererByMaterial)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Sort"))
            so.Sort();
        DrawDefaultInspector();
    }
}
