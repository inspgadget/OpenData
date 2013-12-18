using UnityEngine;
using System.Collections;

using Globetrotter.InputLayer;

public class KeyboardInputController : MonoBehaviour, IInputController
{
	public event InputReceivedEventHandler InputReceived;

	public KeyCode rotateUpKey = KeyCode.W;
	public KeyCode rotateDownKey = KeyCode.S;
	public KeyCode rotateLeftKey = KeyCode.A;
	public KeyCode rotateRightKey = KeyCode.D;

	void Start()
	{
	}

	void FixedUpdate()
	{
		InputType inputTypes = 0;

		if(Input.GetKey(rotateUpKey) == true)
		{
			inputTypes = inputTypes | InputType.RotateUp;
		}

		if(Input.GetKey(rotateDownKey) == true)
		{
			inputTypes = inputTypes | InputType.RotateDown;
		}
		
		if(Input.GetKey(rotateLeftKey) == true)
		{
			inputTypes = inputTypes | InputType.RotateLeft;
		}
		
		if(Input.GetKey(rotateRightKey) == true)
		{
			inputTypes = inputTypes | InputType.RotateRight;
		}

		OnInputReceived(inputTypes);
	}
	
	public void StartController()
	{
		throw new System.NotImplementedException();
	}
	
	public void StopController()
	{
		throw new System.NotImplementedException();
	}
	
	protected void OnInputReceived(InputType inputTypes)
	{
		if(InputReceived != null)
		{
			InputReceived(this, new InputReceivedEventArgs(inputTypes));
		}
	}

	protected void OnInputReceived(InputType[] inputTypes)
	{
		if((InputReceived != null) && (inputTypes != null))
		{
			InputReceived(this, new InputReceivedEventArgs(inputTypes));
		}
	}
}
