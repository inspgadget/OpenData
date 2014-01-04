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
			if(m_selectedCountriesViewModel.ReactOnInput == true)
			{
				GUI.Box(new Rect(screenWidth - 220, (screenHeight / 2) + 5, 210, (screenHeight / 2) - 10),
				        	string.Empty,
				        	StyleDepot.Instance.FocusedBoxStyle);
			}
			else
			{
				GUI.Box(new Rect(screenWidth - 220, (screenHeight / 2) + 5, 210, (screenHeight / 2) - 10), string.Empty,
				        	StyleDepot.Instance.UnfocusedBoxStyle);
			}

			//labels
			if((selectedCountries != null) && (selectedCountries.Length > 0))
			{
				int top = (screenHeight / 2) + 15;

				for(int i = 0; i < selectedCountries.Length; i++)
				{
					GUIStyle style = StyleDepot.Instance.SelectedCountryStyle;

					if(i == m_selectedCountriesViewModel.CurrentCountryIndex)
					{
						style = StyleDepot.Instance.SelectedCountryHoverStyle;
					}

					GUI.Label(new Rect( screenWidth - 210, top, 200, 25), selectedCountries[i].Name, style);

					top = top + 30;
				}
			}
			else
			{
				GUI.Label(new Rect( screenWidth - 210, (screenHeight / 2) + 15, 200, 25),
							"No countries selected.",
				          	StyleDepot.Instance.SelectedCountryStyle);
			}
		}
	}

	public void Init(SelectedCountriesViewModel selectedCountriesViewModel)
	{
		m_selectedCountriesViewModel = selectedCountriesViewModel;
	}
}
