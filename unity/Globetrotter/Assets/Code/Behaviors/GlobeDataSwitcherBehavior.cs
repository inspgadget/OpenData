using UnityEngine;
using System.Collections;

public class GlobeDataSwitcherBehavior : MonoBehaviour
{
	public GameObject lamp;

	public GameObject transparentSphere;
	
	public Vector3 targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
	
	public int steps = 150;

	public string nextScene = string.Empty;
	
	private Vector3 m_positionDeltaVector;
	
	private int m_currStep;
	
	void Start()
	{
		m_positionDeltaVector = (targetPosition - transform.position) / steps;
		
		m_currStep = 0;
	}
	
	void FixedUpdate()
	{
		if(m_currStep <= steps)
		{
			transform.position = transform.position + m_positionDeltaVector;
			lamp.transform.position = lamp.transform.position + m_positionDeltaVector;
			transparentSphere.transform.position = transparentSphere.transform.position + m_positionDeltaVector;
			
			m_currStep++;
		}
		else
		{
			Application.LoadLevel(nextScene);
		}
	}
}
