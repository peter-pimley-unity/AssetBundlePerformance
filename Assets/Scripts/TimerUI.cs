using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class TimerUI : MonoBehaviour
{

	[SerializeField]
	private Timer m_timer;

	[SerializeField]
	private Text m_timingText;

	[SerializeField]
	private GameObject m_buttonsPanel;

	[SerializeField]
	private Button m_mountButton;

	[SerializeField]
	private Button m_unloadButton;


	[SerializeField]
	private LoadedBundleButtons m_loadedBundleButtonsPrefab;

	[SerializeField]
	private Transform m_parentForButtons;


	private void Awake()
	{
		m_mountButton.onClick.AddListener(delegate
		{
			m_mountButton.interactable = false;
			StartCoroutine(DoMount(null));
		});

		m_unloadButton.onClick.AddListener(delegate
		{
			StartCoroutine(m_timer.Unload(d => RecordTime("UnloadUnusedAssets", d)));
		});
	}

	private class BusyPeriod : System.IDisposable
	{
		private Selectable[] m_selectables;
		private GameObject m_selection;

		public BusyPeriod(TimerUI me)
		{
			m_selection = EventSystem.current.currentSelectedGameObject;
			m_selectables = me.m_buttonsPanel.GetComponentsInChildren<Selectable>().Where(x => x.IsInteractable()).ToArray();
			foreach (Selectable s in m_selectables)
				s.interactable = false;
		}

		public void Dispose ()
		{
			foreach (Selectable s in m_selectables)
				s.interactable = true;
			EventSystem.current.SetSelectedGameObject(m_selection);
		}
	}


	private IEnumerator DoMount (System.Action cb)
	{
		AssetBundle b = null;

		yield return m_timer.Mount(delegate (AssetBundle x, TimeSpan duration)
		{
			b = x;
			RecordTime("LoadFromFileAsync", duration);
		});

		var buttons = Instantiate<LoadedBundleButtons> (m_loadedBundleButtonsPrefab, m_parentForButtons);
		BindButtons(b, buttons);
		EventSystem.current.SetSelectedGameObject(buttons.AllButton.gameObject);
	}


	private void BindButtons(AssetBundle b, LoadedBundleButtons buttons)
	{
		
		buttons.AllButton.onClick.AddListener(() => StartCoroutine(DoAll(b)));
		buttons.IndividualsButton.onClick.AddListener(() => StartCoroutine(DoIndividuals(b)));
		buttons.Unmount.onClick.AddListener(() => StartCoroutine(DoUnmount(b, buttons.gameObject)));
	}

	private IEnumerator DoAll (AssetBundle b)
	{
		using (new BusyPeriod(this))
		{
			yield return m_timer.LoadAll(b, d => RecordTime("LoadAllAssetsAsync", d));
		}
	}

	private IEnumerator DoIndividuals (AssetBundle b)
	{
		using (new BusyPeriod(this))
		{
			yield return m_timer.LoadIndividuals (b, d => RecordTime ("LoadAssetAsync (many)", d));
		}
	}


	private IEnumerator DoUnmount (AssetBundle b, GameObject buttonsToDestroy)
	{
		GameObject.Destroy(buttonsToDestroy);
		yield return m_timer.Unmount(b, true, (duration) => RecordTime("Unmount", duration));
		m_mountButton.interactable = true;
		EventSystem.current.SetSelectedGameObject(m_mountButton.gameObject);
	}

	private void RecordTime(string description, TimeSpan duration)
	{
		m_timingText.text = $"{description}: {duration.TotalSeconds}.s";
	}
}
