using UnityEngine;
using System.Collections.Generic;

namespace GlobeTest.DomainLayer
{
	public class Continent
	{
		private string m_name;
		private List<Country> m_countries;
		
		private float m_rotationX;
		private float m_rotationY;
		private float m_rotationZ;
		
		private Texture2D m_texture;
		
		public List<Country> Countries
		{
			get
			{
				return m_countries;
			}
			
			set
			{
				if(value != null)
				{
					m_countries = value;
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
		
		public Continent()
		{
			m_name = string.Empty;
			m_countries = new List<Country>();
			
			m_rotationX = 0.0f;
			m_rotationY = 0.0f;
			m_rotationZ = 0.0f;
			
			m_texture = null;
		}
		
		public Continent(string name, List<Country> countries, string textureAsset)
		{
			m_name = name;
			m_countries = countries;
			
			if(string.IsNullOrEmpty(textureAsset) == false)
			{
				m_texture = (Texture2D)Resources.Load(textureAsset);
			}
			else
			{
				m_texture = null;
			}
		}
		
		public class ContinentComparer : IComparer<Continent>
		{
			public ContinentComparer()
			{
			}
			
			public int Compare (Continent a, Continent b)
			{
				return a.Name.CompareTo(b.Name);
			}
		}
	}
}
