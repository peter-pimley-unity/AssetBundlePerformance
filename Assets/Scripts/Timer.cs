using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Timer : MonoBehaviour
{

	[SerializeField]
	private FilenamesScriptableObject m_filenamesObject;

	private IEnumerable<string> m_filenames;

	[SerializeField]
	private BuildsScriptableObject m_builds;


	private Coroutine m_coroutine;

	public bool IsBusy => m_coroutine != null;


	private struct TimedSection : IDisposable
	{
		private DateTime before;
		System.Action<TimeSpan> cb;

		public TimedSection (System.Action<TimeSpan> cb)
		{
			this.cb = cb;
			before = DateTime.Now;
		}

		public void Dispose ()
		{
			DateTime after = DateTime.Now;
			if (cb != null)
				cb(after - before);
			cb = null; // avoid duplicate calls.
		}
	}


	private void Awake()
	{
		m_filenames = m_filenamesObject.GetRandomizedNames();
	}

	public IEnumerator Mount (System.Action<AssetBundle, TimeSpan> cb)
	{
		string platform = Application.platform.ToString();
		string dir = m_builds.m_configs.First(c => c.RuntimePlatformNames.Contains(platform)).Directory;
		Debug.Log($"Platform {platform} loading from {dir}.");
		string path = $"{Application.streamingAssetsPath}/{dir}/village";
		Debug.Log($"Opening from {path}");
		DateTime before = DateTime.Now;
		var load = AssetBundle.LoadFromFileAsync(path);
		yield return load;
		AssetBundle b = load.assetBundle;
		DateTime after = DateTime.Now;
		if (cb != null)
			cb(b, after - before);
	}


	private IEnumerator DoIndividuals (AssetBundle b, System.Action<TimeSpan> cb)
	{
		DateTime before = DateTime.Now;
		foreach (string filename in m_filenames)
			yield return b.LoadAssetAsync<Texture>(filename);
		DateTime after = DateTime.Now;
		cb(after - before);
	}


	public IEnumerator LoadAll (AssetBundle b, System.Action<TimeSpan> cb)
	{
		using (new TimedSection(cb))
			yield return b.LoadAllAssetsAsync();
	}

	public IEnumerator LoadIndividuals (AssetBundle b, System.Action<TimeSpan> cb)
	{
		using (new TimedSection(cb))
			foreach (string filename in m_filenames)
				yield return b.LoadAssetAsync(filename);
	}

	public IEnumerator Unload (System.Action<TimeSpan> cb)
	{
		using (new TimedSection(cb))
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			yield return Resources.UnloadUnusedAssets();
		}
	}


	public IEnumerator Unmount(AssetBundle b, bool unloadAllLoadedObjects, System.Action<TimeSpan> cb)
	{
		using (new TimedSection(cb))
		{
			b.Unload(unloadAllLoadedObjects);
			yield return null;
		}
	}
	
}
