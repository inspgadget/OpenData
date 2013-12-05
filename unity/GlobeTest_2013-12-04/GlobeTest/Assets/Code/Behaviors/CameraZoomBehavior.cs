using UnityEngine;
using System.Collections;

using GlobeTest;
using GlobeTest.InputLayer;

public class CameraZoomBehavior : MonoBehaviour
{
	private object m_lockObj = new object();
	
	public float zoomFactor = 0.05f;
	
	public float maxZoom = -1.5f;
	public float minZoom = -2.5f;
	
	private IInputController m_inputController;
	
	private float m_z;
	
	void Start()
	{
		lock(m_lockObj)
		{
			m_z = transform.position.z;
		}
	}
	
	void FixedUpdate()
	{
		lock(m_lockObj)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, m_z);
			m_z = transform.position.z;
		}
	}
	
	public void Init(IInputController inputController)
	{
		m_inputController = inputController;
		
		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;
	}
	
	public void ZoomIn()
	{
		lock(m_lockObj)
		{
			float z = m_z + zoomFactor;
			
			if(z <= maxZoom)
			{
				m_z = z;
			}
		}
	}
	
	public void ZoomOut()
	{
		lock(m_lockObj)
		{
			float z = m_z - zoomFactor;
			
			if(z >= minZoom)
			{
				m_z = z;
			}
		}
	}
	
	public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		if(args.HasDirection(InputDirection.Forwards) == true)
		{
			ZoomIn();
		}
		
		if(args.HasDirection(InputDirection.Backwards) == true)
		{
			ZoomOut();
		}
	}
}
