using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BuildsScriptableObject))]
public class BuildsScriptableObjectInspector : Editor
{

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Build"))
			BuildBundles();
		DrawDefaultInspector();
	}


	private void BuildBundles ()
	{
		BuildsScriptableObject b = target as BuildsScriptableObject;

		foreach (BuildsScriptableObject.Config config in b.m_configs)
		{
			BuildTarget target = BuildTarget.NoTarget;
			bool ok = System.Enum.TryParse<BuildTarget>(config.BuiltTargetName, out target);
			if (target == BuildTarget.NoTarget)
			{
				Debug.LogWarning($"Failed to parse \"{config.BuiltTargetName}\" as a BuildTarget.  Ignoring.");
				continue;
			}


			BuildAssetBundleOptions opts = BuildAssetBundleOptions.UncompressedAssetBundle;
			string path = $"{Application.streamingAssetsPath}/{config.Directory}";
			System.IO.Directory.CreateDirectory(path);
			Debug.Log($"Building to {path}");
			BuildPipeline.BuildAssetBundles(path, opts, target);
			AssetDatabase.Refresh(); // Otherwise the editor can fail to notice the new bundle in StreamingAssets.
			Debug.Log("Completed.");
		}
	}
}
