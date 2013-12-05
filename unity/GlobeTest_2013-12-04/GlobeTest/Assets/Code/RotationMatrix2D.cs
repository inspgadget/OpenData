using System;
using UnityEngine;

namespace GlobeTest
{
	public struct RotationMatrix2D
	{
		private double m_angle;
		
		public double Angle
		{
			get
			{
				return m_angle;
			}
			
			set
			{
				m_angle = value;
			}
		}
		
		public RotationMatrix2D(double angle)
		{
			m_angle = angle;
		}
		
		public Vector2D Rotate(Vector2D vector)
		{
			double x = (Math.Cos(m_angle) * vector.X) + ((Math.Sin(m_angle) * vector.Y) * (-1));
			double y = (Math.Sin(m_angle) * vector.X) + (Math.Cos(m_angle) * vector.Y);
			
			return new Vector2D(x, y);
		}
	}
}
