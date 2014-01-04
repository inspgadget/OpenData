using UnityEngine;
using System.Collections;

using Globetrotter;
using Globetrotter.ApplicationLayer;
using Globetrotter.GuiLayer.Controllers;
using Globetrotter.GuiLayer.ViewModel;
using Globetrotter.InputLayer;

public class DataSceneSetupBehavior : MonoBehaviour
{
	private object m_lockObj = new object();

	private string m_sceneName;
	
	void Start()
	{
		m_sceneName = null;
		
		//input controller
		IInputController inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(inputController == null)
		{
			inputController = gameObject.AddComponent<KeyboardInputController>();
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

		//indicator selector view model
		IndicatorSelectorViewModel indicatorSelectorViewModel = ObjectDepot.Instance.Retrive<IndicatorSelectorViewModel>();

		if(indicatorSelectorViewModel == null)
		{
			indicatorSelectorViewModel = new IndicatorSelectorViewModel(countriesController, dataController);
			inputController.InputReceived -= indicatorSelectorViewModel.InputReceivedHandler;
			inputController.InputReceived += indicatorSelectorViewModel.InputReceivedHandler;
			
			ObjectDepot.Instance.Store<IndicatorSelectorViewModel>(indicatorSelectorViewModel);
		}

		//year from view model
		YearFromViewModel yearFromViewModel = new YearFromViewModel(dataController, 1960);
		inputController.InputReceived -= yearFromViewModel.InputReceivedHandler;
		inputController.InputReceived += yearFromViewModel.InputReceivedHandler;

		//indicator selector behavior
		IndicatorSelectorBehavior indicatorSelectorBehavior = gameObject.AddComponent<IndicatorSelectorBehavior>();
		indicatorSelectorBehavior.Init(indicatorSelectorViewModel);

		//year from view behavior
		YearFromBehavior yearFromBehavior = gameObject.AddComponent<YearFromBehavior>();
		yearFromBehavior.Init(yearFromViewModel);
	}
	
	void FixedUpdate()
	{
		lock(m_lockObj)
		{
			if(string.IsNullOrEmpty(m_sceneName) == false)
			{
				Application.LoadLevel(m_sceneName);
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
