using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu (fileName = "filenames", menuName = "Filenames Scriptable Object")]
public class FilenamesScriptableObject : ScriptableObject
{

	[SerializeField]
	private string[] m_names;
	

	public IEnumerable<string> GetRandomizedNames ()
	{
		System.Random rng = new System.Random();
		return m_names.OrderBy(x => rng.Next()).ToArray();
	}
	
}
