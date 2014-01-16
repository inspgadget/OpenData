using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Globetrotter.DataLayer;
using Globetrotter.DomainLayer;
using Globetrotter.NetworkLayer;
using Globetrotter.PersistenceLayer;

namespace Globetrotter.ApplicationLayer
{
	public class DataController
	{
		private object m_lockObject = new object();

		public delegate void ChartFetchedEventHandler(object sender, ChartFetchedEventArgs args);
		public event ChartFetchedEventHandler ChartFetched;

		public delegate void WorldBankDataFetchedEventHandler(object sender, WorldBankDataFetchedEventArgs args);
		public event WorldBankDataFetchedEventHandler WorldBankDataFetched;

		private WorldBankIndicator m_currIndicator;
		private Dictionary<string, WorldBankIndicator> m_indicators;

		private int m_yearFrom;
		private int m_yearTo;

		public WorldBankIndicator CurrentIndicator
		{
			get
			{
				lock(m_lockObject)
				{
					return m_currIndicator;
				}
			}

			set
			{
				lock(m_lockObject)
				{
					m_currIndicator = value;
				}
			}
		}

		public WorldBankIndicator[] Indicators
		{
			get
			{
				lock(m_lockObject)
				{
					return m_indicators.Values.ToArray();
				}
			}
		}
		
		public int YearFrom
		{
			get
			{
				lock(m_lockObject)
				{
					return m_yearFrom;
				}
			}
			
			set
			{
				lock(m_lockObject)
				{
					m_yearFrom = value;
				}
			}
		}

		public int YearTo
		{
			get
			{
				lock(m_lockObject)
				{
					return m_yearTo;
				}
			}

			set
			{
				lock(m_lockObject)
				{
					m_yearTo = value;
				}
			}
		}

		public DataController()
		{
			m_indicators = new Dictionary<string, WorldBankIndicator>();
			IList<WorldBankIndicator> indicators = new IndicatorsXmlLoader().loadIndicators();

			if(indicators != null)
			{
				foreach(WorldBankIndicator indicator in indicators)
				{
					m_indicators[indicator.Code] = indicator;
				}

				if(indicators.Count > 0)
				{
					m_currIndicator = indicators[0];
				}
				else
				{
					m_currIndicator = null;
				}
			}

			m_yearTo = DateTime.Now.Year;
			m_yearFrom = m_yearTo - 10;
		}

		public byte[] FetchChart(Country[] countries, WorldBankIndicator indicator)
		{
			return FetchChart(countries, indicator, YearFrom, YearTo);
		}
		
		public byte[] FetchChart(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo)
		{
			HttpResponse response = new HttpRequestController().GetResponse(getChartRequestUrl(countries, indicator, yearFrom, yearTo), 8080);

			if(response.StatusCode != 200)
			{
				UnityEngine.Debug.Log("chart not fetched: " + response.StatusCode);
			}

			return response.Data;
		}
		
		public void FetchChartAsync(Country[] countries, WorldBankIndicator indicator)
		{
			Thread thread = new Thread(delegate(){
				byte[] data = FetchChart(countries, indicator, YearFrom, YearTo);

				OnChartFetched(data);
			});
			
			thread.IsBackground = true;
			thread.Start();
		}
		
		public void FetchChartAsync(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo)
		{
			Thread thread = new Thread(delegate(){
				byte[] data = FetchChart(countries, indicator, yearFrom, yearTo);

				OnChartFetched(data);
			});
			
			thread.IsBackground = true;
			thread.Start();
		}
		
		public WorldBankData FetchData(Country[] countries, WorldBankIndicator indicator)
		{
			return FetchData(countries, indicator, YearFrom, YearTo);
		}

		public WorldBankData FetchData(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo)
		{
			return indicator.FetchData(CountriesAsCodes(countries), yearFrom, yearTo);
		}
		
		public void FetchDataAsync(Country[] countries, WorldBankIndicator indicator)
		{
			Thread thread = new Thread(delegate(){
				WorldBankData data = FetchData(countries, indicator, YearFrom, YearTo);
				
				OnWorldBankDataFetched(data);
			});
			
			thread.IsBackground = true;
			thread.Start();
		}

		public void FetchDataAsync(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo)
		{
			Thread thread = new Thread(delegate(){
				WorldBankData data = FetchData(countries, indicator, yearFrom, yearTo);

				OnWorldBankDataFetched(data);
			});

			thread.IsBackground = true;
			thread.Start();
		}

		public WorldBankIndicator GetIndicator(string code)
		{
			lock(m_lockObject)
			{
				return m_indicators[code];
			}
		}

		private string[] CountriesAsCodes(Country[] countries)
		{
			string[] codes = new string[countries.Length];

			for(int i = 0; i < codes.Length; i++)
			{
				codes[i] = countries[i].IsoAlphaThreeCode;
			}

			return codes;
		}

		private string getChartRequestUrl(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("http://localhost:8080/GlobetrotterChartWebService/ChartGeneratorServlet?");

			//countries
			sb.Append("?countries=");

			for(int i = 0; i < countries.Length; i++)
			{
				sb.Append(countries[i].IsoAlphaThreeCode);

				if(i < (countries.Length - 1))
				{
					sb.Append(';');
				}
			}

			//indicator
			sb.Append("&indicator=");
			sb.Append(indicator.Code);

			//yearfrom
			sb.Append("&yearfrom=");
			sb.Append(yearTo);
			
			//yearto
			sb.Append("&yearto=");
			sb.Append(yearTo);

			//charttype
			sb.Append("&charttype=");
			sb.Append("linechart");

			return sb.ToString();
		}

		protected void OnChartFetched(byte[] data)
		{
			lock(m_lockObject)
			{
				if(ChartFetched != null)
				{
					ChartFetched(this, new ChartFetchedEventArgs(data));
				}
			}
		}

		protected void OnWorldBankDataFetched(WorldBankData data)
		{
			lock(m_lockObject)
			{
				if(WorldBankDataFetched != null)
				{
					WorldBankDataFetched(this, new WorldBankDataFetchedEventArgs(data));
				}
			}
		}
	}
}
