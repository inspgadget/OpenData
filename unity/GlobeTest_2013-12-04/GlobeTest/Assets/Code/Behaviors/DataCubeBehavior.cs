using UnityEngine;
using System.Collections;
using System.ComponentModel;

using GlobeTest.ApplicationLayer;
using GlobeTest.DomainLayer;
using GlobeTest.InputLayer;

public class DataCubeBehavior : MonoBehaviour
{
	private object m_lockObject = new object();
	
	private IInputController m_inputController;
	private DataCubeController m_dataCubeController;
	
	private GameObject m_barPrefab;
	
	private bool m_dataFetched;
	
	public bool DataFetched
	{
		get
		{
			lock(m_lockObject)
			{
				return m_dataFetched;
			}
		}
		
		set
		{
			lock(m_lockObject)
			{
				m_dataFetched = value;
			}
		}
	}
	
	void Start()
	{
		m_dataCubeController.FetchData();
	}
	
	void Update()
	{
		if(DataFetched == true)
		{
		}
	}
	
	public void Init(IInputController inputController, CountrySelectorController countrySelectorController,
						DataController dataController, GameObject barPrefab)
	{
		m_inputController = inputController;
		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;
		
		m_dataCubeController = new DataCubeController(countrySelectorController, dataController);
		
		m_barPrefab = barPrefab;
		
		m_dataFetched = false;
	}
	
	public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		if(args.HasDirection(InputDirection.Forwards) == true)
		{
		}
		
		if(args.HasDirection(InputDirection.Backwards) == true)
		{
		}
	}
	
	public void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
	{
		if(sender == m_dataCubeController)
		{
			if(args.PropertyName == "DataCube")
			{
				DataFetched = true;
			}
		}
	}
}
