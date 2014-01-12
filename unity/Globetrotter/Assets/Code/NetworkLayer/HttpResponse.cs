using System;
using System.Collections.Generic;

namespace Globetrotter.NetworkLayer
{
	public class HttpResponse
	{
		private string m_statusLine;
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
				if(string.IsNullOrEmpty(m_statusLine) == false)
				{
					return int.Parse(m_statusLine.Split(' ')[1]);
				}
				else
				{
					return -1;
				}
			}
		}

		public string StatusLine
		{
			get
			{
				return m_statusLine;
			}

			set
			{
				if(value != null)
				{
					m_statusLine = value;
				}
			}
		}

		public HttpResponse()
		{
			m_statusLine = string.Empty;
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
