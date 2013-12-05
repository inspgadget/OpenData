using UnityEngine;
using System.Collections;

public class ThirdPersonCameraBehavior : MonoBehaviour
{
	public KeyCode forwards = KeyCode.W;
	public KeyCode backwards = KeyCode.S;
	public KeyCode left = KeyCode.A;
	public KeyCode right = KeyCode.D;
	
	public float moveFactor = 0.05f;
	
	void FixedUpdate()
	{
		float x = 0.0f;
		float y = 0.0f;
		float z = 0.0f;
		
		if(Input.GetKey(forwards) == true)
		{
			z = z + moveFactor;
		}
		
		if(Input.GetKey(backwards) == true)
		{
			z = z - moveFactor;
		}
		
		if(Input.GetKey(left) == true)
		{
			x = x - moveFactor;
		}
		
		if(Input.GetKey(right) == true)
		{
			x = x + moveFactor;
		}
		
		transform.Translate(x, y, z, Space.Self);
	}
}
