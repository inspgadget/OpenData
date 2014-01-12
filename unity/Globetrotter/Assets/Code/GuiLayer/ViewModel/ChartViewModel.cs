using System;
using System.Collections.Generic;
using System.Linq;

using Globetrotter.ApplicationLayer;
using Globetrotter.DataLayer;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class ChartViewModel : ViewModelBase
	{
		private DataController m_dataController;

		private WorldBankData m_worldBankData;
		private DataPoint[] m_dataPoints;
		private string[] m_seriesNames;

		private double m_min;
		private double m_max;

		private int m_currDataPointIndex;

		public DataPoint CurrentDataPoint
		{
			get
			{
				lock(m_lockObj)
				{
					if((m_currDataPointIndex <= -1) || (m_dataPoints.Length <= 0))
					{
						return null;
					}

					return m_dataPoints[m_currDataPointIndex];
				}
			}
		}

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

		public WorldBankData Data
		{
			get
			{
				lock(m_lockObj)
				{
					return m_worldBankData;
				}
			}
		}

		public double Max
		{
			get
			{
				lock(m_lockObj)
				{
					return m_max;
				}
			}
		}

		public double Min
		{
			get
			{
				lock(m_lockObj)
				{
					return m_min;
				}
			}
		}

		public DataPoint NextDataPoint
		{
			get
			{
				lock(m_lockObj)
				{
					if(m_dataPoints.Length <= 0)
					{
						return null;
					}

					int index = m_currDataPointIndex + 1;

					if(index >= m_dataPoints.Length)
					{
						index = 0;
					}

					return m_dataPoints[index];
				}
			}
		}
		
		public DataPoint PreviousDataPoint
		{
			get
			{
				lock(m_lockObj)
				{
					if(m_dataPoints.Length <= 0)
					{
						return null;
					}

					int index = m_currDataPointIndex - 1;
					
					if(index < 0)
					{
						index = m_dataPoints.Length - 1;
					}
					
					return m_dataPoints[index];
				}
			}
		}

		public string[] SeriesNames
		{
			get
			{
				lock(m_lockObj)
				{
					return m_seriesNames;
				}
			}
		}

		public ChartViewModel(DataController dataController)
			: base()
		{
			m_dataController = dataController;
			m_dataController.WorldBankDataFetched += WorldBankDataFetchedHandler;

			m_worldBankData = null;
			m_dataPoints = new DataPoint[0];
			m_seriesNames = new string[5];

			m_min = 0.0;
			m_max = 0.0;

			m_currDataPointIndex = -1;
		}

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					int scroll = 0;
					
					if(args.HasInputType(InputType.ScrollLeft) == true)
					{
						scroll = -1;
					}
					
					if(args.HasInputType(InputType.ScrollRight) == true)
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
						int currDataPointIndex = m_currDataPointIndex + scroll;

						while(currDataPointIndex < 0)
						{
							currDataPointIndex = m_dataPoints.Length + currDataPointIndex;
						}

						while(currDataPointIndex >= m_dataPoints.Length)
						{
							currDataPointIndex = 0 + (currDataPointIndex - m_dataPoints.Length);
						}

						m_currDataPointIndex = currDataPointIndex;
					}
				}
			}
		}

		public void WorldBankDataFetchedHandler(object sender, WorldBankDataFetchedEventArgs args)
		{
			if(args.Data != null)
			{
				lock(m_lockObj)
				{
					m_worldBankData = args.Data;

					m_min = double.MaxValue;
					m_max = double.MinValue;

					List<DataPoint> dataPoints = new List<DataPoint>();

					//divide items by year
					Dictionary<int, List<WorldBankDataItem>> items = new Dictionary<int, List<WorldBankDataItem>>();

					foreach(WorldBankDataItem item in m_worldBankData.Items)
					{
						List<WorldBankDataItem> itemsList = null;
						items.TryGetValue(item.Year, out itemsList);

						if(itemsList == null)
						{
							itemsList = new List<WorldBankDataItem>();
							items[item.Year] = itemsList;
						}

						itemsList.Add(item);

						if(item.Value < m_min)
						{
							m_min = item.Value;
						}

						if(item.Value > m_max)
						{
							m_max = item.Value;
						}
					}

					//series names
					Dictionary<int, List<WorldBankDataItem>>.KeyCollection keys = items.Keys;

					if(keys.Count > 0)
					{
						int n = items[keys.First()].Count;
						string[] seriesNames = new string[n];

						for(int k = 0; k < n; k++)
						{
							seriesNames[k] = items[keys.First()][k].Country;
						}

						Array.Sort<string>(seriesNames);

						m_seriesNames = new string[5];

						for(int l = 0; l < 5; l++)
						{
							m_seriesNames[l] = l < n ? seriesNames[l] : string.Empty;
						}
					}

					//create data points
					int[] years = keys.ToArray();
					Array.Sort<int>(years);

					for(int i = 0; i < years.Length; i++)
					{
						double[] data = new double[items[years[i]].Count];

						for(int j = 0; j < data.Length; j++)
						{
							data[j] = items[years[i]][j].Value;
						}

						dataPoints.Add(new DataPoint(years[i], data));
					}

					m_dataPoints = dataPoints.ToArray();

					if(m_dataPoints.Length == 1)
					{
						m_currDataPointIndex = 0;
					}
					else if(m_dataPoints.Length > 1)
					{
						m_currDataPointIndex = m_dataPoints.Length - 2;
					}
				}
			}
		}
	}
}
