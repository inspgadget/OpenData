using System;
using System.Collections.Generic;

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

			m_currCountryIndex = 0;
			m_countries = m_countriesController.Countries;
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
					if(args.InputTypes.And(InputType.ClickDouble) == InputType.ClickDouble)
					{
						if(m_currCountryIndex >= 0)
						{
							m_countriesController.AddCountry();
						}
					}
				}

				//todo: other inputs
			}
		}
	}
}
