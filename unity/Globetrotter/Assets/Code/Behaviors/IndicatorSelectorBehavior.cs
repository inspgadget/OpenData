using UnityEngine;
using System.Collections;

using Globetrotter.DataLayer;
using Globetrotter.GuiLayer.ViewModel;

public class IndicatorSelectorBehavior : MonoBehaviour
{
	private IndicatorSelectorViewModel m_indicatorSelectorViewModel;

	void OnGUI()
	{
		lock(m_indicatorSelectorViewModel.LockObject)
		{
			//left, top, width, height
			int screenWidth = Screen.width;
			int screenWidthHalf = screenWidth / 2;

			GUI.Label(new Rect(screenWidthHalf - 200, 10, 120, 50), m_indicatorSelectorViewModel.PreviousIndicator.Name);
			GUI.Label(new Rect(screenWidthHalf - 60, 10, 120, 50), m_indicatorSelectorViewModel.CurrentIndicator.Name);
			GUI.Label(new Rect(screenWidthHalf + 80, 10, 120, 50), m_indicatorSelectorViewModel.NextIndicator.Name);
		}
	}

	public void Init(IndicatorSelectorViewModel indicatorSelectorViewModel)
	{
		m_indicatorSelectorViewModel = indicatorSelectorViewModel;
	}
}
