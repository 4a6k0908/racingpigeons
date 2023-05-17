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
        GUI.enabled = true;
        DrawDefaultInspector();
    }
}
