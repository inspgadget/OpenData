using UnityEngine;
using System.Collections;

using Globetrotter.GuiLayer;
using Globetrotter.GuiLayer.ViewModel;
using System.IO;
using System;
using Globetrotter;

public class YearRangeBehavior : MonoBehaviour
{
	private GameObject m_yearRangeScaleObj;
	private GameObject m_yearFromObj;
	private GameObject m_yearToObj;

	private GUIText m_yearRangeText;

	private Material m_focusedObjectMaterial;
	private Material m_unfocusedObjectMaterial;

	private YearFromViewModel m_yearFromViewModel;
	private YearToViewModel m_yearToViewModel;

	private float m_offsetX;

	private DateTime m_firstRunDt = DateTime.MinValue;

	void Start()
	{
		m_offsetX = -4.0f;
	}

	void OnGUI(){
		Debug.Log("asdfdsffads");
		bool react = false;

		lock(m_yearFromViewModel.LockObject){
			if(m_yearFromViewModel.ReactOnInput){
				react = true;
			}
		}

		if(!react){
			lock(m_yearToViewModel.LockObject){
				if(m_yearToViewModel.ReactOnInput){
					react = true;
				}
			}
		}

		if(react){
			DateTime now = DateTime.Now;
			if(m_firstRunDt == DateTime.MinValue){
				m_firstRunDt = now;
			}
			TimeSpan ts = (now - m_firstRunDt);
			if(ts.TotalSeconds <= 6){
				Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/swipe_d_y.png");
				GUI.DrawTexture( new Rect( 0, 
				                          Screen.height - texture.height,
				                          texture.width,
				                          texture.height ), 
				                texture );
			} else if (ts.TotalSeconds <= 12){
				Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/doubletap_d.png");
				GUI.DrawTexture( new Rect( 0, 
				                          Screen.height - texture.height,
				                          texture.width,
				                          texture.height ), 
				                texture );
			} else if (ts.TotalSeconds <= 18){
				Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/longpress.png");
				GUI.DrawTexture( new Rect( 0, 
				                          Screen.height - texture.height,
				                          texture.width,
				                          texture.height ), 
				                texture );
			} else if (ts.TotalSeconds <= 24){
				Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/swipedownup.png");
				GUI.DrawTexture( new Rect( 0, 
				                          Screen.height - texture.height,
				                          texture.width,
				                          texture.height ), 
				                texture );
			}
		}
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

		//text
		m_yearRangeText.text = yearFrom + " - " + yearTo;

		//materials
		bool yearFromActive = m_yearFromViewModel.ReactOnInput;
		bool yearToActive = m_yearToViewModel.ReactOnInput;
		bool active = yearFromActive || yearToActive;

		if(active == true)
		{
			//m_yearRangeScaleObj.renderer.material = m_focusedObjectMaterial;

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
			m_yearRangeScaleObj.renderer.material = m_unfocusedObjectMaterial;
			m_yearFromObj.renderer.material = m_unfocusedObjectMaterial;
			m_yearToObj.renderer.material = m_unfocusedObjectMaterial;
		}

		if(!m_yearToViewModel.Loaded && m_yearToViewModel.YearCurrent != m_yearToViewModel.LastYear && (DateTime.Now - m_yearToViewModel.LastChange).TotalSeconds >= 2){
			IndicatorSelectorViewModel vm = ObjectDepot.Instance.Retrive<IndicatorSelectorViewModel>();
			vm.Fetch();
			m_yearToViewModel.Loaded = true;
		}

		if(!m_yearFromViewModel.Loaded && m_yearFromViewModel.YearCurrent != m_yearFromViewModel.LastYear && (DateTime.Now - m_yearFromViewModel.LastChange).TotalSeconds >= 2){
			IndicatorSelectorViewModel vm = ObjectDepot.Instance.Retrive<IndicatorSelectorViewModel>();
			vm.Fetch();
			m_yearFromViewModel.Loaded = true;
		}
	}

	private Texture2D loadTexture(string path){
		Texture2D texture = new Texture2D(256,200);
		FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
		byte[] imageData = new byte[fs.Length];
		fs.Read(imageData, 0, (int) fs.Length);
		texture.LoadImage(imageData);
		return texture;
	}

	public void Init(YearFromViewModel yearFromViewModel, YearToViewModel yearToViewModel,
	                 	GameObject yearRangeScaleObj, GameObject yearFromObj, GameObject yearToObj,
	                 	GUIText yearRangeText,
	                 	Material focusedObjectMaterial, Material unfocusedObjectMaterial)
	{
		m_yearFromViewModel = yearFromViewModel;
		m_yearToViewModel = yearToViewModel;
		
		m_yearRangeScaleObj = yearRangeScaleObj;
		m_yearFromObj = yearFromObj;
		m_yearToObj = yearToObj;

		m_yearRangeText = yearRangeText;

		m_focusedObjectMaterial = focusedObjectMaterial;
		m_unfocusedObjectMaterial = unfocusedObjectMaterial;
	}
}
