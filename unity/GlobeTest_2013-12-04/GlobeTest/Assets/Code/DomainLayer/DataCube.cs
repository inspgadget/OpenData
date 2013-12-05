using System;
using System.Collections.Generic;

using GlobeTest.DataLayer;

namespace GlobeTest.DomainLayer
{
	public class DataCube
	{
		private Dictionary<string, List<WorldBankDataItem>> m_data;
		
		public DataCube(WorldBankData worldBankData)
		{
			m_data = new Dictionary<string, List<WorldBankDataItem>>();
			
			foreach(WorldBankDataItem item in worldBankData.Items)
			{
				if(m_data.ContainsKey(item.Country) == false)
				{
					m_data[item.Country] = new List<WorldBankDataItem>();
				}
				
				m_data[item.Country].Add(item);
			}
			
			foreach(string key in m_data.Keys)
			{
				m_data[key].Sort(new WorldBankDataItem.WorldBankDataItemComparer());
			}
		}
	}
}
