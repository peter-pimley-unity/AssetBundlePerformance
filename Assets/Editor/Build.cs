using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Build
{
    // Start is called before the first frame update


		[MenuItem ("Test/Build AssetBundles")]
	public static void BuildBundles ()
	{
		BuildAssetBundleOptions opts = BuildAssetBundleOptions.UncompressedAssetBundle;
		BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
		string path = $"{Application.streamingAssetsPath}/{target}";
		System.IO.Directory.CreateDirectory(path);
		Debug.Log($"Building to {path}");
		BuildPipeline.BuildAssetBundles (path, opts, target);
		AssetDatabase.Refresh(); // Otherwise the editor can fail to notice the new bundle in StreamingAssets.
		Debug.Log("Completed.");
	}
}
