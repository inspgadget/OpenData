using UnityEngine;
using System.Collections;

using Globetrotter.DataLayer;
using Globetrotter.GuiLayer;
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

			//boxes
			GUIStyle style = StyleDepot.Instance.UnfocusedBoxStyle;

			if(m_indicatorSelectorViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedBoxStyle;
			}

			GUI.Box(new Rect(screenWidthHalf - 100, 10, 200, 50), string.Empty, style);

			//labels
			GUI.Label(new Rect(screenWidthHalf - 420, 10, 200, 50), m_indicatorSelectorViewModel.PreviousIndicator.Name);

			/*style = StyleDepot.Instance.UnfocusedTextStyle;

			if(m_indicatorSelectorViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedTextStyle;
			}*/

			GUI.Label(new Rect(screenWidthHalf - 100, 10, 200, 50), m_indicatorSelectorViewModel.CurrentIndicator.Name/*, style*/);

			GUI.Label(new Rect(screenWidthHalf + 120, 10, 200, 50), m_indicatorSelectorViewModel.NextIndicator.Name);
		}
	}

	public void Init(IndicatorSelectorViewModel indicatorSelectorViewModel)
	{
		m_indicatorSelectorViewModel = indicatorSelectorViewModel;
	}
}
