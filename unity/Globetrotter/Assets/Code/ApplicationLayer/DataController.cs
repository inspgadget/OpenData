using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

		private string m_currChartType;

		private string m_dataPath;
		private string m_tempDir;

		public string CurrentChartType
		{
			get
			{
				lock(m_lockObject)
				{
					return m_currChartType;
				}
			}

			set
			{
				lock(m_lockObject)
				{
					m_currChartType = value;
				}
			}
		}

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

		public string DataPath
		{
			get
			{
				lock(m_lockObject)
				{
					return m_dataPath;
				}
			}

			set
			{
				lock(m_lockObject)
				{
					m_dataPath = value;
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

			m_currChartType = "linechart";

			m_dataPath = null;

			//create temp directory
			m_tempDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/Globetrotter/temp";

			if(Directory.Exists(m_tempDir) == false)
			{
				Directory.CreateDirectory(m_tempDir);

				UnityEngine.Debug.Log("created temp dir: " + m_tempDir);
			}
			else
			{
				UnityEngine.Debug.Log("temp dir already exists: " + m_tempDir);
			}
		}

		public byte[] FetchChart(Country[] countries, WorldBankIndicator indicator, string chartType)
		{
			return FetchChart(countries, indicator, YearFrom, YearTo, chartType);
		}
		
		public byte[] FetchChart(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo, string chartType)
		{
			try
			{
				//run adapter
				Process javaAdapter = Process.Start("javaw.exe",
				                                    "-jar " + m_dataPath + "/Chart/GlobetrotterChartWebServiceAdapter.jar " +
				                                    m_tempDir + "/chart.png " + GetCountriesAsArgument(countries) +
				                                    " " + indicator.Code + " " + yearFrom + " " + yearTo + " " + chartType);
				javaAdapter.WaitForExit();

				//read image file
				using(BufferedStream bs = new BufferedStream(new FileStream(m_tempDir + "/chart.png", FileMode.Open)))
				{
					byte[] data = new byte[bs.Length];
					bs.Read(data, 0, (int)bs.Length);

					return data;
				}

				/*HttpResponse response = new HttpRequestController().GetResponse(getChartRequestUrl(countries, indicator, yearFrom, yearTo), 8080);

				if(response.StatusCode != 200)
				{
					UnityEngine.Debug.Log("chart not fetched: " + response.StatusCode);
				}

				UnityEngine.Debug.Log("default: " + System.Text.Encoding.Default.GetString(response.Data));
				UnityEngine.Debug.Log("ascii: " + System.Text.Encoding.ASCII.GetString(response.Data));
				string imagePath = System.Text.ASCIIEncoding.ASCII.GetString(response.Data);

				using(FileStream fs = new FileStream(imagePath, FileMode.Open))
				{
					byte[] data = new byte[fs.Length];
					fs.Read(data, 0, data.Length);

					UnityEngine.Debug.Log("yuhuuuuuuuuu: " + fs.Length + "; " + imagePath);

					return data;
				}*/
				
				//
				//UnityEngine.Debug.Log("chart fetched");
				//

				/*WebClient wc = new WebClient();
				Stream stream = wc.OpenRead(getChartRequestUrl(countries, indicator, yearFrom, yearTo));

				using(BufferedStream bs = new BufferedStream(wc.OpenRead(getChartRequestUrl(countries, indicator, yearFrom, yearTo))))
				{
					using(MemoryStream ms = new MemoryStream())
					{
						byte[] buffer = new byte[4092];
						int read = -1;

						while((read = bs.Read(buffer, 0, buffer.Length)) > -1)
						{
							ms.Write(buffer, 0 , read);
						}

						return ms.ToArray();
					}
				}*/

				/*WebRequest request = WebRequest.Create(getChartRequestUrl(countries, indicator, yearFrom, yearTo));
				WebResponse response = request.GetResponse();
				Stream stream = response.GetResponseStream();
				BinaryReader reader = new BinaryReader(stream);
				byte[] imageBytes = reader.ReadBytes((int)stream.Length);
				reader.Close();

				return imageBytes;*/

				//return response.Data;
				//return data;
			}
			catch(Exception exc)
			{
				UnityEngine.Debug.LogError(exc);

				return null;
			}
		}
		
		public void FetchChartAsync(Country[] countries, WorldBankIndicator indicator, string chartType)
		{
			Thread thread = new Thread(delegate(){
				byte[] data = FetchChart(countries, indicator, YearFrom, YearTo, chartType);

				OnChartFetched(data);
			});
			
			thread.IsBackground = true;
			thread.Start();
		}
		
		public void FetchChartAsync(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo, string chartType)
		{
			Thread thread = new Thread(delegate(){
				byte[] data = FetchChart(countries, indicator, yearFrom, yearTo, chartType);

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

		private string getChartRequestUrl(Country[] countries, WorldBankIndicator indicator, int yearFrom, int yearTo, string chartType)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("http://localhost:8080/GlobetrotterChartWebService/ChartGeneratorServlet");

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
			sb.Append(chartType);

			return sb.ToString();
		}

		private string GetCountriesAsArgument(Country[] countries)
		{
			StringBuilder sb = new StringBuilder();

			for(int i = 0; i < countries.Length; i++)
			{
				sb.Append(countries[i].IsoAlphaThreeCode);

				if(i < (countries.Length - 1))
				{
					sb.Append(";");
				}
			}

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
