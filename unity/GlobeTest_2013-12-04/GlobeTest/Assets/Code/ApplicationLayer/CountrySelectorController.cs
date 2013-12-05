using System;
using System.Collections.Generic;
using System.ComponentModel;

using GlobeTest.DomainLayer;
using GlobeTest.InputLayer;
using GlobeTest.PersistenceLayer;

namespace GlobeTest.ApplicationLayer
{
	public class CountrySelectorController : INotifyPropertyChanged
	{
		private object m_lockObject = new object();
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		private IContinentCountryLoader m_loader;
		
		private List<Continent> m_continents;
		
		private int m_continentIndex;
		private int m_countryIndex;
		
		private List<Country> m_pinnedCountries;
		
		private bool m_continentsHorizontal;
		private bool m_onlyUseVertical;
		
		public List<Country> AllCountries
		{
			get
			{
				lock(m_lockObject)
				{
					List<Country> allCountries = new List<Country>();
					
					foreach(Continent continent in m_continents)
					{
						allCountries.AddRange(continent.Countries);
					}
					
					allCountries.Sort(new Country.CountryComparer());
					
					return allCountries;
				}
			}
		}
		
		public List<Continent> Continents
		{
			get
			{
				lock(m_lockObject)
				{
					return m_continents;
				}
			}
		}
		
		public bool ContinentsHorizontal
		{
			get
			{
				lock(m_lockObject)
				{
					return m_continentsHorizontal;
				}
			}
			
			set
			{
				lock(m_lockObject)
				{
					m_continentsHorizontal = value;
				}
			}
		}
		
		public Continent CurrentContinent
		{
			get
			{
				lock(m_lockObject)
				{
					if((m_continents != null) && (m_continentIndex >= 0) && (m_continentIndex < m_continents.Count))
					{
						return m_continents[m_continentIndex];
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public Country CurrentCountry
		{
			get
			{
				lock(m_lockObject)
				{
					Continent continent = CurrentContinent;
					
					if((continent != null) && (continent.Countries != null)
						&& (m_countryIndex >= 0) && (m_countryIndex < continent.Countries.Count))
					{
						return continent.Countries[m_countryIndex];
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public string[] IsoAlphaThreeCodes
		{
			get
			{
				lock(m_lockObject)
				{
					List<string> codes = new List<string>();
					
					for(int i = 0; i < CurrentContinent.Countries.Count; i++)
					{
						codes.Add(CurrentContinent.Countries[i].IsoAlphaThreeCode);
					}
					
					return codes.ToArray();
				}
			}
		}
		
		public Continent NextContinent
		{
			get
			{
				lock(m_lockObject)
				{
					if((m_continents != null) && (m_continents.Count > 0))
					{
						int nextIndex = m_continentIndex + 1;
						
						if(nextIndex >= m_continents.Count)
						{
							nextIndex = 0;
						}
						
						return m_continents[nextIndex];
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public Country NextCountry
		{
			get
			{
				lock(m_lockObject)
				{
					Continent continent = CurrentContinent;
					
					if(continent != null)
					{
						if((continent.Countries != null) && (continent.Countries.Count > 0))
						{
							int nextIndex = m_countryIndex + 1;
							
							if(nextIndex >= continent.Countries.Count)
							{
								nextIndex = 0;
							}
							
							return continent.Countries[nextIndex];
						}
						else
						{
							return null;
						}
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public bool OnlyUseVertical
		{
			get
			{
				lock(m_lockObject)
				{
					return m_onlyUseVertical;
				}
			}
			
			set
			{
				lock(m_lockObject)
				{
					m_onlyUseVertical = value;
				}
			}
		}
		
		public List<Country> PinnedCountries
		{
			get
			{
				lock(m_lockObject)
				{
					return m_pinnedCountries;
				}
			}
		}
		
		public Continent PreviousContinent
		{
			get
			{
				lock(m_lockObject)
				{
					if((m_continents != null) && (m_continents.Count > 0))
					{
						int prevIndex = m_continentIndex - 1;
						
						if(prevIndex < 0)
						{
							prevIndex = m_continents.Count - 1;
						}
						
						return m_continents[prevIndex];
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public Country PreviousCountry
		{
			get
			{
				lock(m_lockObject)
				{
					Continent continent = CurrentContinent;
					
					if(continent != null)
					{
						if((continent.Countries != null) && (continent.Countries.Count > 0))
						{
							int prevIndex = m_countryIndex - 1;
							
							if(prevIndex < 0)
							{
								prevIndex = continent.Countries.Count - 1;
							}
							
							return continent.Countries[prevIndex];
						}
						else
						{
							return null;
						}
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public CountrySelectorController()
		{
			m_continentIndex = 0;
			m_countryIndex = 0;
			
			m_pinnedCountries = new List<Country>();
			
			m_continentsHorizontal = true;
			m_onlyUseVertical = false;
			
			//load continents and countries
			m_loader = new ContinentCountryXmlLoader();
			m_continents = m_loader.LoadContinents();
			
			//sort continents and countries
			m_continents.Sort(new Continent.ContinentComparer());
			
			foreach(Continent continent in m_continents)
			{
				continent.Countries.Sort(new Country.CountryComparer());
			}
		}
		
		public Country GetCountryForIsoAlphaThreeCode(string isoAlphaThreeCode)
		{
			foreach(Continent continent in m_continents)
			{
				foreach(Country country in continent.Countries)
				{
					if(country.IsoAlphaThreeCode == isoAlphaThreeCode)
					{
						return country;
					}
				}
			}
			
			return null;
		}
		
		public void SwitchToCountry(string isoAlphaThreeCode)
		{
			lock(m_lockObject)
			{
				for(int i = 0; i < m_continents.Count; i++)
				{
					for(int j = 0; j < m_continents[i].Countries.Count; j++)
					{
						if(m_continents[i].Countries[j].IsoAlphaThreeCode == isoAlphaThreeCode)
						{
							m_continentIndex = i;
							m_countryIndex = j;
							
							OnPropertyChanged("CurrentContinent");
							OnPropertyChanged("CurrentCountry");
							
							i = int.MaxValue;
							j = int.MaxValue;
						}
					}
				}
			}
		}
		
		public void SwitchToNextContinent()
		{
			lock(m_lockObject)
			{
				m_continentIndex++;
				
				if(m_continentIndex >= m_continents.Count)
				{
					m_continentIndex = 0;
				}
				
				m_countryIndex = 0;
				
				OnPropertyChanged("CurrentContinent");
			}
		}
		
		public void SwitchToPreviousContinent()
		{
			lock(m_lockObject)
			{
				m_continentIndex--;
				
				if(m_continentIndex < 0)
				{
					m_continentIndex = m_continents.Count - 1;
				}
				
				m_countryIndex = 0;
				
				OnPropertyChanged("CurrentContinent");
			}
		}
		
		public void SwitchToNextCountry()
		{
			lock(m_lockObject)
			{
				m_countryIndex++;
				
				if(m_countryIndex >= m_continents[m_continentIndex].Countries.Count)
				{
					m_countryIndex = 0;
				}
				
				OnPropertyChanged("CurrentCountry");
			}
		}
		
		public void SwitchToPreviousCountry()
		{
			lock(m_lockObject)
			{
				m_countryIndex--;
				
				if(m_countryIndex < 0)
				{
					m_countryIndex = m_continents[m_continentIndex].Countries.Count - 1;
				}
				
				OnPropertyChanged("CurrentCountry");
			}
		}
		
		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			//directions
			if((args.HasDirection(InputDirection.Left) == true) && (m_onlyUseVertical == false))
			{
				if(m_continentsHorizontal == true)
				{
					SwitchToPreviousContinent();
				}
				else
				{
					SwitchToPreviousCountry();
				}
			}
			
			if((args.HasDirection(InputDirection.Right) == true) && (m_onlyUseVertical == false))
			{
				if(m_continentsHorizontal == true)
				{
					SwitchToNextContinent();
				}
				else
				{
					SwitchToNextCountry();
				}
			}
			
			if(args.HasDirection(InputDirection.Up) == true)
			{
				if(m_continentsHorizontal == true)
				{
					SwitchToPreviousCountry();
				}
				else
				{
					SwitchToPreviousContinent();
				}
			}
			
			if(args.HasDirection(InputDirection.Down) == true)
			{
				if(m_continentsHorizontal == true)
				{
					SwitchToNextCountry();
				}
				else
				{
					SwitchToNextContinent();
				}
			}
			
			//confirm
			
			//cancel
		}
		
		protected void OnPropertyChanged(string propertyName)
		{
			if((PropertyChanged != null) && (string.IsNullOrEmpty(propertyName) == false))
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
