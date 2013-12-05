using System;
using System.Collections.Generic;
using System.ComponentModel;

using GlobeTest.DataLayer;
using GlobeTest.InputLayer;
using GlobeTest.PersistenceLayer;

namespace GlobeTest.ApplicationLayer
{
	public class DataController : INotifyPropertyChanged
	{
		private object m_lockObject = new object();
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		private List<WorldBankIndicator> m_indicators;
		private int m_currIndex;
		
		private WorldBankData m_data;
		private bool m_fetching;
		
		public WorldBankIndicator CurrentIndicator
		{
			get
			{
				lock(m_lockObject)
				{
					if(m_indicators.Count > 0)
					{
						return m_indicators[m_currIndex];
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public WorldBankData Data
		{
			get
			{
				lock(m_lockObject)
				{
					return m_data;
				}
			}
		}
		
		public bool Fetching
		{
			get
			{
				lock(m_lockObject)
				{
					return m_fetching;
				}
			}
		}
		
		public WorldBankIndicator NextIndicator
		{
			get
			{
				lock(m_lockObject)
				{
					if(m_indicators.Count > 0)
					{
						if((m_currIndex + 1) >= m_indicators.Count)
						{
							return m_indicators[0];
						}
						else
						{
							return m_indicators[(m_currIndex + 1)];
						}
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public WorldBankIndicator PreviousIndicator
		{
			get
			{
				lock(m_lockObject)
				{
					if(m_indicators.Count > 0)
					{
						if(m_currIndex == 0)
						{
							return m_indicators[(m_indicators.Count - 1)];
						}
						else
						{
							return m_indicators[(m_currIndex - 1)];
						}
					}
					else
					{
						return null;
					}
				}
			}
		}
		
		public DataController()
		{
			m_currIndex = 0;
			
			m_data = null;
			m_fetching = false;
			
			m_indicators = new IndicatorsXmlLoader().loadIndicators();
			m_indicators.Sort(new WorldBankIndicator.WorldBankIndicatorComparer());
			
			foreach(WorldBankIndicator indicator in m_indicators)
			{
				indicator.WorldBankDataFetched += WorldBankDataFetchedHandler;
			}
		}
		
		public void FetchData(string[] countries)
		{
			lock(m_lockObject)
			{
				if(m_fetching == false)
				{
					m_fetching = true;
					OnPropertyChanged("Fetching");
					
					CurrentIndicator.FetchData(countries);
				}
			}
		}
		
		public void SwitchToNextIndicator()
		{
			lock(m_lockObject)
			{
				if(m_fetching == false)
				{
					if((m_currIndex + 1) >= m_indicators.Count)
					{
						m_currIndex = 0;
					}
					else
					{
						m_currIndex++;
					}
				}
				
				OnPropertyChanged("CurrentIndicator");
			}
		}
		
		public void SwitchToPreviousIndicator()
		{
			lock(m_lockObject)
			{
				if(m_fetching == false)
				{
					if(m_currIndex == 0)
					{
						m_currIndex = m_indicators.Count - 1;
					}
					else
					{
						m_currIndex--;
					}
				}
				
				OnPropertyChanged("CurrentIndicator");
			}
		}
		
		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			//directions
			if(args.HasDirection(InputDirection.Left) == true)
			{
				SwitchToPreviousIndicator();
			}
			
			if(args.HasDirection(InputDirection.Right) == true)
			{
				SwitchToNextIndicator();
			}
			
			//confirm
			
			//cancel
		}
		
		public void WorldBankDataFetchedHandler(object sender, WorldBankDataFetchedEventArgs args)
		{
			lock(m_lockObject)
			{
				m_data = args.Data;
				
				if(m_data.Items is List<WorldBankDataItem>)
				{
					((List<WorldBankDataItem>)m_data.Items).Sort(new WorldBankDataItem.WorldBankDataItemComparer());
				}
				
				m_fetching = false;
				
				OnPropertyChanged("Data");
				OnPropertyChanged("Fetching");
			}
		}
		
		protected void OnPropertyChanged(string propertyName)
		{
			if((PropertyChanged != null) && (string.IsNullOrEmpty(propertyName) == false))
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
