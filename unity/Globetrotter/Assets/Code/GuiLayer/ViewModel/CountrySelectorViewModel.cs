using System;
using System.Collections.Generic;
using System.Linq;

using Globetrotter.ApplicationLayer;
using Globetrotter.DomainLayer;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class CountrySelectorViewModel : ViewModelBase
	{
		private CountriesController m_countriesController;

		private int m_currCountryIndex;
		private Country[] m_countries;

		public int CurrCountryIndex {
			get {
				return m_currCountryIndex;
			}
			set {
				m_currCountryIndex = value;
				m_countriesController.CurrentCountry = m_countries[m_currCountryIndex];
				OnPropertyChanged("CurrentCountry");
			}
		}

		public Country CurrentCountry
		{
			get
			{
				lock(m_lockObj)
				{
					return m_countriesController.CurrentCountry;
				}
			}
		}

		public Country NextCountry
		{
			get
			{
				lock(m_lockObj)
				{
					int nextCountryIndex = m_currCountryIndex + 1;
					
					if(nextCountryIndex >= m_countries.Length)
					{
						nextCountryIndex = 0;
					}

					return m_countries[nextCountryIndex];
				}
			}
		}

		public Country PreviousCountry
		{
			get
			{
				lock(m_lockObj)
				{
					int prevCountryIndex = m_currCountryIndex - 1;
					
					if(prevCountryIndex < 0)
					{
						prevCountryIndex = m_countries.Length - 1;
					}

					return m_countries[prevCountryIndex];
				}
			}
		}

		public CountrySelectorViewModel(CountriesController countriesController)
			: base()
		{
			m_countriesController = countriesController;

			m_countries = m_countriesController.Countries;
			Array.Sort(m_countries, new Country.CountryComparer(Country.SortProperty.Name));

			m_currCountryIndex = GetIndexOfCountry(m_countriesController.CurrentCountry);
		}

		
		public int GetIndexOfCountry(string IsoAlphaThreeCode)
		{
			if(!string.IsNullOrEmpty(IsoAlphaThreeCode))
			{
				for(int i = 0; i < m_countries.Length; i++)
				{
					if(m_countries[i].IsoAlphaThreeCode == IsoAlphaThreeCode)
					{
						return i;
					}
				}
			}
			
			return -1;
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

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					if(args.HasInputType(InputType.ClickDouble) == true)
					{
						if(m_currCountryIndex >= 0)
						{
							m_countriesController.AddCountry();
						}
					}

					int scroll = 0;

					if(args.HasInputType(InputType.ScrollLeft) == true)
					{
						scroll = -1;
					}

					if(args.HasInputType(InputType.ScrollRight) == true)
					{
						scroll = 1;
					}

					if(args.HasInputType(InputType.WipeLeft) == true)
					{
						scroll = -10;
					}
					
					if(args.HasInputType(InputType.WipeRight) == true)
					{
						scroll = 10;
					}

					int currCountryIndex = m_currCountryIndex + scroll;

					if(currCountryIndex != m_currCountryIndex)
					{
						while(currCountryIndex < 0)
						{
							currCountryIndex = m_countries.Length + currCountryIndex;
						}

						while(currCountryIndex >= m_countries.Length)
						{
							currCountryIndex = 0 + (currCountryIndex - m_countries.Length);
						}

						m_currCountryIndex = currCountryIndex;
						m_countriesController.CurrentCountry = m_countries[m_currCountryIndex];

						OnPropertyChanged("CurrentCountry");
					}
				}
			}
		}
	}
}
