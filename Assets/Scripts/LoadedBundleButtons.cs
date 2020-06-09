using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadedBundleButtons : MonoBehaviour
{

	[SerializeField]
	private Button m_individuals;
	public Button IndividualsButton { get { return m_individuals; } }

	[SerializeField]
	private Button m_all;
	public Button AllButton { get { return m_all; } }

	[SerializeField]
	private Button m_unmount;
	public Button Unmount { get { return m_unmount; } }
}
