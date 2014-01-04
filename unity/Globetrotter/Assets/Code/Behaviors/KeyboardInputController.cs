using UnityEngine;
using System.Collections;

using Globetrotter.InputLayer;

public class KeyboardInputController : MonoBehaviour, IInputController
{
	public event InputReceivedEventHandler InputReceived;

	public KeyCode clickDoubleKey = KeyCode.K;
	public KeyCode clickLongKey = KeyCode.L;

	public KeyCode focusPreviousKey = KeyCode.O;
	public KeyCode focusNextKey = KeyCode.P;

	public KeyCode rotateUpKey = KeyCode.W;
	public KeyCode rotateDownKey = KeyCode.S;
	public KeyCode rotateLeftKey = KeyCode.A;
	public KeyCode rotateRightKey = KeyCode.D;
	
	public KeyCode zoomInKey = KeyCode.R;
	public KeyCode zoomOutKey = KeyCode.F;

	void Start()
	{
	}

	void FixedUpdate()
	{
		InputType inputTypes = 0;

		if(Input.GetKeyUp(clickDoubleKey) == true)
		{
			inputTypes = inputTypes | InputType.ClickDouble;
		}
		
		if(Input.GetKeyUp(clickLongKey) == true)
		{
			inputTypes = inputTypes | InputType.ClickLong;
		}
		
		if(Input.GetKeyUp(focusNextKey) == true)
		{
			inputTypes = inputTypes | InputType.FocusNext;
		}
		
		if(Input.GetKeyUp(focusPreviousKey) == true)
		{
			inputTypes = inputTypes | InputType.FocusPrevious;
		}

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
		
		if(Input.GetKey(zoomInKey) == true)
		{
			inputTypes = inputTypes | InputType.ZoomIn;
		}
		
		if(Input.GetKey(zoomOutKey) == true)
		{
			inputTypes = inputTypes | InputType.ZoomOut;
		}

		OnInputReceived(inputTypes);
	}
	
	public void StartController()
	{
		//throw new System.NotImplementedException();
	}
	
	public void StopController()
	{
		//throw new System.NotImplementedException();
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
