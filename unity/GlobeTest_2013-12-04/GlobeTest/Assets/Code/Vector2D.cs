using System;

namespace GlobeTest
{
	public struct Vector2D
	{
		private double m_x;
		private double m_y;
		
		public double AbsoluteValue
		{
			get
			{
				return Math.Sqrt((m_x * m_x) + (m_y * m_y));
			}
		}
		
		public double X
		{
			get
			{
				return m_x;
			}
			
			set
			{
				m_x = value;
			}
		}
		
		public double Y
		{
			get
			{
				return m_y;
			}
			
			set
			{
				m_y = value;
			}
		}
		
		public Vector2D(double x, double y)
		{
			m_x = x;
			m_y = y;
		}
		
		public double AngleBetween(Vector2D vector)
		{
			double top = this * vector;
			double bottom = AbsoluteValue * vector.AbsoluteValue;
			
			return Math.Acos(top / bottom);
		}
		
		public static double operator *(Vector2D a, Vector2D b)
		{
			return ((a.X * b.X) + (a.Y * b.Y));
		}
	}
}
