using UnityEngine;
using System.Collections;

using Globetrotter.InputLayer;

public class GlobeBehavior : MonoBehaviour
{
	private object m_lockObj = new object();

	private IInputController m_inputController;

	private float m_horizontalAngle;
	private float m_verticalAngle;

	private float m_speed;

	void Start()
	{
		m_horizontalAngle = 0.0f;
		m_verticalAngle = 0.0f;

		m_speed = 1.0f;
	}

	void Update()
	{
		lock(m_lockObj)
		{
			transform.Rotate(m_verticalAngle, m_horizontalAngle, 0.0f, Space.World);

			m_horizontalAngle = 0.0f;
			m_verticalAngle = 0.0f;
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
		lock(m_lockObj)
		{
			if(args.HasInputType(InputType.RotateUp) == true)
			{
				m_verticalAngle = m_verticalAngle - m_speed;
			}

			if(args.HasInputType(InputType.RotateDown) == true)
			{
				m_verticalAngle = m_verticalAngle + m_speed;
			}
			
			if(args.HasInputType(InputType.RotateLeft) == true)
			{
				m_horizontalAngle = m_horizontalAngle + m_speed;
			}

			if(args.HasInputType(InputType.RotateRight) == true)
			{
				m_horizontalAngle = m_horizontalAngle - m_speed;
			}
		}
	}
}
