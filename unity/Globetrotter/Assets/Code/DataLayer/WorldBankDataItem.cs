﻿using System;
using System.Collections.Generic;

namespace Globetrotter.DataLayer
{
	public class WorldBankDataItem
	{
		private string m_indicatorCode;
		private string m_indicatorName;
		private string m_country;
		private string m_isoTwoCode;
		private int m_year;
		private double m_value;
		
		public string Country
		{
			get
			{
				return m_country;
			}
			
			set
			{
				m_country = value;
			}
		}
		
		public string IndicatorCode
		{
			get
			{
				return m_indicatorCode;
			}
			
			set
			{
				m_indicatorCode = value;
			}
		}
		
		public string IndicatorName
		{
			get
			{
				return m_indicatorName;
			}
			
			set
			{
				m_indicatorName = value;
			}
		}

		public string IsoTwoCode
		{
			get
			{
				return m_isoTwoCode;
			}

			set
			{
				m_isoTwoCode = value;
			}
		}

		public double Value
		{
			get
			{
				return m_value;
			}
			
			set
			{
				m_value = value;
			}
		}
		
		public int Year
		{
			get
			{
				return m_year;
			}
			
			set
			{
				m_year = value;
			}
		}
		
		public WorldBankDataItem()
		{
			m_indicatorCode = string.Empty;
			m_indicatorName = string.Empty;
			m_country = string.Empty;
			m_isoTwoCode = string.Empty;
			m_year = 0;
			m_value = 0.0;
		}
		
		public class WorldBankDataItemComparer : IComparer<WorldBankDataItem>
		{
			private SortProperty m_sortProperty;

			public WorldBankDataItemComparer(SortProperty sortProperty)
			{
				m_sortProperty = sortProperty;
			}
			
			public int Compare (WorldBankDataItem a, WorldBankDataItem b)
			{
				switch(m_sortProperty)
				{
					case SortProperty.Country:
						return a.Country.CompareTo(b.Country);

					case SortProperty.Year:
						return a.Year.CompareTo(b.Year);

					default:
						return 0;
				}
			}
		}

		public enum SortProperty
		{
			Country,
			Year
		}
	}
}
