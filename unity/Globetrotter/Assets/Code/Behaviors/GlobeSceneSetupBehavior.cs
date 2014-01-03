using UnityEngine;
using System.Collections.Generic;

using Globetrotter;
using Globetrotter.ApplicationLayer;
using Globetrotter.GuiLayer;
using Globetrotter.InputLayer;

public class GlobeSceneSetupBehavior : MonoBehaviour
{
	public Camera mainCamera;
	public GameObject magnifyingGlass;

	void Start()
	{
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
		inputController.InputReceived -= globeViewModel.InputReceivedHandler;
		inputController.InputReceived += globeViewModel.InputReceivedHandler;

		//country selector view model
		CountrySelectorViewModel countrySelectorViewModel = ObjectDepot.Instance.Retrive<CountrySelectorViewModel>();

		if(countrySelectorViewModel == null)
		{
			countrySelectorViewModel = new CountrySelectorViewModel(countriesController);
			inputController.InputReceived -= countrySelectorViewModel.InputReceivedHandler;
			inputController.InputReceived += countrySelectorViewModel.InputReceivedHandler;

			ObjectDepot.Instance.Store<CountrySelectorViewModel>(countrySelectorViewModel);
		}

		//selected countries view model
		SelectedCountriesViewModel selectedCountriesViewModel = ObjectDepot.Instance.Retrive<SelectedCountriesViewModel>();

		if(selectedCountriesViewModel == null)
		{
			selectedCountriesViewModel = new SelectedCountriesViewModel(countriesController);
			inputController.InputReceived -= selectedCountriesViewModel.InputReceivedHandler;
			inputController.InputReceived += selectedCountriesViewModel.InputReceivedHandler;

			ObjectDepot.Instance.Store<SelectedCountriesViewModel>(selectedCountriesViewModel);
		}

		//globe behavior
		GlobeBehavior globeBehavior = gameObject.AddComponent<GlobeBehavior>();
		globeBehavior.Init(globeViewModel);

		//country selector behavior
		CountrySelectorBehavior countrySelectorBehavior = mainCamera.gameObject.AddComponent<CountrySelectorBehavior>();
		countrySelectorBehavior.Init(countrySelectorViewModel);

		//selected countries behavior
		SelectedCountriesBehavior selectedCountriesBehavior = mainCamera.gameObject.AddComponent<SelectedCountriesBehavior>();
		selectedCountriesBehavior.Init(selectedCountriesViewModel);

		//camera zoom behavior
		CameraZoomBehavior cameraZoomBehavior = mainCamera.gameObject.AddComponent<CameraZoomBehavior>();
		cameraZoomBehavior.Init(inputController, new float[]{ -1.5f, -3.0f }, new GameObject[]{ magnifyingGlass });

	}
}
