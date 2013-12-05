using UnityEngine;
using System.Collections;
using System.ComponentModel;

using GlobeTest;
using GlobeTest.ApplicationLayer;
using GlobeTest.DomainLayer;
using GlobeTest.InputLayer;

public class ContinentSelectorBehavior : MonoBehaviour
{
	private CountrySelectorController m_countrySelectorController;
	private DataController m_dataController;
	private IInputController m_inputController;
	
	private IndicatorSelectorBehavior m_indicatorSelectorBehavior;
	private GlobeBehavior m_globeBehavior;
	
	void Start()
	{
		//indicator selector
		m_indicatorSelectorBehavior = gameObject.GetComponent<IndicatorSelectorBehavior>();
		
		if(m_indicatorSelectorBehavior != null)
		{
			m_dataController = ObjectDepot.Instance.Retrive<DataController>();
			
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
		
		m_countrySelectorController.ContinentsHorizontal = false;
		
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
		
		m_globeBehavior = (GlobeBehavior)gameObject.GetComponent(typeof(GlobeBehavior));
		m_globeBehavior.reactOnKeyboard = false;
		
		//fetch data
		//m_dataController.FetchData(new string[] {m_countrySelectorController.CurrentCountry.IsoAlphaThreeCode });
	}
	
	void OnGUI()
	{
		//left top width height
		int screenHeightCenter = Screen.height / 2;
		int marginLeft = 10;
		
		Continent prevContinent = m_countrySelectorController.PreviousContinent;
		Continent currContinent = m_countrySelectorController.CurrentContinent;
		Continent nextContinent = m_countrySelectorController.NextContinent;
		
		//previous continent
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter - 90, 140, 20), prevContinent.Name);
		
		//current continent
		GUI.Box(new Rect(marginLeft, screenHeightCenter - 50, 150, 100), string.Empty);
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter + 30, 140, 20), currContinent.Name);
		
		//next continent
		GUI.Label(new Rect(marginLeft + 10, screenHeightCenter + 70, 140, 20), nextContinent.Name);
	}
	
	void FixedUpdate()
	{
		Continent currContinent = m_countrySelectorController.CurrentContinent;
		m_globeBehavior.RotateTo(currContinent.RotationX, currContinent.RotationY, currContinent.RotationZ);
	}
	
	public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		if(args.Confirm == true)
		{
			Application.LoadLevel("ContinentLevel");
		}
		
		if(args.Cancel == true)
		{
			Application.LoadLevel("GlobeTestMain");
		}
	}
	
	public void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
	{
		if(sender == m_dataController)
		{
			if(args.PropertyName == "CurrentContinent")
			{
				m_dataController.FetchData(null);
			}
		}
	}
}
