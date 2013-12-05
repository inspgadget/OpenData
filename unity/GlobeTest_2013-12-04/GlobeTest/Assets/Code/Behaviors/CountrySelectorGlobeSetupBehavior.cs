using UnityEngine;
using System.Collections;

using GlobeTest;
using GlobeTest.ApplicationLayer;
using GlobeTest.InputLayer;

public class CountrySelectorGlobeSetupBehavior : MonoBehaviour
{
	public GameObject mainCamera;
	
	void Start()
	{
		//input controller
		IInputController inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(inputController == null)
		{
			inputController = gameObject.AddComponent<KeyboardInputBehavior>();
			((KeyboardInputBehavior)inputController).useKeyUp = false;
		}
		
		//country selector controller
		CountrySelectorController countrySelectorController = ObjectDepot.Instance.Retrive<CountrySelectorController>();
		
		if(countrySelectorController == null)
		{
			countrySelectorController = new CountrySelectorController();
			ObjectDepot.Instance.Store<CountrySelectorController>(countrySelectorController);
		}
		
		//globe behavior
		GlobeBehavior globeBehavior = gameObject.AddComponent<GlobeBehavior>();
		
		globeBehavior.reactOnKeyboard = false;
		globeBehavior.Init(inputController);
		
		//camera zoom behavior
		CameraZoomBehavior cameraZoomBehavior = mainCamera.GetComponent<CameraZoomBehavior>();
		
		if(cameraZoomBehavior != null)
		{
			cameraZoomBehavior.Init(inputController);
		}
	}
}
