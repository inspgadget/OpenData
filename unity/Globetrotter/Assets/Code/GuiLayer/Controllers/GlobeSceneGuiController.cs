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

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if((args.HasInputType(InputType.ClickLong) == true) && (m_selectedCountriesViewModel.SelectedCountries.Length > 0))
				{
					//disconnect from event
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
					if((args.HasInputType(InputType.FocusPrevious) == true) ||
					   	(args.HasInputType(InputType.FocusNext) == true))
					{
						m_countrySelectorViewModel.ReactOnInput = !m_countrySelectorViewModel.ReactOnInput;
						m_selectedCountriesViewModel.ReactOnInput = !m_selectedCountriesViewModel.ReactOnInput;
					}
				}
			}
		}
	}
}
