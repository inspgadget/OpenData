using System;
using System.Collections.Generic;

using Globetrotter.DataLayer;

namespace Globetrotter.GuiLayer
{
	public class DataPoint
	{
		private int m_year;
		private double[] m_data;

		public double[] Data
		{
			get
			{
				return m_data;
			}

			set
			{
				m_data = value;
			}
		}
		
		public DataPoint()
		{
			m_year = 0;
			m_data = new double[0];
		}

		public int Year
		{
			get
			{
				return m_year;
			}

			set
			{
				m_year = value;
			}
		}

		public DataPoint(int year, double[] data)
		{
			m_year = year;
			m_data = data;
		}

		public double this[int index]
		{
			get
			{
				try
				{
					return m_data[index];
				}
				catch(IndexOutOfRangeException)
				{
					return 0.0;
				}
			}
		}
	}
}
