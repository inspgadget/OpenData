using UnityEngine;
using System.Collections;

using Globetrotter.GuiLayer.ViewModel;

public class YearFromBehavior : MonoBehaviour
{
	private YearFromViewModel m_yearFromViewModel;

	void Update()
	{
		lock(m_yearFromViewModel.LockObject)
		{
		}
	}

	public void Init(YearFromViewModel yearFromViewModel)
	{
		m_yearFromViewModel = yearFromViewModel;
	}
}
