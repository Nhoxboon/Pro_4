using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TowerUnlockConfigSO))]
public class TowerUnlockConfigSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TowerUnlockConfigSO config = (TowerUnlockConfigSO)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Initialize Tower Data"))
            config.InitializeTowerData();
    }
}