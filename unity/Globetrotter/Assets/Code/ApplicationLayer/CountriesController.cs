using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Globetrotter.DataLayer;
using Globetrotter.DomainLayer;
using Globetrotter.PersistenceLayer;

namespace Globetrotter.ApplicationLayer
{
	public class CountriesController
	{
		private object m_lockObj = new object();

		private const int MaxSelectedCountries = 5;

		public delegate void SelectedCountriesChangedEventHandler(object sender, SelectedCountriesChangedEventArgs args);
		public event SelectedCountriesChangedEventHandler SelectedCountriesChanged;

		private DataController m_dataController;

		private Country m_currCountry;
		private Dictionary<string, Country> m_countries;
		private List<Country> m_selectedCountries;

		public Country CurrentCountry
		{
			get
			{
				lock(m_lockObj)
				{
					return m_currCountry;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_currCountry = value;
				}
			}
		}

		public Country[] Countries
		{
			get
			{
				lock(m_lockObj)
				{
					return m_countries.Values.ToArray();
				}
			}
		}

		public Country[] SelectedCountries
		{
			get
			{
				lock(m_lockObj)
				{
					return m_selectedCountries.ToArray();
				}
			}
		}

		public CountriesController(DataController dataController)
		{
			//try{
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
					if(string.IsNullOrEmpty(country.CapitalCity) == false)
					{
						m_countries[country.IsoAlphaThreeCode] = country;

						//load indicator data
						Thread t = new Thread(delegate(){
							//population
							WorldBankIndicator populationIndicator = m_dataController.GetIndicator("");
							WorldBankData populationData = m_dataController.FetchData(new Country[]{ country }, populationIndicator, year, year);
							country.Population = (int)populationData.Items[0].Value;

							//surface area
							WorldBankIndicator surfaceAreaIndicator = m_dataController.GetIndicator("AG.LND.TOTL.K2");
							WorldBankData surfaceAreaData = m_dataController.FetchData(new Country[]{ country }, surfaceAreaIndicator, year, year);
							country.SurfaceArea = surfaceAreaData.Items[0].Value;
						});

						t.IsBackground = true;
						//t.Start();
					}
				}
			}

			try
			{
				m_currCountry = m_countries["AUT"];
			}
			catch(Exception exc)
			{
				UnityEngine.Debug.LogError(exc);
			}
			//}catch(Exception exc){UnityEngine.Debug.LogError(exc);}
		}

		public void AddCountry()
		{
			lock(m_lockObj)
			{
				AddCountry(m_currCountry);
			}
		}
		
		public void AddCountry(Country country)
		{
			lock(m_lockObj)
			{
				if(country != null)
				{
					if(m_selectedCountries.Count >= CountriesController.MaxSelectedCountries)
					{
						m_selectedCountries.RemoveAt(0);
					}
					
					m_selectedCountries.Add(country);
				}
			}
			
			OnSelectedCountriesChanged(SelectedCountries);
		}

		public void RemoveCountry()
		{
			lock(m_lockObj)
			{
				RemoveCountry(m_currCountry);
			}
		}
		
		public void RemoveCountry(Country country)
		{
			lock(m_lockObj)
			{
				if(country != null)
				{
					m_selectedCountries.Remove(country);
				}
			}
			
			OnSelectedCountriesChanged(SelectedCountries);
		}

		public void WorldBankDataFetchedHandler(object sender, WorldBankDataFetchedEventArgs args)
		{
			//
		}

		protected void OnSelectedCountriesChanged(Country[] countries)
		{
			if(SelectedCountriesChanged != null)
			{
				SelectedCountriesChanged(this, new SelectedCountriesChangedEventArgs(countries));
			}
		}
	}
}
