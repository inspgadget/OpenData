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

	public GameObject loading;

	public GameObject arrowLeft;
	public GameObject arrowRight;

	public GameObject yearRangeScale;
	public GameObject yearFrom;
	public GameObject yearTo;

	public GameObject xAxis;
	public GameObject yAxis;

	public GameObject currentDataPointOne;
	public GameObject currentDataPointTwo;
	public GameObject currentDataPointThree;
	public GameObject currentDataPointFour;
	public GameObject currentDataPointFive;

	public GUIText yearRangeText;

	public GUIText xAxisNameText;
	public GUIText yAxisNameText;

	public GUIText prevYearText;
	public GUIText currYearText;
	public GUIText nextYearText;

	public GameObject chartPlane;
	
	public GUIText yAxisHalfText;
	public GUIText yAxisMaxText;
	
	public GameObject seriesOneLegend;
	public GameObject seriesTwoLegend;
	public GameObject seriesThreeLegend;
	public GameObject seriesFourLegend;
	public GameObject seriesFiveLegend;

	public GUIText seriesOneLegendText;
	public GUIText seriesTwoLegendText;
	public GUIText seriesThreeLegendText;
	public GUIText seriesFourLegendText;
	public GUIText seriesFiveLegendText;

	public Material focusedObjectMaterial;
	public Material unfocusedObjectMaterial;

	private Vector3[,] m_arrowPositions;

	private DataSceneGuiController m_dataSceneGuiController;

	private string m_sceneName;
	
	void Start()
	{
		m_sceneName = null;

		m_arrowPositions = new Vector3[,] { { new Vector3(-4.5f, 5.1f, 0.0f), new Vector3(4.5f, 5.1f, 0.0f) },
											{ new Vector3(-4.5f, 4.25f, 0.0f), new Vector3(4.5f, 4.25f, 0.0f) },
											{ new Vector3(-4.5f, 4.25f, 0.0f), new Vector3(4.5f, 4.25f, 0.0f) },
											{ new Vector3(-4.5f, 1.5f, 0.0f), new Vector3(4.5f, 1.5f, 0.0f) }};
		
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

		dataController.DataPath = Application.dataPath;
		
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
			
			ObjectDepot.Instance.Store<IndicatorSelectorViewModel>(indicatorSelectorViewModel);
		}

		//set event handler outside to catch the special case for keyboard input
		inputController.InputReceived -= indicatorSelectorViewModel.InputReceivedHandler;
		inputController.InputReceived += indicatorSelectorViewModel.InputReceivedHandler;

		//year from view model
		YearFromViewModel yearFromViewModel = new YearFromViewModel(dataController, 1960);
		inputController.InputReceived += yearFromViewModel.InputReceivedHandler;
		
		//year to view model
		YearToViewModel yearToViewModel = new YearToViewModel(dataController, System.DateTime.Now.Year);
		inputController.InputReceived += yearToViewModel.InputReceivedHandler;

		//chart view model
		ChartViewModel chartViewModel = new ChartViewModel(dataController, indicatorSelectorViewModel);
		inputController.InputReceived += chartViewModel.InputReceivedHandler;
		
		//data scene gui controller
		m_dataSceneGuiController = new DataSceneGuiController(indicatorSelectorViewModel,
		                                                      	yearFromViewModel,
		                                                      	yearToViewModel,
		                                                      	chartViewModel,
		                                                      	inputController);
		m_dataSceneGuiController.ChangeScene += ChangeSceneHandler;

		//indicator selector behavior
		IndicatorSelectorBehavior indicatorSelectorBehavior = gameObject.AddComponent<IndicatorSelectorBehavior>();
		indicatorSelectorBehavior.Init(indicatorSelectorViewModel);

		//year range behavior
		YearRangeBehavior yearRangeBehavior = gameObject.AddComponent<YearRangeBehavior>();
		yearRangeBehavior.Init(yearFromViewModel, yearToViewModel,
		                       	yearRangeScale, yearFrom, yearTo,
		                       	yearRangeText,
		                       	focusedObjectMaterial, unfocusedObjectMaterial);

		//chart behavior
		ChartBehavior chartBehavior = gameObject.AddComponent<ChartBehavior>();
		chartBehavior.Init(chartViewModel,
		                   	new GameObject[] { currentDataPointOne, currentDataPointTwo, currentDataPointThree,
												currentDataPointFour, currentDataPointFive },
							chartPlane,
							xAxis, yAxis,
		                   	xAxisNameText, yAxisNameText,
		                   	prevYearText, currYearText, nextYearText,
							yAxisHalfText, yAxisMaxText,
							new GameObject[] { seriesOneLegend, seriesTwoLegend, seriesThreeLegend,
												seriesFourLegend, seriesFiveLegend} ,
		                    new GUIText[] { seriesOneLegendText, seriesTwoLegendText, seriesThreeLegendText,
												seriesFourLegendText, seriesFiveLegendText },
							focusedObjectMaterial, unfocusedObjectMaterial);

		//indicator loading behavior
		IndicatorLoadingBehavior indicatorLoadingBehavior = loading.AddComponent<IndicatorLoadingBehavior>();
		indicatorLoadingBehavior.Init(indicatorSelectorViewModel, 1.0f);
	}
	
	void FixedUpdate()
	{
		lock(m_lockObj)
		{
			if(m_sceneName == "GlobeScene")
			{
				IndicatorSelectorViewModel indicatorSelectorViewModel = ObjectDepot.Instance.Retrive<IndicatorSelectorViewModel>();
				IndicatorLoadingBehavior indicatorLoadingBehavior = loading.GetComponent<IndicatorLoadingBehavior>();

				if((indicatorSelectorViewModel != null) && (indicatorLoadingBehavior != null))
				{
					indicatorSelectorViewModel.PropertyChanged -= indicatorLoadingBehavior.PropertyChangedHandler;
				}

				Application.LoadLevel("DataToGlobeScene");
			}
		}
	}

	void Update()
	{
		int focusIndex = m_dataSceneGuiController.FocusIndex;

		arrowLeft.transform.position = m_arrowPositions[focusIndex, 0];
		arrowRight.transform.position = m_arrowPositions[focusIndex, 1];
	}
	
	public void ChangeSceneHandler(object sender, ChangeSceneEventArgs args)
	{
		lock(m_lockObj)
		{
			m_sceneName = args.SceneName;
		}
	}
}
