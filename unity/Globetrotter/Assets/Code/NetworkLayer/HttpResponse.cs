using System;
using System.Collections.Generic;

namespace Globetrotter.NetworkLayer
{
	public class HttpResponse
	{
		private string m_stateLine;
		private Dictionary<string, string> m_header;
		private byte[] m_data;

		public byte[] Data
		{
			get
			{
				return m_data;
			}

			set
			{
				if(value != null)
				{
					m_data = value;
				}
			}
		}

		public Dictionary<string, string> Header
		{
			get
			{
				return m_header;
			}

			set
			{
				if(value != null)
				{
					m_header = value;
				}
			}
		}

		public int StatusCode
		{
			get
			{
				if(string.IsNullOrEmpty(m_stateLine) == false)
				{
					return int.Parse(m_stateLine.Split(' ')[1]);
				}
				else
				{
					return -1;
				}
			}
		}

		public string StateLine
		{
			get
			{
				return m_stateLine;
			}

			set
			{
				if(value != null)
				{
					m_stateLine = value;
				}
			}
		}

		public HttpResponse()
		{
			m_stateLine = string.Empty;
			m_header = new Dictionary<string, string>();
			m_data = new byte[0];
		}

		public string GetHeaderValue(string key)
		{
			return m_header[key];
		}

		public void SetHeaderValue(string key, string value)
		{
			if((string.IsNullOrEmpty(key) == false) && (string.IsNullOrEmpty(value) == false))
			{
				m_header[key] = value;
			}
		}
	}
}
