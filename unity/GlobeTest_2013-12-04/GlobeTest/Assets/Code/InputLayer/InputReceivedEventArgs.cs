using System;

namespace GlobeTest.InputLayer
{
	public class InputReceivedEventArgs : EventArgs
	{
		private InputDirection m_inputDirections;
		private bool m_confirm;
		private bool m_cancel;
		
		public bool Cancel
		{
			get
			{
				return m_cancel;
			}
		}
		
		public bool Confirm
		{
			get
			{
				return m_confirm;
			}
		}
		
		public InputDirection InputDirections
		{
			get
			{
				return m_inputDirections;
			}
		}
		
		public InputReceivedEventArgs(InputDirection inputDirections, bool confirm, bool cancel)
		{
			m_inputDirections = inputDirections;
			m_confirm = confirm;
			m_cancel = cancel;
		}
		
		public InputReceivedEventArgs(InputDirection[] inputDirections, bool confirm, bool cancel)
		{
			m_confirm = confirm;
			m_cancel = cancel;
			
			m_inputDirections = 0;
			
			if(inputDirections != null)
			{
				for(int i = 0; i < inputDirections.Length; i++)
				{
					m_inputDirections = m_inputDirections | inputDirections[i];
				}
			}
		}
		
		public bool HasDirection(InputDirection inputDirection)
		{
			return ((m_inputDirections & inputDirection) == inputDirection);
		}
	}
}
