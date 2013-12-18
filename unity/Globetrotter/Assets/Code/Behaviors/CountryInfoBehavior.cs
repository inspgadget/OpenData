using UnityEngine;
using System.Collections;
using System.Text;

using Globetrotter.ApplicationLayer;
using Globetrotter.DomainLayer;

public class CountryInfoBehavior : MonoBehaviour
{
	private CountriesController m_countriesController;

	void OnGUI()
	{
		Country country = m_countriesController.CurrentCountry;

		GUI.Box(new Rect(Screen.width - 200, 80, 200, 300), string.Empty);

		if(country != null)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(country.Name).Append(" - ").Append(country.IsoAlphaThreeCode).Append("\n\n");
			sb.Append("Capital city: ").Append(country.CapitalCity).Append("\n");
			sb.Append("Population: ").Append(country.Population);

			GUI.Label(new Rect(Screen.width - 190, 90, 180, 280), sb.ToString());
		}
	}

	public void Init(CountriesController countriesController)
	{
		m_countriesController = countriesController;
	}
}
