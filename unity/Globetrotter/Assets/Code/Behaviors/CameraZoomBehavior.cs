using UnityEngine;
using System.Collections;

using Globetrotter.GuiLayer.ViewModel;

public class CameraZoomBehavior : MonoBehaviour
{
	private GameObject[] m_followers;
	private float[] m_followersOffset;

	private CameraZoomViewModel m_cameraZoomViewModel;

	void Start()
	{
		float z = transform.position.z;

		m_followersOffset = new float[m_followers.Length];

		for(int i = 0; i < m_followers.Length; i++)
		{
			m_followersOffset[i] = z - m_followers[i].transform.position.z;
		}

		m_cameraZoomViewModel.Z = z;
	}

	void Update()
	{
		float z = m_cameraZoomViewModel.Z;

		transform.position = new Vector3(transform.position.x, transform.position.y, z);

		if(m_followersOffset != null)
		{
			for(int i = 0; i < m_followers.Length; i++)
			{
				m_followers[i].transform.position = new Vector3(m_followers[i].transform.position.x,
				                                                	m_followers[i].transform.position.y,
				                                                	z - m_followersOffset[i]);
			}
		}
	}

	public void Init(CameraZoomViewModel cameraZoomViewModel, GameObject[] followers)
	{
		m_cameraZoomViewModel = cameraZoomViewModel;

		if(followers != null)
		{
			m_followers = followers;
		}
	}
}
