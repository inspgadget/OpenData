using UnityEngine;
using System.Collections;

using Globetrotter.GuiLayer;
using Globetrotter.GuiLayer.ViewModel;

public class YearRangeBehavior : MonoBehaviour
{
	private GameObject m_yearRangeObj;
	private GameObject m_yearFromObj;
	private GameObject m_yearToObj;

	private Material m_focusedObjectMaterial;
	private Material m_unfocusedObjectMaterial;

	private YearFromViewModel m_yearFromViewModel;
	private YearToViewModel m_yearToViewModel;

	private float m_offsetX;

	void Start()
	{
		m_offsetX = -4.0f;
	}

	void OnGUI()
	{
		int yearFrom = m_yearFromViewModel.YearFrom;
		int yearTo = m_yearToViewModel.YearTo;

		//left, top, width, height
		int screenWidth = Screen.width;
		int screenWidthHalf = screenWidth / 2;
		int screenHeight = Screen.height;
		int screenHeigthHalf = screenHeight / 2;

		GUIStyle style = StyleDepot.Instance.UnfocusedBoxStyle;

		if((m_yearFromViewModel.ReactOnInput || m_yearToViewModel.ReactOnInput) == true)
		{
			style = StyleDepot.Instance.FocusedBoxStyle;
		}

		GUI.Box(new Rect(screenWidthHalf - 40, screenHeigthHalf - 130, 80, 20), string.Empty, style);

		GUI.Label(new Rect(screenWidthHalf - 40, screenHeigthHalf - 130, 80, 20),
		          	yearFrom + " - " + yearTo,
		          	StyleDepot.Instance.UnfocusedTextStyle);
	}

	void Update()
	{
		//positions
		int yearFrom = m_yearFromViewModel.YearFrom;
		int yearTo = m_yearToViewModel.YearTo;

		int min = m_yearFromViewModel.Min;
		int max = m_yearToViewModel.Max;

		int yearRange = max - min;

		float xYearFrom = ((8.0f * (yearFrom - min)) / yearRange) + m_offsetX;
		float xYearTo = ((8.0f * (yearTo - min)) / yearRange) + m_offsetX;

		m_yearFromObj.transform.position = new Vector3(xYearFrom,
		                                               	m_yearFromObj.transform.position.y,
		                                               	m_yearFromObj.transform.position.z);
		m_yearToObj.transform.position = new Vector3(xYearTo,
		                                             	m_yearToObj.transform.position.y,
		                                             	m_yearToObj.transform.position.z);

		//materials
		bool yearFromActive = m_yearFromViewModel.ReactOnInput;
		bool yearToActive = m_yearToViewModel.ReactOnInput;
		bool active = yearFromActive || yearToActive;

		if(active == true)
		{
			m_yearRangeObj.renderer.material = m_focusedObjectMaterial;

			if(yearFromActive == true)
			{
				m_yearFromObj.renderer.material = m_focusedObjectMaterial;
			}
			else
			{
				m_yearFromObj.renderer.material = m_unfocusedObjectMaterial;
			}

			if(yearToActive == true)
			{
				m_yearToObj.renderer.material = m_focusedObjectMaterial;
			}
			else
			{
				m_yearToObj.renderer.material = m_unfocusedObjectMaterial;
			}
		}
		else
		{
			m_yearRangeObj.renderer.material = m_unfocusedObjectMaterial;
			m_yearFromObj.renderer.material = m_unfocusedObjectMaterial;
			m_yearToObj.renderer.material = m_unfocusedObjectMaterial;
		}
	}

	public void Init(YearFromViewModel yearFromViewModel, YearToViewModel yearToViewModel,
	                 	GameObject yearRangeObj, GameObject yearFromObj, GameObject yearToObj,
	                 	Material focusedObjectMaterial, Material unfocusedObjectMaterial)
	{
		m_yearFromViewModel = yearFromViewModel;
		m_yearToViewModel = yearToViewModel;
		
		m_yearRangeObj = yearRangeObj;
		m_yearFromObj = yearFromObj;
		m_yearToObj = yearToObj;

		m_focusedObjectMaterial = focusedObjectMaterial;
		m_unfocusedObjectMaterial = unfocusedObjectMaterial;
	}
}
