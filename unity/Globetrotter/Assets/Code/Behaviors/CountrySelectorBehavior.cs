using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Globetrotter.ApplicationLayer;
using Globetrotter.DomainLayer;
using Globetrotter.PersistenceLayer;

public class CountrySelectorBehavior : MonoBehaviour
{
	private CountriesController m_countriesController;

	private Country[] m_countries;

	void OnGUI()
	{
		int currCountryIndex = GetIndexOfCountry(m_countriesController.CurrentCountry);
		IFlagLoader flagLoader = new FlagResourcesLoader();

		if(currCountryIndex >= 0)
		{
			//left, top, width, height
			int screenWidth = Screen.width;
			int screenHeight = Screen.height;
			int boxCenter = screenWidth - 110;

			//boxes
			GUI.Box(new Rect(screenWidth - 220, 10, 210, (screenHeight / 2) - 15), string.Empty);
			GUI.Box(new Rect(boxCenter - 37, 15, 74, 74), string.Empty);

			//flags
			int prevCountryIndex = currCountryIndex - 1;

			if(prevCountryIndex < 0)
			{
				prevCountryIndex = m_countries.Length - 1;
			}

			int nextCountryIndex = currCountryIndex + 1;

			if(nextCountryIndex >= m_countries.Length)
			{
				nextCountryIndex = 0;
			}

			Texture prevCountryTexture = flagLoader.LoadFlag(m_countries[prevCountryIndex].IsoAlphaThreeCode);

			if(prevCountryTexture == null)
			{
				prevCountryTexture = Resources.Load<Texture2D>("no_image");
			}

			Texture currCountryTexture = flagLoader.LoadFlag(m_countries[currCountryIndex].IsoAlphaThreeCode);

			if(currCountryTexture == null)
			{
				currCountryTexture = Resources.Load<Texture2D>("no_image");
			}

			Texture nextCountryTexture = flagLoader.LoadFlag(m_countries[nextCountryIndex].IsoAlphaThreeCode);

			if(nextCountryTexture == null)
			{
				nextCountryTexture = Resources.Load<Texture2D>("no_image");
			}

			GUI.Label(new Rect(boxCenter - 106, 20, 64, 32), prevCountryTexture);
			GUI.Label(new Rect(boxCenter - 32, 20, 64, 32), currCountryTexture);
			GUI.Label(new Rect(boxCenter + 42, 20, 64, 32), nextCountryTexture);

			//country info
			StringBuilder sb = new StringBuilder();

			sb.Append(m_countries[currCountryIndex].Name).Append(" - ").Append(m_countries[currCountryIndex].IsoAlphaThreeCode).Append("\n\n");
			sb.Append("Capital city: ").Append(m_countries[currCountryIndex].CapitalCity).Append("\n");
			sb.Append("Population: ").Append(m_countries[currCountryIndex].Population).Append("\n");
			sb.Append("Surface Area: ").Append(m_countries[currCountryIndex].SurfaceArea).Append("sq. km");
			
			GUI.Label(new Rect(Screen.width - 210, 90, 200, 280), sb.ToString());
		}
	}

	public void Init(CountriesController countriesController)
	{
		m_countriesController = countriesController;

		m_countries = m_countriesController.Countries;
		Array.Sort(m_countries, new Country.CountryComparer());
	}

	private int GetIndexOfCountry(Country country)
	{
		if(country != null)
		{
			for(int i = 0; i < m_countries.Length; i++)
			{
				if(m_countries[i] == country)
				{
					return i;
				}
			}
		}

		return -1;
	}
}
