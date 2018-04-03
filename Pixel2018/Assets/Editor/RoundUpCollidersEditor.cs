using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(RoundUpColliders))]
public class RoundUpCollidersEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		RoundUpColliders roundUpColliders = (RoundUpColliders)target;

		if(GUILayout.Button("Snap Collider Points"))
			roundUpColliders.RoundUp ();
	}
}