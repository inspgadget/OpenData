using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GlobeTest.ApplicationLayer;
using GlobeTest.DomainLayer;

public class CountrySelectorBehaviorOld : MonoBehaviour
{
	public KeyCode continentLeft = KeyCode.LeftArrow;
	public KeyCode continentRight = KeyCode.RightArrow;
	public KeyCode countryUp = KeyCode.UpArrow;
	public KeyCode countryDown = KeyCode.DownArrow;
	
	public SelectorItemBehavior previousContinent = null;
	public SelectorItemBehavior activeContinent = null;
	public SelectorItemBehavior nextContinent = null;
	
	public SelectorItemBehavior previousCountry = null;
	public SelectorItemBehavior activeCountry = null;
	public SelectorItemBehavior nextCountry = null;
	
	private CountrySelectorController m_countrySelectorController;
	/*private List<Continent> m_continents;
	
	private int m_continentIndex;
	private int m_countryIndex;*/
	
	void OnGUI()
	{
		GUI.Label(new Rect(10.0f, 10.0f, 200.0f, 30.0f), m_countrySelectorController.CurrentContinent.Name);
		GUI.Label(new Rect(10.0f, 40.0f, 200.0f, 30.0f), m_countrySelectorController.CurrentCountry.Name);
	}
	
	void Start()
	{
		m_countrySelectorController = new CountrySelectorController();
		/*m_continentIndex = 0;
		m_countryIndex = 0;
		
		m_continents = new List<Continent>();*/
		
		//europe
		List<Country> europeCountries = new List<Country>();
		europeCountries.Add(new Country("Austria", "at", "flag_austria"));
		europeCountries.Add(new Country("Germany", "de", "flag_germany"));
		europeCountries.Add(new Country("Switzerland", "ch", "flag_switzerland"));
		
		//m_continents.Add(new Continent("Europe", europeCountries, "flag_europe"));
		m_countrySelectorController.Continents.Add(new Continent("Europe", europeCountries, "flag_europe"));
		
		//north america
		List<Country> northAmericaCountries = new List<Country>();
		northAmericaCountries.Add(new Country("Canada", "ca", "flag_canada"));
		northAmericaCountries.Add(new Country("USA", "us", "flag_usa"));
		
		//m_continents.Add(new Continent("North America", northAmericaCountries, ""));
		m_countrySelectorController.Continents.Add(new Continent("North America", northAmericaCountries, ""));
	}
	
	void FixedUpdate()
	{
		RefreshSelectionIndexes();
		
		PrepareRendering();
	}
	
	private void RefreshSelectionIndexes()
	{
		if(Input.GetKeyDown(continentLeft) == true)
		{
			m_countrySelectorController.SwitchToPreviousContinent();
			/*m_continentIndex--;
			
			if(m_continentIndex < 0)
			{
				m_continentIndex = m_continents.Count - 1;
			}
			
			m_countryIndex = 0;*/
		}
		
		if(Input.GetKeyDown(continentRight) == true)
		{
			m_countrySelectorController.SwitchToNextContinent();
			/*m_continentIndex++;
			
			if(m_continentIndex >= m_continents.Count)
			{
				m_continentIndex = 0;
			}
			
			m_countryIndex = 0;*/
		}
		
		if(Input.GetKeyDown(countryUp) == true)
		{
			m_countrySelectorController.SwitchToPreviousCountry();
			/*m_countryIndex--;
			
			if(m_countryIndex < 0)
			{
				m_countryIndex = m_continents[m_continentIndex].Countries.Count - 1;
			}*/
		}
		
		if(Input.GetKeyDown(countryDown) == true)
		{
			m_countrySelectorController.SwitchToNextCountry();
			/*m_countryIndex++;
			
			if(m_countryIndex >= m_continents[m_continentIndex].Countries.Count)
			{
				m_countryIndex = 0;
			}*/
		}
	}
	
	private void PrepareRendering()
	{
		//continents
		previousContinent.PlaneTexture = m_countrySelectorController.PreviousContinent.PlaneTexture;
		activeContinent.PlaneTexture = m_countrySelectorController.CurrentContinent.PlaneTexture;
		nextContinent.PlaneTexture = m_countrySelectorController.NextContinent.PlaneTexture;
		/*int activeContinentIndex = m_continentIndex;
		int previousContinentIndex = activeContinentIndex -1;
		int nextContinentIndex = activeContinentIndex + 1;
		
		if(previousContinentIndex < 0)
		{
			previousContinentIndex = m_continents.Count - 1;
		}
		
		if(nextContinentIndex >= m_continents.Count)
		{
			nextContinentIndex = 0;
		}
		
		previousContinent.PlaneTexture = m_continents[previousContinentIndex].PlaneTexture;
		activeContinent.PlaneTexture = m_continents[m_continentIndex].PlaneTexture;
		nextContinent.PlaneTexture = m_continents[nextContinentIndex].PlaneTexture;*/
		
		//countries
		previousCountry.PlaneTexture = m_countrySelectorController.PreviousCountry.PlaneTexture;
		activeCountry.PlaneTexture = m_countrySelectorController.CurrentCountry.PlaneTexture;
		nextCountry.PlaneTexture = m_countrySelectorController.NextCountry.PlaneTexture;
		/*int activeCountryIndex = m_countryIndex;
		int previousCountryIndex = activeCountryIndex -1;
		int nextCountryIndex = activeCountryIndex + 1;
		
		if(previousCountryIndex < 0)
		{
			previousCountryIndex = m_continents[m_continentIndex].Countries.Count - 1;
		}
		
		if(nextCountryIndex >= m_continents[m_continentIndex].Countries.Count)
		{
			nextCountryIndex = 0;
		}
		
		previousCountry.PlaneTexture = m_continents[m_continentIndex].Countries[previousCountryIndex].PlaneTexture;
		activeCountry.PlaneTexture = m_continents[m_continentIndex].Countries[activeCountryIndex].PlaneTexture;
		nextCountry.PlaneTexture = m_continents[m_continentIndex].Countries[nextCountryIndex].PlaneTexture;*/
	}
}
