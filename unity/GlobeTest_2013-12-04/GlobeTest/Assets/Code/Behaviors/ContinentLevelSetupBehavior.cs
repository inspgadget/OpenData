using UnityEngine;
using System.Collections;

using GlobeTest;
using GlobeTest.ApplicationLayer;
using GlobeTest.InputLayer;

public class ContinentLevelSetupBehavior : MonoBehaviour
{
	public LoadingBehavior loadingBehavior;
	public GameObject mainCamera;
	
	void Start()
	{
		//input controller
		IInputController inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(inputController == null)
		{
			inputController = gameObject.AddComponent<KeyboardInputBehavior>();
		}
		
		//country selector controller
		CountrySelectorController countrySelectorController = ObjectDepot.Instance.Retrive<CountrySelectorController>();
		
		if(countrySelectorController == null)
		{
			countrySelectorController = new CountrySelectorController();
			ObjectDepot.Instance.Store<CountrySelectorController>(countrySelectorController);
		}
		
		//data controller
		DataController dataController = ObjectDepot.Instance.Retrive<DataController>();
		
		if(dataController == null)
		{
			dataController = new DataController();
			ObjectDepot.Instance.Store<DataController>(dataController);
		}
		
		//add bevahiors
		GlobeBehavior globeBehavior = gameObject.AddComponent<GlobeBehavior>();
		
		IndicatorSelectorBehavior indicatorSelectorBehavior = gameObject.AddComponent<IndicatorSelectorBehavior>();
		indicatorSelectorBehavior.Init();
		
		CenterScreenIndicatorDisplayBehavior centerScrennIndicatorBehavior = mainCamera.AddComponent<CenterScreenIndicatorDisplayBehavior>();
		centerScrennIndicatorBehavior.LoadingBehavior = loadingBehavior;
		centerScrennIndicatorBehavior.Init();
		
		CountrySelectorBehavior countrySelectorBehavior = gameObject.AddComponent<CountrySelectorBehavior>();
		countrySelectorBehavior.Init();
	}
}
