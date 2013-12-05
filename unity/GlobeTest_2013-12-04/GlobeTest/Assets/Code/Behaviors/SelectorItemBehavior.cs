using UnityEngine;
using System.Collections;

public class SelectorItemBehavior : MonoBehaviour
{
	private Texture2D m_texture;
	
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
	
	void Start()
	{
		m_texture = null;
	}
	
	void Update()
	{
		gameObject.renderer.material.mainTexture = m_texture;
	}
}
