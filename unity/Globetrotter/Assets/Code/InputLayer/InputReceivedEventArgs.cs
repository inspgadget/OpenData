using System;

namespace Globetrotter.InputLayer
{
	public class InputReceivedEventArgs : EventArgs
	{
		private InputType m_inputTypes;
		
		public InputType InputTypes
		{
			get
			{
				return m_inputTypes;
			}
		}
		
		public InputReceivedEventArgs(InputType inputTypes)
		{
			m_inputTypes = inputTypes;
		}
		
		public InputReceivedEventArgs(InputType[] inputTypes)
		{
			m_inputTypes = 0;
			
			if(inputTypes != null)
			{
				for(int i = 0; i < inputTypes.Length; i++)
				{
					m_inputTypes = m_inputTypes.Or(inputTypes[i]);
				}
			}
		}
		
		public bool HasInputType(InputType inputType)
		{
			return (m_inputTypes.And(inputType) == inputType);
		}
	}
}
