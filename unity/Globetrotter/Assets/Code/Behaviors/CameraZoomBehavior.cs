using UnityEngine;
using System.Collections;

using Globetrotter.InputLayer;

public class CameraZoomBehavior : MonoBehaviour
{
	private object m_lockObject = new object();

	private IInputController m_inputController;

	private float m_z;
	private float[] m_limits;

	private GameObject[] m_followers;
	private float[] m_followersOffset;

	private float m_speed;

	void Start()
	{
		lock(m_lockObject)
		{
			m_z = transform.position.z;

			m_followersOffset = new float[m_followers.Length];

			for(int i = 0; i < m_followers.Length; i++)
			{
				m_followersOffset[i] = m_z - m_followers[i].transform.position.z;
			}
		}

		m_speed = 0.05f;
	}

	void Update()
	{
		lock(m_lockObject)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, m_z);

			if(m_followersOffset != null)
			{
				for(int i = 0; i < m_followers.Length; i++)
				{
					m_followers[i].transform.position = new Vector3(m_followers[i].transform.position.x,
					                                                	m_followers[i].transform.position.y,
					                                                	m_z - m_followersOffset[i]);
				}
			}
		}
	}

	public void Init(IInputController inputController, float[] limits, GameObject[] followers)
	{
		m_inputController = inputController;

		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;

		if(followers != null)
		{
			m_followers = followers;
		}

		if((limits != null) && (limits.Length == 2))
		{
			m_limits = limits;
		}
	}

	protected void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		lock(m_lockObject)
		{
			if(args.HasInputType(InputType.ZoomIn) == true)
			{
				float z = m_z + m_speed;

				if(z <= m_limits[0])
				{
					m_z = z;
				}
			}

			if(args.HasInputType(InputType.ZoomOut) == true)
			{
				float z = m_z - m_speed;

				if(z >= m_limits[1])
				{
					m_z = z;
				}
			}
		}
	}
}
