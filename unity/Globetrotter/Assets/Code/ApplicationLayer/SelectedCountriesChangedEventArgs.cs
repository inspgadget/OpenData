using System;
using System.Collections.Generic;

using Globetrotter.DomainLayer;

namespace Globetrotter.ApplicationLayer
{
	public class SelectedCountriesChangedEventArgs : EventArgs
	{
		private Country[] m_selectedCountries;

		public Country[] SelectedCountries
		{
			get
			{
				return m_selectedCountries;
			}
		}

		public SelectedCountriesChangedEventArgs(Country[] selectedCountries)
			: base()
		{
			m_selectedCountries = selectedCountries;
		}
	}
}
