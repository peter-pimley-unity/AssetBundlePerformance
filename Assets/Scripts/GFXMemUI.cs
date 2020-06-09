using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GFXMemUI : MonoBehaviour
{

	[SerializeField]
	private Slider m_slider;

	[SerializeField]
	private long m_maxMiB;

	[SerializeField]
	private Text m_maxText;


	private void Awake()
	{
		m_maxText.text = $"{m_maxMiB} MiB";
	}


	// Update is called once per frame
	void Update()
    {
		long gfxBytes = UnityEngine.Profiling.Profiler.GetAllocatedMemoryForGraphicsDriver();

		m_slider.value = (float)gfxBytes / (m_maxMiB * 1024 * 1024);
    }
}
