using UnityEngine;
using System.Collections;
using Globetrotter;
using Globetrotter.GuiLayer.ViewModel;

public class RotatingGlobeBehavior : MonoBehaviour
{
	public float speed = 0.25f;

	void FixedUpdate()
	{
		transform.Rotate(0.0f, speed, 0.0f, Space.World);
	}
}
