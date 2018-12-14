#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AIController))]
public class AIControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AIController c = (AIController)target;
        if (GUILayout.Button("Add Pattern Point")) c.AddPatternPoint();
    }
}
#endif