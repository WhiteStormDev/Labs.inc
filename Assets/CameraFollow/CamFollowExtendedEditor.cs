
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CamFollowExtended))]
public class CamFollowExtenedEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CamFollowExtended e = (CamFollowExtended)target;
        GUILayout.Label("FocusPoints&Triggers Control", EditorStyles.centeredGreyMiniLabel);
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Add FocusPoint/Trigger PARE")) e.AddFocusPointAndTriggerPare();
        if (GUILayout.Button("Add FocusPoint/Trigger QUAD")) e.AddFocusPointAndTriggerQuad();
        if (GUILayout.Button("Add SetFreeCamTrigger")) e.AddSetFreeCamTrigger();
        GUILayout.EndHorizontal();
   
       
    }
}
#endif