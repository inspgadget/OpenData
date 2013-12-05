using UnityEngine;
using System.Collections;

using GlobeTest.InputLayer;

public class GlobeTestMainBehavior : MonoBehaviour
{
	private IInputController m_keyboardInputController;
	
	void Start()
	{
		m_keyboardInputController = gameObject.AddComponent<KeyboardInputBehavior>();
		m_keyboardInputController.InputReceived += InputReceivedHandler;
	}
	
	void Update()
	{
	}
	
	public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		if(args.Confirm == true)
		{
			Application.LoadLevel("WorldLevel");
		}
	}
}
