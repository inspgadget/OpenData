using System;
using System.Collections.Generic;

namespace Globetrotter.DataLayer
{
	public class WorldBankDataItem
	{
		private string m_indicatorCode;
		private string m_indicatorName;
		private string m_country;
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
			m_year = 0;
			m_value = 0.0;
		}
		
		public class WorldBankDataItemComparer : IComparer<WorldBankDataItem>
		{
			public WorldBankDataItemComparer()
			{
			}
			
			public int Compare (WorldBankDataItem a, WorldBankDataItem b)
			{
				return a.Year.CompareTo(b.Year);
			}
		}
	}
}
