using UnityEngine;
using System.Collections.Generic;

using Globetrotter;
using Globetrotter.ApplicationLayer;
using Globetrotter.GuiLayer.Controllers;
using Globetrotter.GuiLayer.ViewModel;
using Globetrotter.InputLayer;

public class GlobeSceneSetupBehavior : MonoBehaviour
{
	private object m_lockObj = new object();

	public Camera mainCamera;
	public GameObject magnifyingGlass;
	public GameObject globeLight;

	private GlobeSceneGuiController m_globeSceneGuiController;

	private string m_sceneName;

	void Start()
	{
		m_sceneName = null;

		//input controller
		IInputController inputController = ObjectDepot.Instance.Retrive<IInputController>();

		if(inputController == null)
		{
			inputController = mainCamera.gameObject.AddComponent<KeyboardInputController>();
		}

		//data controller
		DataController dataController = ObjectDepot.Instance.Retrive<DataController>();

		if(dataController == null)
		{
			dataController = new DataController();
			ObjectDepot.Instance.Store<DataController>(dataController);
		}

		//countries controller
		CountriesController countriesController = ObjectDepot.Instance.Retrive<CountriesController>();

		if(countriesController == null)
		{
			countriesController = new CountriesController(dataController);
			ObjectDepot.Instance.Store<CountriesController>(countriesController);
		}

		//globe view model
		GlobeViewModel globeViewModel = new GlobeViewModel(1.0f);
		inputController.InputReceived += globeViewModel.InputReceivedHandler;

		//country selector view model
		CountrySelectorViewModel countrySelectorViewModel = ObjectDepot.Instance.Retrive<CountrySelectorViewModel>();

		if(countrySelectorViewModel == null)
		{
			countrySelectorViewModel = new CountrySelectorViewModel(countriesController);

			ObjectDepot.Instance.Store<CountrySelectorViewModel>(countrySelectorViewModel);
		}
		
		//set event handler outside to catch the special case for keyboard input
		inputController.InputReceived -= countrySelectorViewModel.InputReceivedHandler;
		inputController.InputReceived += countrySelectorViewModel.InputReceivedHandler;

		//selected countries view model
		SelectedCountriesViewModel selectedCountriesViewModel = ObjectDepot.Instance.Retrive<SelectedCountriesViewModel>();

		if(selectedCountriesViewModel == null)
		{
			selectedCountriesViewModel = new SelectedCountriesViewModel(countriesController);

			ObjectDepot.Instance.Store<SelectedCountriesViewModel>(selectedCountriesViewModel);
		}
		
		//set event handler outside to catch the special case for keyboard input
		inputController.InputReceived -= selectedCountriesViewModel.InputReceivedHandler;
		inputController.InputReceived += selectedCountriesViewModel.InputReceivedHandler;

		//camera zoom view model
		CameraZoomViewModel cameraZoomViewModel = new CameraZoomViewModel(new float[] { -1.5f, -3.0f}, 0.05f);
		inputController.InputReceived += cameraZoomViewModel.InputReceivedHandler;

		ObjectDepot.Instance.Store<CameraZoomViewModel>(cameraZoomViewModel);

		//globe scene gui controller
		m_globeSceneGuiController = new GlobeSceneGuiController(cameraZoomViewModel,
		                                                        	globeViewModel,
		                                                        	countrySelectorViewModel,
		                                                        	selectedCountriesViewModel,
		                                                        	inputController);
		m_globeSceneGuiController.ChangeScene += ChangeSceneHandler;

		//globe behavior
		GlobeBehavior globeBehavior = gameObject.AddComponent<GlobeBehavior>();
		globeBehavior.Init(globeViewModel, globeLight);

		//country selector behavior
		CountrySelectorBehavior countrySelectorBehavior = mainCamera.gameObject.AddComponent<CountrySelectorBehavior>();
		countrySelectorBehavior.Init(countrySelectorViewModel);

		//selected countries behavior
		SelectedCountriesBehavior selectedCountriesBehavior = mainCamera.gameObject.AddComponent<SelectedCountriesBehavior>();
		selectedCountriesBehavior.Init(selectedCountriesViewModel);

		//camera zoom behavior
		CameraZoomBehavior cameraZoomBehavior = mainCamera.gameObject.AddComponent<CameraZoomBehavior>();
		cameraZoomBehavior.Init(cameraZoomViewModel, new GameObject[]{ magnifyingGlass });

		//country selection behavior
		CountrySelection countrySelection = gameObject.AddComponent<CountrySelection>();
		countrySelection.Init(globeViewModel, countrySelectorViewModel, mainCamera);
	}

	void FixedUpdate()
	{
		lock(m_lockObj)
		{
			if(m_sceneName == "DataScene")
			{
				CountrySelectorViewModel countrySelectorViewModel = ObjectDepot.Instance.Retrive<CountrySelectorViewModel>();
				CountrySelection countrySelection = gameObject.GetComponent<CountrySelection>();

				if((countrySelectorViewModel != null) && (countrySelection != null))
				{
					countrySelectorViewModel.PropertyChanged -= countrySelection.PropertyChangedHandler;
				}

				Application.LoadLevel("GlobeToDataScene");
			}
		}
	}

	public void ChangeSceneHandler(object sender, ChangeSceneEventArgs args)
	{
		lock(m_lockObj)
		{
			m_sceneName = args.SceneName;
		}
	}
}
