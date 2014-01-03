using UnityEngine;
using System.Collections;

using Globetrotter.ApplicationLayer;
using Globetrotter.DomainLayer;

public class SelectedCountriesBehavior : MonoBehaviour
{
	private CountriesController m_countriesController;

	void OnGUI()
	{
		Country[] m_selectedCountries = m_countriesController.SelectedCountries;

		//left, top, width, height
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		
		//box
		GUI.Box(new Rect(screenWidth - 220, (screenHeight / 2) + 5, 210, (screenHeight / 2) - 10), string.Empty);

		//labels
		if((m_selectedCountries != null) && (m_selectedCountries.Length > 0))
		{
			int top = (screenHeight / 2) + 15;

			for(int i = 0; i < m_selectedCountries.Length; i++)
			{
				GUI.Label(new Rect( screenWidth - 210, top, 200, 25), "No countries selected.");

				top = top + 30;
			}
		}
		else
		{
			GUI.Label(new Rect( screenWidth - 210, (screenHeight / 2) + 15, 200, 25), "No countries selected.");
		}
	}

	public void Init(CountriesController countriesController)
	{
		m_countriesController = countriesController;
	}
}
