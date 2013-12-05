using UnityEngine;
using System.Collections;

using GlobeTest.InputLayer;

public class KeyboardInputBehavior : MonoBehaviour, IInputController
{
	public KeyCode upKey = KeyCode.W;
	public KeyCode downKey = KeyCode.S;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	public KeyCode forwardKey = KeyCode.R;
	public KeyCode backwardKey = KeyCode.F;
	public KeyCode confirmKey = KeyCode.K;
	public KeyCode cancelKey = KeyCode.L;
	
	public bool useKeyUp = true;
	
	public event InputReceivedEventHandler InputReceived;
	
	void Start()
	{
	}
	
	void FixedUpdate()
	{
		InputDirection inputDirections = 0;
		bool confirm = false;
		bool cancel = false;
		
		if(((useKeyUp == true) && (Input.GetKeyUp(upKey) == true)) || ((useKeyUp == false) && (Input.GetKey(upKey) == true)))
		{
			inputDirections = inputDirections | InputDirection.Up;
		}
		
		if(((useKeyUp == true) && (Input.GetKeyUp(downKey) == true)) || ((useKeyUp == false) && (Input.GetKey(downKey) == true)))
		{
			inputDirections = inputDirections | InputDirection.Down;
		}
		
		if(((useKeyUp == true) && (Input.GetKeyUp(leftKey) == true)) || ((useKeyUp == false) && (Input.GetKey(leftKey) == true)))
		{
			inputDirections = inputDirections | InputDirection.Left;
		}
		
		if(((useKeyUp == true) && (Input.GetKeyUp(rightKey) == true)) || ((useKeyUp == false) && (Input.GetKey(rightKey) == true)))
		{
			inputDirections = inputDirections | InputDirection.Right;
		}
		
		if(((useKeyUp == true) && (Input.GetKeyUp(forwardKey) == true)) || ((useKeyUp == false) && (Input.GetKey(forwardKey) == true)))
		{
			inputDirections = inputDirections | InputDirection.Forwards;
		}
		
		if(((useKeyUp == true) && (Input.GetKeyUp(backwardKey) == true)) || ((useKeyUp == false) && (Input.GetKey(backwardKey) == true)))
		{
			inputDirections = inputDirections | InputDirection.Backwards;
		}
		
		if(Input.GetKeyUp(confirmKey) == true)
		{
			confirm = true;
		}
		
		if(Input.GetKeyUp(cancelKey) == true)
		{
			cancel = true;
		}
		
		OnInputReceived(inputDirections, confirm, cancel);
	}
	
	public void StartController()
	{
	}
	
	public void StopController()
	{
	}
	
	protected void OnInputReceived(InputDirection inputDirections, bool confirm, bool cancel)
	{
		if((InputReceived != null) && ((inputDirections != 0) || (confirm == true) || (cancel == true)))
		{
			InputReceived(this, new InputReceivedEventArgs(inputDirections, confirm, cancel));
		}
	}
}
