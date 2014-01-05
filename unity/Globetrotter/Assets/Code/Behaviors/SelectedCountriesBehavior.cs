using UnityEngine;
using System.Collections;

using Globetrotter.DomainLayer;
using Globetrotter.GuiLayer;
using Globetrotter.GuiLayer.ViewModel;

public class SelectedCountriesBehavior : MonoBehaviour
{
	private SelectedCountriesViewModel m_selectedCountriesViewModel;

	void OnGUI()
	{
		lock(m_selectedCountriesViewModel.LockObject)
		{
			Country[] selectedCountries = m_selectedCountriesViewModel.SelectedCountries;

			//left, top, width, height
			int screenWidth = Screen.width;
			int screenHeight = Screen.height;
			
			//box
			GUIStyle style = StyleDepot.Instance.UnfocusedBoxStyle;
			
			if(m_selectedCountriesViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedBoxStyle;
			}

			GUI.Box(new Rect(screenWidth - 220, (screenHeight / 2) + 5, 210, (screenHeight / 2) - 10), string.Empty, style);

			//labels
			style = StyleDepot.Instance.UnfocusedTextStyle;
			
			if(m_selectedCountriesViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedTextStyle;
			}

			if((selectedCountries != null) && (selectedCountries.Length > 0))
			{
				int top = (screenHeight / 2) + 15;

				for(int i = 0; i < selectedCountries.Length; i++)
				{
					GUIStyle s = style;

					if((i == m_selectedCountriesViewModel.CurrentCountryIndex) && (m_selectedCountriesViewModel.ReactOnInput == true))
					{
						s = StyleDepot.Instance.SelectedCountryHoverStyle;
					}

					GUI.Label(new Rect( screenWidth - 210, top, 200, 25), selectedCountries[i].Name, s);

					top = top + 30;
				}
			}
			else
			{
				GUI.Label(new Rect( screenWidth - 210, (screenHeight / 2) + 15, 200, 25), "No countries selected.", style);
			}
		}
	}

	public void Init(SelectedCountriesViewModel selectedCountriesViewModel)
	{
		m_selectedCountriesViewModel = selectedCountriesViewModel;
	}
}
