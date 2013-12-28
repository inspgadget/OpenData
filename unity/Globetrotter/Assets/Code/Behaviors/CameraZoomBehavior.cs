using UnityEngine;
using System.Collections;

using Globetrotter.InputLayer;

public class CameraZoomBehavior : MonoBehaviour
{
	private object m_lockObject = new object();

	private IInputController m_inputController;

	private float m_z;

	private float m_speed;

	void Start()
	{
		lock(m_lockObject)
		{
			m_z = transform.position.z;
		}

		m_speed = 0.05f;
	}

	void Update()
	{
		lock(m_lockObject)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, m_z);
		}
	}

	public void Init(IInputController inputController)
	{
		m_inputController = inputController;

		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;
	}

	protected void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		lock(m_lockObject)
		{
			if(args.HasInputType(InputType.ZoomIn) == true)
			{
				m_z = m_z + m_speed;
			}

			if(args.HasInputType(InputType.ZoomOut) == true)
			{
				m_z = m_z - m_speed;
			}
		}
	}
}
