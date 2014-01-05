using UnityEngine;
using System.Collections;

using Globetrotter.ApplicationLayer;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class YearFromViewModel : ViewModelBase
	{
		private DataController m_dataController;

		private int m_min;

		public int Min
		{
			get
			{
				lock(m_lockObj)
				{
					return m_min;
				}
			}
		}

		public int YearFrom
		{
			get
			{
				lock(m_lockObj)
				{
					return m_dataController.YearFrom;
				}
			}
		}

		public YearFromViewModel(DataController dataController, int min)
			: base()
		{
			m_dataController = dataController;

			m_min = min;
		}

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					if(args.InputTypes.And(InputType.ScrollLeft) == InputType.ScrollLeft)
					{
						int year = m_dataController.YearFrom - 1;

						if(year >= m_min)
						{
							m_dataController.YearFrom = year;
						}
					}

					if(args.InputTypes.And(InputType.ScrollRight) == InputType.ScrollRight)
					{
						int year = m_dataController.YearFrom + 1;
						
						if(year <= m_dataController.YearTo)
						{
							m_dataController.YearFrom = year;
						}
					}
				}
			}
		}
	}
}
