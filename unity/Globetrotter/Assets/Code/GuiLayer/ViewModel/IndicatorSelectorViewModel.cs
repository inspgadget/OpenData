using System;
using System.Collections.Generic;

using Globetrotter.ApplicationLayer;
using Globetrotter.DataLayer;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class IndicatorSelectorViewModel : ViewModelBase
	{
		private CountriesController m_countriesController;
		private DataController m_dataController;

		private int m_currIndicatorIndex;
		private WorldBankIndicator[] m_indicators;
		private bool m_isFetching;

		public WorldBankIndicator CurrentIndicator
		{
			get
			{
				lock(m_lockObj)
				{
					return m_indicators[m_currIndicatorIndex];
				}
			}
		}

		public bool IsFetching
		{
			get
			{
				lock(m_lockObj)
				{
					return m_isFetching;
				}
			}
		}

		public WorldBankIndicator NextIndicator
		{
			get
			{
				lock(m_lockObj)
				{
					int nextIndicatorIndex = m_currIndicatorIndex + 1;
					
					if(nextIndicatorIndex >= m_indicators.Length)
					{
						nextIndicatorIndex = 0;
					}
					
					return m_indicators[nextIndicatorIndex];
				}
			}
		}
		
		public WorldBankIndicator PreviousIndicator
		{
			get
			{
				lock(m_lockObj)
				{
					int previousIndicatorIndex = m_currIndicatorIndex - 1;
					
					if(previousIndicatorIndex < 0)
					{
						previousIndicatorIndex = m_indicators.Length - 1;
					}
					
					return m_indicators[previousIndicatorIndex];
				}
			}
		}

		public IndicatorSelectorViewModel(CountriesController countriesController, DataController dataController)
			: base()
		{
			m_countriesController = countriesController;
			m_dataController = dataController;
			m_dataController.ChartFetched += ChartFetchedHandler;
			m_dataController.WorldBankDataFetched += WorldBankDataFetchedHandler;

			m_indicators = m_dataController.Indicators;
			m_currIndicatorIndex = GetIndexOfIndicator(dataController.CurrentIndicator);
			m_isFetching = false;
		}
		
		private int GetIndexOfIndicator(WorldBankIndicator indicator)
		{
			if(indicator != null)
			{
				for(int i = 0; i < m_indicators.Length; i++)
				{
					if(m_indicators[i] == indicator)
					{
						return i;
					}
				}
			}
			
			return -1;
		}

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			if(ReactOnInput == true)
			{
				if(args.HasInputType(InputType.ClickDouble) == true)
				{
					lock(m_lockObj)
					{
						if((m_currIndicatorIndex >= 0) && (m_isFetching == false) && (m_countriesController.SelectedCountries.Length > 0))
						{
							m_isFetching = true;

							m_dataController.CurrentIndicator = m_indicators[m_currIndicatorIndex];
							m_dataController.FetchDataAsync(m_countriesController.SelectedCountries, m_indicators[m_currIndicatorIndex]);
						}
					}

					OnPropertyChanged("IsFetching");
				}
				
				int scroll = 0;
				
				if(args.HasInputType(InputType.ScrollRight) == true)
				{
					scroll = -1;
				}
				
				if(args.HasInputType(InputType.ScrollLeft) == true)
				{
					scroll = 1;
				}
				
				if(args.HasInputType(InputType.WipeLeft) == true)
				{
					scroll = -10;
				}
				
				if(args.HasInputType(InputType.WipeRight) == true)
				{
					scroll = 10;
				}

				if(scroll != 0)
				{
					lock(m_lockObj)
					{
						int currIndicatorIndex = m_currIndicatorIndex + scroll;
						
						if(currIndicatorIndex != m_currIndicatorIndex)
						{
							while(currIndicatorIndex < 0)
							{
								currIndicatorIndex = m_indicators.Length + currIndicatorIndex;
							}

							while(currIndicatorIndex >= m_indicators.Length)
							{
								currIndicatorIndex = 0 + (currIndicatorIndex - m_indicators.Length);
							}
							
							m_currIndicatorIndex = currIndicatorIndex;
						}
					}
				}
			}
		}
		
		public void ChartFetchedHandler(object sender, ChartFetchedEventArgs args)
		{
			lock(m_lockObj)
			{
				m_isFetching = false;
			}

			OnPropertyChanged("IsFetching");
		}

		public void WorldBankDataFetchedHandler(object sender, WorldBankDataFetchedEventArgs args)
		{
			lock(m_lockObj)
			{
				m_isFetching = false;
			}

			OnPropertyChanged("IsFetching");
		}
	}
}
