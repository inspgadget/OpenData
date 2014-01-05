using System;
using System.Collections.Generic;
using System.Linq;

using Globetrotter.ApplicationLayer;
using Globetrotter.DomainLayer;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class SelectedCountriesViewModel : ViewModelBase
	{
		private CountriesController m_countriesController;

		private int m_currCountryIndex;
		private Country[] m_selectedCountries;

		public int CurrentCountryIndex
		{
			get
			{
				lock(m_lockObj)
				{
					return m_currCountryIndex;
				}
			}
		}

		public override bool ReactOnInput
		{
			get
			{
				lock(m_lockObj)
				{
					return base.ReactOnInput;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					base.ReactOnInput = value;

					if(ReactOnInput == true)
					{
						if(m_currCountryIndex <= -1)
						{
							m_currCountryIndex = 0;
						}

						int n = m_countriesController.SelectedCountries.Length;

						if(m_currCountryIndex >= n)
						{
							m_currCountryIndex = n - 1;
						}
					}
				}
			}
		}

		public Country[] SelectedCountries
		{
			get
			{
				lock(m_lockObj)
				{
					return m_selectedCountries;
				}
			}
		}

		public SelectedCountriesViewModel(CountriesController countriesController)
			: base()
		{
			m_countriesController = countriesController;
			m_countriesController.SelectedCountriesChanged += SelectedCountriesChangedHandler;

			m_selectedCountries = m_countriesController.SelectedCountries;

			if(m_selectedCountries.Length > 0)
			{
				m_currCountryIndex = 0;
			}
			else
			{
				m_currCountryIndex = -1;
			}
		}

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					if(args.HasInputType(InputType.WipeLeft) == true)
					{
						m_countriesController.RemoveCountry(m_selectedCountries[m_currCountryIndex]);

						int n = m_countriesController.SelectedCountries.Length;

						if(n == 0)
						{
							m_currCountryIndex = -1;
						}
						else if(m_currCountryIndex >= n)
						{
							m_currCountryIndex = n - 1;
						}
					}

					if(args.HasInputType(InputType.ScrollUp) == true)
					{
						if(m_currCountryIndex >= 0)
						{
							m_currCountryIndex--;

							if(m_currCountryIndex < 0)
							{
								m_currCountryIndex = m_countriesController.Countries.Length - 1;
							}
						}
					}

					if(args.HasInputType(InputType.ScrollDown) == true)
					{
						if(m_currCountryIndex >= 0)
						{
							m_currCountryIndex++;

							if(m_currCountryIndex >= m_countriesController.Countries.Length)
							{
								m_currCountryIndex = 0;
							}
						}
					}
				}
			}
		}

		public void SelectedCountriesChangedHandler(object sender, SelectedCountriesChangedEventArgs args)
		{
			lock(m_lockObj)
			{
				m_selectedCountries = args.SelectedCountries;
			}
		}
	}
}
