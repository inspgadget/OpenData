using System;

namespace GlobeTest.InputLayer
{
	public delegate void InputReceivedEventHandler(object sender, InputReceivedEventArgs args);
	
	public interface IInputController
	{
		event InputReceivedEventHandler InputReceived;
		
		void StartController();
		
		void StopController();
	}
}
