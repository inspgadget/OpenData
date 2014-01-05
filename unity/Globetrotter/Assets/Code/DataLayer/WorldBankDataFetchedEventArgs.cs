using UnityEngine;
using System.Collections;

namespace Globetrotter.DataLayer
{
	public class WorldBankDataFetchedEventArgs
	{
		private WorldBankData m_data;
		
		public WorldBankData Data
		{
			get
			{
				return m_data;
			}
		}
		
		public WorldBankDataFetchedEventArgs(WorldBankData data)
			: base()
		{
			m_data = data;
		}
	}
}
