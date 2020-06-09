using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName ="Builds", menuName ="Builds")]
public class BuildsScriptableObject : ScriptableObject
{
	[System.Serializable]
	public class Config
	{
		public string BuiltTargetName;

		public string [] RuntimePlatformNames;

		public string Directory;

	};

	public Config [] m_configs;

}

