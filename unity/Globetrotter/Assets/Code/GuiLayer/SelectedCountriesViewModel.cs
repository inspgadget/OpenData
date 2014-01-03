using System;
using System.Collections.Generic;
using System.Linq;

using Globetrotter.ApplicationLayer;
using Globetrotter.DomainLayer;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer
{
	public class SelectedCountriesViewModel : ViewModelBase
	{
		private CountriesController m_countriesController;

		private int m_currCountryIndex;

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
					return m_countriesController.SelectedCountries;
				}
			}
		}

		public SelectedCountriesViewModel(CountriesController countriesController)
		{
			m_countriesController = countriesController;

			if(m_countriesController.SelectedCountries.Length > 0)
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
					if(args.InputTypes.And(InputType.WipeLeft) == InputType.WipeLeft)
					{
						m_countriesController.RemoveCountry(m_countriesController.Countries[m_currCountryIndex]);

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

					if(args.InputTypes.And(InputType.ScrollUp) == InputType.ScrollUp)
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

					if(args.InputTypes.And(InputType.ScrollDown) == InputType.ScrollDown)
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
	}
}
