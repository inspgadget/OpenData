using UnityEngine;
using System.Collections;

public class LoadingBehavior : MonoBehaviour
{
	private object m_lockObj = new object();
	
	public float m_rotationSpeed = 2.0f;
	
	public Vector3 m_newPosition;
	
	private bool m_animate;
	
	public bool Animate
	{
		get
		{
			lock(m_lockObj)
			{
				return m_animate;
			}
		}
		
		set
		{
			lock(m_lockObj)
			{
				m_animate = value;
			}
		}
	}
	
	public Vector3 NewPosition
	{
		get
		{
			lock(m_lockObj)
			{
				return m_newPosition;
			}
		}
		
		set
		{
			lock(m_lockObj)
			{
				m_newPosition = value;
			}
		}
	}
	
	void Start()
	{
		m_animate = false;
		m_newPosition = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	void FixedUpdate()
	{
		if(Animate == true)
		{
			transform.Rotate(m_rotationSpeed, 0.0f, 0.0f);
		}
		
		transform.position = NewPosition;
	}
	
	public void SetPosition(float x, float y, float z)
	{
		NewPosition = new Vector3(x, y, z);
	}
}
