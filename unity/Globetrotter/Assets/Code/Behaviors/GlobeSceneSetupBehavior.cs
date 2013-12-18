using UnityEngine;
using System.Collections;

using Globetrotter;
using Globetrotter.ApplicationLayer;
using Globetrotter.InputLayer;

public class GlobeSceneSetupBehavior : MonoBehaviour
{
	void Start()
	{
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

		//globe behavior
		GlobeBehavior globeBehavior = gameObject.AddComponent<GlobeBehavior>();
		globeBehavior.Init(inputController);

		//country info behvior
		CountryInfoBehavior countryInfoBehavior = gameObject.AddComponent<CountryInfoBehavior>();
		countryInfoBehavior.Init(countriesController);
	}
}
