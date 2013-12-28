using System;
using System.Collections.Generic;
using System.Linq;

using Globetrotter.DataLayer;
using Globetrotter.DomainLayer;
using Globetrotter.PersistenceLayer;

namespace Globetrotter.ApplicationLayer
{
	public class CountriesController
	{
		//private object m_lockObject = new object();

		private const int MaxSelectedCountries = 5;

		private DataController m_dataController;

		private Country m_currCountry;
		private Dictionary<string, Country> m_countries;
		private List<Country> m_selectedCountries;

		public Country CurrentCountry
		{
			get
			{
				return m_currCountry;
			}
		}

		public Country[] Countries
		{
			get
			{
				return m_countries.Values.ToArray();
			}
		}

		public Country[] SelectedCountries
		{
			get
			{
				return m_selectedCountries.ToArray();
			}
		}

		public CountriesController(DataController dataController)
		{
			try{
			//
			m_dataController = dataController;
			m_dataController.WorldBankDataFetched += WorldBankDataFetchedHandler;

			m_currCountry = null;
			m_selectedCountries = new List<Country>();

			m_countries = new Dictionary<string, Country>();
			IList<Country> countries = new ContinentCountryWorldBankLoader().LoadCountries();

			int year = DateTime.Now.Year - 1;

			if(countries != null)
			{
				foreach(Country country in countries)
				{
					m_countries[country.IsoAlphaThreeCode] = country;

					//load indicator data
				}
			}
			//
			try
			{
				m_currCountry = m_countries["AUT"];
			}
			catch(Exception exc)
			{
				UnityEngine.Debug.LogError(exc);
			}
			//
			}catch(Exception exc){UnityEngine.Debug.LogError(exc);}
		}

		public void AddCountry()
		{
			if(m_currCountry!=null)
			{
				if(m_selectedCountries.Count >= CountriesController.MaxSelectedCountries)
				{
					m_selectedCountries.RemoveAt(0);
				}

				m_selectedCountries.Add(m_currCountry);
			}
		}

		public void RemoveCountry()
		{
			if(m_currCountry!=null)
			{
				m_selectedCountries.Remove(m_currCountry);
			}
		}

		protected void WorldBankDataFetchedHandler(object sender, WorldBankDataFetchedEventArgs args)
		{
			//
		}
	}
}
