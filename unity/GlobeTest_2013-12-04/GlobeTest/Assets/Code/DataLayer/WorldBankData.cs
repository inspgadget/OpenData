using System;
using System.Collections.Generic;

namespace GlobeTest.DataLayer
{
	public class WorldBankData
	{
		private string m_indicatorName;
		private string m_isoAlphaThreeCode;
		private bool m_isAggregate;
		private IList<WorldBankDataItem> m_items;
		
		public string IndicatorName
		{
			get
			{
				return m_indicatorName;
			}
		}
		
		public bool IsAggregate
		{
			get
			{
				return m_isAggregate;
			}
		}
		
		public string IsoAlphaThreeCode
		{
			get
			{
				return m_isoAlphaThreeCode;
			}
		}
		
		public IList<WorldBankDataItem> Items
		{
			get
			{
				return m_items;
			}
			
			set
			{
				m_items = value;
			}
		}
		
		public WorldBankData(string indicatorName, string isoAlphaThreeCode, bool isAggregate, IList<WorldBankDataItem> items)
		{
			m_indicatorName = indicatorName;
			m_isoAlphaThreeCode = isoAlphaThreeCode;
			m_isAggregate = isAggregate;
			m_items = items;
		}
	}
}
