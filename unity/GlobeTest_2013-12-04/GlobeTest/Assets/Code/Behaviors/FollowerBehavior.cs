using UnityEngine;
using System.Collections;

public class FollowerBehavior : MonoBehaviour
{
	public GameObject target;
	
	public float xOffset = 0.0f;
	public float yOffset = 0.0f;
	public float zOffset = 0.4f;
	
	void FixedUpdate()
	{
		if(target != null)
		{
			transform.position = new Vector3(target.transform.position.x + xOffset,
												target.transform.position.y + yOffset,
												target.transform.position.z + zOffset);
		}
	}
}
