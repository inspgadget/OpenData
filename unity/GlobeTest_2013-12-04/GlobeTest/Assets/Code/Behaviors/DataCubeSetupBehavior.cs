using UnityEngine;
using System.Collections;

using GlobeTest;
using GlobeTest.InputLayer;

public class DataCubeSetupBehavior : MonoBehaviour
{
	void Start()
	{
		//input controller
		IInputController inputController = ObjectDepot.Instance.Retrive<IInputController>();
		
		if(inputController == null)
		{
			inputController = gameObject.AddComponent<KeyboardInputBehavior>();
			((KeyboardInputBehavior)inputController).useKeyUp = false;
		}
		
		//data cube camera behavior
		DataCubeCameraBehavior dataCubeCameraBehavior = gameObject.AddComponent<DataCubeCameraBehavior>();
		dataCubeCameraBehavior.Init(inputController);
	}
}
