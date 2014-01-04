﻿using System;
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
					return m_dataController.CurrentIndicator;
				}
			}
		}

		public WorldBankIndicator NextIndicator
		{
			get
			{
				lock(m_lockObj)
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
		}
		
		public WorldBankIndicator PreviousIndicator
		{
			get
			{
				lock(m_lockObj)
				{
					lock(m_lockObj)
					{
						int previousIndicatorIndex = m_currIndicatorIndex + 1;
						
						if(previousIndicatorIndex >= m_indicators.Length)
						{
							previousIndicatorIndex = 0;
						}
						
						return m_indicators[previousIndicatorIndex];
					}
				}
			}
		}

		public IndicatorSelectorViewModel(CountriesController countriesController, DataController dataController)
			: base()
		{
			m_countriesController = countriesController;
			m_dataController = dataController;

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
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					if(args.InputTypes.And(InputType.ClickDouble) == InputType.ClickDouble)
					{
						if((m_currIndicatorIndex >= 0) && (m_isFetching == false))
						{
							m_isFetching = true;

							m_dataController.CurrentIndicator = m_indicators[m_currIndicatorIndex];
							m_dataController.FetchDataAsync(m_countriesController.SelectedCountries, m_indicators[m_currIndicatorIndex]);
						}
					}
				}
			}
		}

		public void WorldBankDataFetchedHandler(object sender, WorldBankDataFetchedEventArgs args)
		{
			lock(m_lockObj)
			{
				m_isFetching = false;
			}
		}
	}
}