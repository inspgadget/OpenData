using UnityEngine;
using System.Collections;

public class ZoomBehavior : MonoBehaviour
{
	public KeyCode zoomIn = KeyCode.K;
	public KeyCode zoomOut = KeyCode.M;
	
	public float zoomFactor = 0.05f;
	
	public float zoomInLimit = -1.5f;
	public float zoomOutLimit = -3.0f;
	
	void FixedUpdate()
	{
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;
		
		if(Input.GetKey(zoomIn) == true)
		{
			if((z + zoomFactor) < zoomInLimit)
			{
				z = z + zoomFactor;
			}
			else
			{
				z = zoomInLimit;
			}
		}
		
		if(Input.GetKey(zoomOut) == true)
		{
			if((z - zoomFactor) > zoomOutLimit)
			{
				z = z - zoomFactor;
			}
			else
			{
				z = zoomOutLimit;
			}
		}
		
		transform.position = new Vector3(x, y, z);
	}
}
