using System;
using System.Collections.Generic;

using Globetrotter.GuiLayer.ViewModel;
using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.Controllers
{
	public class GlobeSceneGuiController : GuiController
	{
		private object m_lockObj = new object();

		private IInputController m_inputController;

		private CameraZoomViewModel m_cameraZoomViewModel;
		private GlobeViewModel m_globeViewModel;
		private CountrySelectorViewModel m_countrySelectorViewModel;
		private SelectedCountriesViewModel m_selectedCountriesViewModel;

		public GlobeSceneGuiController(CameraZoomViewModel cameraZoomViewModel,
		                               	GlobeViewModel globeViewModel,
		                               	CountrySelectorViewModel countrySelectorViewModel,
		                               	SelectedCountriesViewModel selectedCountriesViewModel,
		                               	IInputController inputController)
			: base()
		{
			m_inputController = inputController;
			m_inputController.InputReceived += InputReceivedHandler;

			m_cameraZoomViewModel = cameraZoomViewModel;
			m_cameraZoomViewModel.ReactOnInput = true;

			m_globeViewModel = globeViewModel;
			m_globeViewModel.ReactOnInput = true;

			m_countrySelectorViewModel = countrySelectorViewModel;
			m_countrySelectorViewModel.ReactOnInput = true;

			m_selectedCountriesViewModel = selectedCountriesViewModel;
			m_selectedCountriesViewModel.ReactOnInput = false;
		}

		private void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(args.InputTypes.And(InputType.ClickLong) == InputType.ClickLong)
				{
					//disconnect from event handler
					m_inputController.InputReceived -= InputReceivedHandler;

					m_inputController.InputReceived -= m_cameraZoomViewModel.InputReceivedHandler;
					m_inputController.InputReceived -= m_globeViewModel.InputReceivedHandler;

					//stop reacting on input
					m_countrySelectorViewModel.ReactOnInput = false;
					m_selectedCountriesViewModel.ReactOnInput = false;

					//change scene
					OnChangeScene("DataScene");
				}
				else
				{
					if((args.InputTypes.And(InputType.FocusPrevious) == InputType.FocusPrevious) ||
					   	(args.InputTypes.And(InputType.FocusNext) == InputType.FocusNext))
					{
						m_countrySelectorViewModel.ReactOnInput = !m_countrySelectorViewModel.ReactOnInput;
						m_selectedCountriesViewModel.ReactOnInput = !m_selectedCountriesViewModel.ReactOnInput;
					}
				}
			}
		}
	}
}
