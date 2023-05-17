using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AWJURPStandardBackHelper))]
public class AWJURPStandardBackHelperEditor : Editor
{
    AWJURPStandardBackHelper so;
    void OnEnable()
    {
        so = (AWJURPStandardBackHelper)target;
    }

    public override void OnInspectorGUI()
    {
        if(so.target == null)
        {
            EditorGUILayout.HelpBox("Please assign the Target", MessageType.Error);
            GUI.enabled = false;
        }
        if (GUILayout.Button("Check Materials"))
            so.Check();
        if (so.errorObjects.Length != 0 || so.multMaterialObjects.Length!=0)
        {
            EditorGUILayout.HelpBox("Please fix that object", MessageType.Error);
            GUI.enabled = false;
        }
        if (GUILayout.Button("Help me Sort Materials"))
            so.Sort();
        GUI.enabled = true;
        DrawDefaultInspector();
    }
}
