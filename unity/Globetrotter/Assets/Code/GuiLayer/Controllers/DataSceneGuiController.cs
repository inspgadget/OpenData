using System;
using System.Collections.Generic;

using Globetrotter.GuiLayer.ViewModel;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.Controllers
{
	public class DataSceneGuiController : GuiController
	{
		private object m_lockObj = new object();

		private IInputController m_inputController;

		private IndicatorSelectorViewModel m_indicatorSelectorViewModel;
		private YearFromViewModel m_yearFromViewModel;
		private YearToViewModel m_yearToViewModel;

		private int m_focusIndex;
		private List<ViewModelBase> m_focusList;

		public DataSceneGuiController(IndicatorSelectorViewModel indicatorSelectorViewModel,
		                              	YearFromViewModel yearFromViewModel,
		                              	YearToViewModel yearToViewModel,
		                              	IInputController inputController)
			: base()
		{
			m_inputController = inputController;
			m_inputController.InputReceived += InputReceivedHandler;

			m_focusList = new List<ViewModelBase>();

			m_indicatorSelectorViewModel = indicatorSelectorViewModel;
			m_indicatorSelectorViewModel.ReactOnInput = true;
			m_focusList.Add(m_indicatorSelectorViewModel);

			m_yearFromViewModel = yearFromViewModel;
			m_yearFromViewModel.ReactOnInput = false;
			m_focusList.Add(m_yearFromViewModel);

			m_yearToViewModel = yearToViewModel;
			m_yearToViewModel.ReactOnInput = false;
			m_focusList.Add(m_yearToViewModel);

			m_focusIndex = 0;
		}

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(args.HasInputType(InputType.ClickLong) == true)
				{
					//disconnect from event
					m_inputController.InputReceived -= InputReceivedHandler;
					
					m_inputController.InputReceived -= m_yearToViewModel.InputReceivedHandler;
					m_inputController.InputReceived -= m_yearToViewModel.InputReceivedHandler;
					
					//stop reacting on input
					m_indicatorSelectorViewModel.ReactOnInput = false;
					
					//change scene
					OnChangeScene("GlobeScene");
				}
				else
				{
					if(args.HasInputType(InputType.FocusPrevious) == true)
					{
						m_focusList[m_focusIndex].ReactOnInput = false;

						m_focusIndex--;

						if(m_focusIndex < 0)
						{
							m_focusIndex = m_focusList.Count - 1;
						}

						m_focusList[m_focusIndex].ReactOnInput = true;
					}

					if(args.HasInputType(InputType.FocusNext) == true)
					{
						m_focusList[m_focusIndex].ReactOnInput = false;

						m_focusIndex++;

						if(m_focusIndex >= m_focusList.Count)
						{
							m_focusIndex = 0;
						}

						m_focusList[m_focusIndex].ReactOnInput = true;
					}
				}
			}
		}
	}
}
