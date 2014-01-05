using System;
using System.Collections.Generic;

using Globetrotter.ApplicationLayer;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class YearToViewModel : ViewModelBase
	{
		private DataController m_dataController;
		
		private int m_max;

		public int Max
		{
			get
			{
				lock(m_lockObj)
				{
					return m_max;
				}
			}
		}
		
		public int YearTo
		{
			get
			{
				lock(m_lockObj)
				{
					return m_dataController.YearTo;
				}
			}
		}

		public YearToViewModel(DataController dataController, int max)
			: base()
		{
			m_dataController = dataController;

			m_max = max;
		}
		
		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					if(args.InputTypes.And(InputType.ScrollLeft) == InputType.ScrollLeft)
					{
						int year = m_dataController.YearTo - 1;
						
						if(year >= m_dataController.YearFrom)
						{
							m_dataController.YearTo = year;
						}
					}
					
					if(args.InputTypes.And(InputType.ScrollRight) == InputType.ScrollRight)
					{
						int year = m_dataController.YearTo + 1;
						
						if(year <= m_max)
						{
							m_dataController.YearTo = year;
						}
					}
				}
			}
		}
	}
}
