using UnityEngine;
using System.Collections.Generic;

namespace GlobeTest.DomainLayer
{
	public class Country
	{
		private string m_name;
		private string m_isoAlphaThreeCode;
		
		private float m_rotationX;
		private float m_rotationY;
		private float m_rotationZ;
		
		private Texture2D m_texture;
		
		private float m_x;
		private float m_z;
		
		public string IsoAlphaThreeCode
		{
			get
			{
				return m_isoAlphaThreeCode;
			}
			
			set
			{
				if(value != null)
				{
					m_isoAlphaThreeCode = value;
				}
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
				if(value != null)
				{
					m_name = value;
				}
			}
		}
		
		public Texture2D PlaneTexture
		{
			get
			{
				return m_texture;
			}
			
			set
			{
				m_texture = value;
			}
		}
		
		public float RotationX
		{
			get
			{
				return m_rotationX;
			}
			
			set
			{
				m_rotationX = value;
			}
		}
		
		public float RotationY
		{
			get
			{
				return m_rotationY;
			}
			
			set
			{
				m_rotationY = value;
			}
		}
		
		public float RotationZ
		{
			get
			{
				return m_rotationZ;
			}
			
			set
			{
				m_rotationZ = value;
			}
		}
		
		public float X
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
		
		public float Z
		{
			get
			{
				return m_z;
			}
			
			set
			{
				m_z = value;
			}
		}
		
		public Country()
		{
			m_name = string.Empty;
			m_isoAlphaThreeCode = string.Empty;
			
			m_rotationX = 0.0f;
			m_rotationY = 0.0f;
			m_rotationZ = 0.0f;
			
			m_texture = null;
			
			m_x = 0.0f;
			m_z = 0.0f;
		}
		
		public Country(string name, string isoAlphaThreeCode, string textureAsset)
			: base()
		{
			m_name = name;
			m_isoAlphaThreeCode = isoAlphaThreeCode;
			
			if(string.IsNullOrEmpty(textureAsset) == false)
			{
				m_texture = (Texture2D)Resources.Load(textureAsset);
			}
			else
			{
				m_texture = null;
			}
		}
		
		public Country(string name, string isoAlphaThreeCode, string textureAsset, float x, float z)
			: base()
		{
			m_name = name;
			m_isoAlphaThreeCode = isoAlphaThreeCode;
			
			m_x = x;
			m_z = z;
			
			if(string.IsNullOrEmpty(textureAsset) == false)
			{
				m_texture = (Texture2D)Resources.Load(textureAsset);
			}
			else
			{
				m_texture = null;
			}
		}
		
		public class CountryComparer : IComparer<Country>
		{
			public CountryComparer()
			{
			}
			
			public int Compare (Country a, Country b)
			{
				return a.Name.CompareTo(b.Name);
			}
		}
	}
}
