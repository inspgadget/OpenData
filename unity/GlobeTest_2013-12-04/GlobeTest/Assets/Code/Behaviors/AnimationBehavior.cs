using UnityEngine;
using System.Collections;

public class AnimationBehavior : MonoBehaviour
{
	private bool m_animationRunning;
	
	private float m_scaleX;
	private float m_scaleY;
	private float m_scaleZ;
	
	void Start()
	{
		m_animationRunning = true;
		
		m_scaleX = transform.localScale.x;
		m_scaleY = transform.localScale.y;
		m_scaleZ = transform.localScale.z;
	}
	
	void Update()
	{
		Debug.LogWarning(transform.localScale.ToString());
		/*if((m_animationRunning == false) && (animation != null))
		{
			animation.Play();
			m_animationRunning = true;
			//this.animation.animation.
			
			Debug.LogWarning("animation running");
		}*/
		
		if(m_animationRunning == true)
		{
			transform.localScale.Set(m_scaleX,m_scaleY,m_scaleZ);
		}
	}
}
