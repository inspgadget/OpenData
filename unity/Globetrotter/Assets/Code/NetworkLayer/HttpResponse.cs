using System;
using System.Collections.Generic;

namespace Globetrotter.NetworkLayer
{
	public class HttpResponse
	{
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

		public HttpResponse()
		{
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
