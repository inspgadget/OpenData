using UnityEngine;
using System.Collections;

using GlobeTest;
using GlobeTest.ApplicationLayer;
using GlobeTest.DataLayer;
using GlobeTest.InputLayer;

public class IndicatorSelectorBehavior : MonoBehaviour
{
	private DataController m_dataController;
	private IInputController m_inputController;
	
	/*public DataController DataController
	{
		get
		{
			return m_dataController;
		}
	}*/
	
	public void Init()
	{
		//get controllers
		m_dataController = ObjectDepot.Instance.Retrive<DataController>();
		m_inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(m_inputController == null)
		{
			m_inputController = gameObject.GetComponent<KeyboardInputBehavior>();
		}
		
		//add event handlers
		m_inputController.InputReceived -= m_dataController.InputReceivedHandler;
		m_inputController.InputReceived += m_dataController.InputReceivedHandler;
	}
	
	/*void OnEnable()
	{
		m_dataController = ObjectDepot.Instance.Retrive<DataController>();
		
		//data controller
		if(m_dataController == null)
		{
			m_dataController = new DataController();
			ObjectDepot.Instance.Store<DataController>(m_dataController);
		}
		
		//input controller
		m_inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(m_inputController == null)
		{
			m_inputController = (KeyboardInputBehavior)gameObject.AddComponent<KeyboardInputBehavior>();
		}
		
		m_inputController.InputReceived -= m_dataController.InputReceivedHandler;
		m_inputController.InputReceived += m_dataController.InputReceivedHandler;
		//
		//m_dataController.CurrentIndicator.FetchDataForCountry("aut");
		//
	}*/
	
	void OnGUI()
	{
		//left top width height
		int screenWidthCenter = Screen.width / 2;
		int marginTop = 10;
		
		WorldBankIndicator prevIndicator = m_dataController.PreviousIndicator;
		WorldBankIndicator currIndicator = m_dataController.CurrentIndicator;
		WorldBankIndicator nextIndicator = m_dataController.NextIndicator;
		
		//previous country
		//GUI.Label(new Rect(marginLeft + 10, screenHeightCenter - 150, 140, 65), prevCountry.PlaneTexture);
		GUI.Label(new Rect(screenWidthCenter - 255, marginTop + 10, 150, 70), prevIndicator.Name);
		
		//current continent
		GUI.Box(new Rect(screenWidthCenter - 85, marginTop, 170, 90), string.Empty);
		//GUI.Label(new Rect(screenWidthCenter + 10, marginTop, 140, 65), currCountry.PlaneTexture);
		GUI.Label(new Rect(screenWidthCenter - 75, marginTop + 10, 150, 70), currIndicator.Name);
		
		//next country
		//GUI.Label(new Rect(marginLeft + 10, screenHeightCenter + 70, 140, 65), nextCountry.PlaneTexture);
		GUI.Label(new Rect(screenWidthCenter + 105, marginTop + 10, 150, 70), nextIndicator.Name);
	}
}
