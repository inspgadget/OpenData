using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using GlobeTest;
using GlobeTest.ApplicationLayer;
using GlobeTest.DataLayer;

public class CenterScreenIndicatorDisplayBehavior : MonoBehaviour
{
	private object m_lockObject = new object();
	
	private DataController m_dataController;
	
	private LoadingBehavior m_loadingBehavior;
	
	private WorldBankData m_data;
	private bool m_newData;
	
	private Vector3 m_position;
	
	public LoadingBehavior LoadingBehavior
	{
		get
		{
			return m_loadingBehavior;
		}
		
		set
		{
			m_loadingBehavior = value;
		}
	}
	
	private bool NewData
	{
		get
		{
			lock(m_lockObject)
			{
				return m_newData;
			}
		}
		
		set
		{
			lock(m_lockObject)
			{
				m_newData = value;
			}
		}
	}
	
	private Vector3 Position
	{
		get
		{
			lock(m_lockObject)
			{
				return m_position;
			}
		}
		
		set
		{
			lock(m_lockObject)
			{
				m_position = value;
			}
		}
	}
	
	public void Init()
	{
		//get controllers
		m_dataController = ObjectDepot.Instance.Retrive<DataController>();
		
		//set behavior state
		m_loadingBehavior.Animate = false;
		
		//add event handlers
		m_dataController.PropertyChanged -= PropertyChangedHandler;
		m_dataController.PropertyChanged += PropertyChangedHandler;
	}
	
	/*void OnEnable()
	{
		m_data = null;
		m_newData = false;
		
		//m_dataController = new DataController();
		m_dataController = indicatorSelectorBehavior.DataController;
		m_dataController.PropertyChanged -= PropertyChangedHandler;
		m_dataController.PropertyChanged += PropertyChangedHandler;
		//
		loadingBehavior.Animate = false;
		//loadingBehavior.SetPosition(transform.position.x, transform.position.y, transform.position.z - 1.5f);
		//
	}*/
	
	void OnGUI()
	{
		if(NewData == true)
		{
			m_data = m_dataController.Data;
			NewData = false;
		}
		
		if(m_data != null)
		{
			WorldBankDataItem item = null;
			
			for(int i = (m_data.Items.Count - 1); i >= 0; i--)
			{
				if(m_data.Items[i].Value != 0)
				{
					item = m_data.Items[i];
					
					i = -1;
				}
			}
			
			if(item != null)
			{
				//left top width height
				int screenHeightCenter = Screen.height / 2;
				int screenWidthCenter = Screen.width / 2;
				
				string msg = m_data.IndicatorName + "\n\nValue: " + Math.Round(item.Value, 3) + "\nCountry: " +
								item.Country + "\nYear: " + item.Year;
				
				GUI.Box(new Rect(screenWidthCenter + 50, screenHeightCenter - 50, 300, 150), string.Empty);
				GUI.Label(new Rect(screenWidthCenter + 60, screenHeightCenter - 40, 280, 130), msg);
			}
		}
	}
	
	void FixedUpdate()
	{
		Position = transform.position;
	}
	
	private void AnimateLoading()
	{
		if(m_dataController.Fetching == true)
		{
			if(m_loadingBehavior.Animate == false)
			{
				m_loadingBehavior.Animate = true;
				m_loadingBehavior.SetPosition(Position.x, Position.y, Position.z - 1.5f);
			}
		}
		else
		{
			if(m_loadingBehavior.Animate == true)
			{
				m_loadingBehavior.Animate = false;
				m_loadingBehavior.SetPosition(Position.x, Position.y, Position.z);
			}
		}
	}
	
	public void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
	{
		if(sender == m_dataController)
		{
			switch(args.PropertyName)
			{
				case "Data":
					NewData = true;
					break;
					
				case "Fetching":
					AnimateLoading();
					break;
					
				default:
					break;
			}
		}
	}
}
