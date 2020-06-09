using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(FilenamesScriptableObject))]
public class FilenamesScriptableObjectInspector : Editor
{


	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("Refresh"))
		{
			string[] assets = AssetDatabase.FindAssets("t:Texture");
			Debug.LogFormat($"Refresh: {assets.Length} assets.");

			SerializedProperty names = serializedObject.FindProperty("m_names");
			names.ClearArray();
			for (int i = 0; i < assets.Length; ++i)
			{
				string guid = assets[i];
				string path = AssetDatabase.GUIDToAssetPath(guid);
				names.InsertArrayElementAtIndex(i);
				SerializedProperty p = names.GetArrayElementAtIndex(i);
				p.stringValue = path;
			}
			serializedObject.ApplyModifiedProperties();
		}

		DrawDefaultInspector();
		
		
	}
}
