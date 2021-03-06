﻿using System;
using System.Collections.Generic;

namespace Globetrotter.DomainLayer
{
	public class Country
	{
		private object m_lockObj = new object();

		private string m_name;
		private string m_isoAlphaThreeCode;
		private string m_isoTwoCode;
		private string m_continent;
		private string m_capitalCity;
		private int m_population;
		private double m_surfaceArea;
		private double m_latitude;
		private double m_longitude;

		public string CapitalCity
		{
			get
			{
				lock(m_lockObj)
				{
					return m_capitalCity;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_capitalCity = value;
				}
			}
		}

		public string Continent
		{
			get
			{
				lock(m_lockObj)
				{
					return m_continent;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_continent = value;
				}
			}
		}

		public string IsoAlphaThreeCode
		{
			get
			{
				lock(m_lockObj)
				{
					return m_isoAlphaThreeCode;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_isoAlphaThreeCode = value;
				}
			}
		}

		public string IsoTwoCode
		{
			get
			{
				lock(m_lockObj)
				{
					return m_isoTwoCode;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_isoTwoCode = value;
				}
			}
		}

		public double Latitude
		{
			get
			{
				lock(m_lockObj)
				{
					return m_latitude;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_latitude = value;
				}
			}
		}

		public double Longitude
		{
			get
			{
				lock(m_lockObj)
				{
					return m_longitude;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_longitude = value;
				}
			}
		}

		public string Name
		{
			get
			{
				lock(m_lockObj)
				{
					return m_name;
				}
			}

			set
			{
				lock(m_lockObj)
				{
				m_name = value;
				}
			}
		}

		public int Population
		{
			get
			{
				lock(m_lockObj)
				{
					return m_population;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_population = value;
				}
			}
		}

		public double SurfaceArea
		{
			get
			{
				lock(m_lockObj)
				{
					return m_surfaceArea;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_surfaceArea = value;
				}
			}
		}

		public Country()
		{
			m_name = string.Empty;
			m_isoAlphaThreeCode = string.Empty;
			m_isoTwoCode = string.Empty;
			m_continent = string.Empty;
			m_capitalCity = string.Empty;
			m_population = 0;
			m_surfaceArea = 0.0;
			m_latitude = 0.0;
			m_longitude = 0.0;
		}

		public class CountryComparer : IComparer<Country>
		{
			private SortProperty m_sortProperty;

			public CountryComparer(SortProperty sortProperty)
			{
				m_sortProperty = sortProperty;
			}

			public int Compare(Country a, Country b)
			{
				switch(m_sortProperty)
				{
					case SortProperty.IsoAlphaThreeCode:
						return a.IsoAlphaThreeCode.CompareTo(b.IsoAlphaThreeCode);

					case SortProperty.Name:
						return a.Name.CompareTo(b.Name);

					default:
						return 0;
				}
			}
		}

		public enum SortProperty
		{
			IsoAlphaThreeCode,
			Name
		}
	}
}
