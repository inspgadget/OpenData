using UnityEngine;
using System.Collections;
using System.ComponentModel;

using GlobeTest;
using GlobeTest.ApplicationLayer;
using GlobeTest.DataLayer;
using GlobeTest.DomainLayer;
using GlobeTest.InputLayer;

public class CountrySelectorBehavior : MonoBehaviour
{
	private CountrySelectorController m_countrySelectorController;
	private DataController m_dataController;
	private IInputController m_inputController;
	
	private GlobeBehavior m_globeBehavior;
	
	public void Init()
	{
		//get controllers
		m_countrySelectorController = ObjectDepot.Instance.Retrive<CountrySelectorController>();
		m_dataController = ObjectDepot.Instance.Retrive<DataController>();
		m_inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(m_inputController == null)
		{
			m_inputController = gameObject.GetComponent<KeyboardInputBehavior>();
		}
		
		//set controller state
		m_countrySelectorController.ContinentsHorizontal = true;
		m_countrySelectorController.OnlyUseVertical = true;
		
		//get behaviors
		m_globeBehavior = gameObject.GetComponent<GlobeBehavior>();
		
		//add event handlers
		m_countrySelectorController.PropertyChanged -= PropertyChangedHandler;
		m_countrySelectorController.PropertyChanged += PropertyChangedHandler;
		
		m_dataController.PropertyChanged -= PropertyChangedHandler;
		m_dataController.PropertyChanged += PropertyChangedHandler;
		
		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;
		m_inputController.InputReceived -= m_countrySelectorController.InputReceivedHandler;
		m_inputController.InputReceived += m_countrySelectorController.InputReceivedHandler;
		
		//set state
		m_globeBehavior.reactOnKeyboard = false;
		
		//fetch data
		m_dataController.FetchData(new string[] {m_countrySelectorController.CurrentCountry.IsoAlphaThreeCode });
	}
	
	void Start()
	{
		Continent continent = m_countrySelectorController.CurrentContinent;
		m_globeBehavior.RotateTo(continent.RotationX, continent.RotationY, continent.RotationZ);
	}
	
	/*void Start()
	{
		//indicator selector
		//m_indicatorSelectorBehavior = gameObject.GetComponent<IndicatorSelectorBehavior>();
		
		if(m_indicatorSelectorBehavior != null)
		{
			m_dataController = m_indicatorSelectorBehavior.DataController;
			
			if(m_dataController != null)
			{
			}
		}
		
		//country selector controller
		m_countrySelectorController = ObjectDepot.Instance.Retrive<CountrySelectorController>();
		
		if(m_countrySelectorController == null)
		{
			m_countrySelectorController = new CountrySelectorController();
			ObjectDepot.Instance.Store<CountrySelectorController>(m_countrySelectorController);
		}
		
		m_countrySelectorController.ContinentsHorizontal = true;
		m_countrySelectorController.OnlyUseVertical = true;
		
		m_countrySelectorController.PropertyChanged -= PropertyChangedHandler;
		m_countrySelectorController.PropertyChanged += PropertyChangedHandler;
		
		//input controller
		m_inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(m_inputController == null)
		{
			m_inputController = gameObject.AddComponent<KeyboardInputBehavior>();
		}
		
		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;
		m_inputController.InputReceived -= m_countrySelectorController.InputReceivedHandler;
		m_inputController.InputReceived += m_countrySelectorController.InputReceivedHandler;
		
		//m_globeBehavior = gameObject.GetComponent<GlobeBehavior>();
		
		if(m_globeBehavior != null)
		{
			m_globeBehavior.reactOnKeyboard = false;
			
			Continent continent = m_countrySelectorController.CurrentContinent;
			m_globeBehavior.RotateTo(continent.RotationX, continent.RotationY, continent.RotationZ);
		}
		
		//fetch data
		m_dataController.FetchData(new string[] {m_countrySelectorController.CurrentCountry.IsoAlphaThreeCode });
	}*/
	
	void OnGUI()
	{
		//left top width height
		int screenHeightCenter = Screen.height / 2;
		int marginLeft = 10;
		
		Country prevCountry = m_countrySelectorController.PreviousCountry;
		Country currCountry = m_countrySelectorController.CurrentCountry;
		Country nextCountry = m_countrySelectorController.NextCountry;
		
		//previous country
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter - 150, 140, 65), prevCountry.PlaneTexture);
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter - 80, 140, 20), prevCountry.Name);
		
		//current continent
		GUI.Box(new Rect(marginLeft, screenHeightCenter - 50, 150, 100), string.Empty);
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter - 40, 140, 65), currCountry.PlaneTexture);
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter + 30, 140, 20), currCountry.Name);
		
		//next country
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter + 70, 140, 65), nextCountry.PlaneTexture);
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter + 140, 140, 20), nextCountry.Name);
	}
	
	void FixedUpdate()
	{
	}
	
	public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		if(args.Confirm == true)
		{
			//
		}
		
		if(args.Cancel == true)
		{
			Application.LoadLevel("ContinentLevel");
		}
	}
	
	public void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
	{
		if(sender == m_countrySelectorController)
		{
			if(args.PropertyName == "CurrentCountry")
			{
				m_dataController.FetchData(new string[] { m_countrySelectorController.CurrentCountry.IsoAlphaThreeCode });
			}
		}
		else if(sender == m_dataController)
		{
			if(args.PropertyName == "CurrentIndicator")
			{
				m_dataController.FetchData(new string[] { m_countrySelectorController.CurrentCountry.IsoAlphaThreeCode });
			}
		}
	}
}
