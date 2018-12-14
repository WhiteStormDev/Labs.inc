
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Ladder))]

public class LadderEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		Ladder e = (Ladder)target;
		GUILayout.Label("Управление секциями:", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Section")) e.AddSection();
		if(GUILayout.Button("Remove Section")) e.RemoveSection();
		if(GUILayout.Button("Clear All")) e.ClearAll();
		GUILayout.EndHorizontal();
	}
}
#endif