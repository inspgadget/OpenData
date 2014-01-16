using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Globetrotter.ApplicationLayer
{
	public class ChartFetchedEventArgs : EventArgs
	{
		private byte[] m_data;

		public byte[] Data
		{
			get
			{
				return m_data;
			}
		}

		public ChartFetchedEventArgs(byte[] data)
			: base()
		{
			m_data = data;
		}
	}
}
