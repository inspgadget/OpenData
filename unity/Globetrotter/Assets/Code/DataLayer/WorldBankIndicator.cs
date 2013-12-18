using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using Globetrotter.NetworkLayer;

namespace Globetrotter.DataLayer
{
	public class WorldBankIndicator
	{
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
		
		public WorldBankData FetchData(string[] isoAlphaThreeCode, int yearFrom, int yearTo)
		{
			string url = GetRequestUrlForCountries(isoAlphaThreeCode, yearFrom, yearTo);
			int port = 80;

			return ParseXml(new TcpRequestController().GetRepsonseData(url, port));
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
		
		public string GetRequestUrlForCountries(string[] isoAlphaThreeCodes, int yearFrom, int yearTo)
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
			sb.Append("&date=");
			sb.Append(yearFrom);
			sb.Append(":");
			sb.Append(yearTo);
			
			return sb.ToString();
		}
		
		public string GetRequestUrlForCountry(string isoAlphaThreeCode, int yearFrom, int yearTo)
		{
			return GetRequestUrlForCountries(new string[] { isoAlphaThreeCode }, yearFrom, yearTo);
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
