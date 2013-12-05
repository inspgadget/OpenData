using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GlobeTest.DataLayer
{
	public class WorldBankIndicator
	{
		public delegate void WorldBankDataFetchedEventHandler(object sender, WorldBankDataFetchedEventArgs args);
		public event WorldBankDataFetchedEventHandler WorldBankDataFetched;
		
		private string m_name;
		private string m_code;
		private string m_topic;
		
		public string Code
		{
			get
			{
				return m_code;
			}
			
			set
			{
				m_code = value;
			}
		}
		
		public string Name
		{
			get
			{
				return m_name;
			}
			
			set
			{
				m_name = value;
			}
		}
		
		public string Topic
		{
			get
			{
				return m_topic;
			}
			
			set
			{
				m_topic = value;
			}
		}
		
		public WorldBankIndicator()
		{
			m_name = string.Empty;
			m_code = string.Empty;
			m_topic = string.Empty;
		}
		
		public WorldBankIndicator(string name, string code, string topic)
		{
			m_name = name;
			m_code = code;
			m_topic = topic;
		}
		
		public void FetchData(string[] isoAlphaThreeCode)
		{
			Thread t = new Thread(delegate()
			{
				string url = GetRequestUrlForCountries(isoAlphaThreeCode);
				string host = GetHost(url);
				string path = GetPath(url);
				int port = 80;
				
				string request = "GET " + path + " HTTP/1.1\nHost: " + host + "\r\n\r\n";
				
				using(TcpClient socket = new TcpClient(host, port))
				{
					using(BufferedStream bs = new BufferedStream(socket.GetStream()))
					{
						//request
						//
						UnityEngine.Debug.Log("send request to: " + url);
						//
						using(StreamWriter sw = new StreamWriter(new BufferedStream(socket.GetStream())))
						{
							sw.Write(request);
							sw.Flush();
							
							//response
							using(StreamReader sr = new StreamReader(new BufferedStream(socket.GetStream())))
							{
								if(sr.EndOfStream == false)
								{
									string line = sr.ReadLine();
									
									if(line.Contains("200"))
									{
										line = null;
										
										//read over header
										while((sr.EndOfStream == false) && ((line = sr.ReadLine()) != ""))
										{
										}
										
										//read content
										StringBuilder sb = new StringBuilder();
										line = null;
										
										while((sr.EndOfStream == false) && ((line = sr.ReadLine()) != null))
										{
											sb.Append(line);
										}
										
										//
										UnityEngine.Debug.Log(sb.ToString());
										//
										OnWorldBankDataFetched(ParseXml(sb.ToString()));
									}
									else
									{
										UnityEngine.Debug.Log("error: " + line);
									}
								}
							}
						}
					}
				}
			});
			
			t.IsBackground = true;
			t.Start();
		}
		
		private WorldBankData ParseXml(string xml)
		{
			try
			{
				IWorldBankDataParser parser = new WorldBankDataParser();
				IList<WorldBankDataItem> items = parser.parseXml(xml);
				
				return new WorldBankData(m_name, string.Empty, false, items);
			}
			catch(Exception exc)
			{
				UnityEngine.Debug.LogException(exc);
				
				return null;
			}
		}
		
		private string GetHost(string url)
		{
			string withoutProtocol = url.Substring(7);
			int hostEnd = withoutProtocol.IndexOf("/");
			
			return withoutProtocol.Substring(0, hostEnd);
		}
		
		private string GetPath(string url)
		{
			string withoutProtocol = url.Substring(7);
			int hostEnd = withoutProtocol.IndexOf("/");
			
			return withoutProtocol.Substring(hostEnd);
		}
		
		public string GetRequestUrlForCountries(string[] isoAlphaThreeCodes)
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append("http://api.worldbank.org/en/countries/");
			
			for(int i = 0; i < (isoAlphaThreeCodes.Length - 1); i++)
			{
				sb.Append(isoAlphaThreeCodes[i]);
				sb.Append(';');
			}
			
			sb.Append(isoAlphaThreeCodes[(isoAlphaThreeCodes.Length - 1)]);
			
			sb.Append("/indicators/");
			sb.Append(m_code);
			sb.Append("?per_page=1024");
			sb.Append("&date=2010:");
			sb.Append(DateTime.Now.Year);
			
			return sb.ToString();
		}
		
		public string GetRequestUrlForCountry(string isoAlphaThreeCode)
		{
			return GetRequestUrlForCountries(new string[] { isoAlphaThreeCode });
		}
		
		protected void OnWorldBankDataFetched(WorldBankData data)
		{
			if((WorldBankDataFetched != null) && (data != null))
			{
				WorldBankDataFetched(this, new WorldBankDataFetchedEventArgs(data));
			}
		}
		
		public class WorldBankIndicatorComparer : IComparer<WorldBankIndicator>
		{
			public int Compare(WorldBankIndicator a, WorldBankIndicator b)
			{
				return a.Name.CompareTo(b.Name);
			}
		}
	}
}
