using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using Globetrotter.DataLayer;
using Globetrotter.GuiLayer.ViewModel;

public class ChartBehavior : MonoBehaviour
{
	private object m_lockObj = new object();

	private ChartViewModel m_chartViewModel;

	private GameObject m_chartPlane;

	private GameObject m_xAxis;
	private GameObject m_yAxis;

	public GameObject[] m_currDataPoints;

	private GUIText m_xAxisNameText;
	private GUIText m_yAxisNameText;

	private GUIText m_prevYearText;
	private GUIText m_currYearText;
	private GUIText m_nextYearText;

	private GUIText m_yAxisHalfText;
	private GUIText m_yAxisMaxText;

	private GameObject[] m_legendObjects;
	private GUIText[] m_legend;

	private Material m_focusedObjectMaterial;
	private Material m_unfocusedObjectMaterial;

	private byte[] m_chartData;

	void Start()
	{
		for(int i = 0; i < m_currDataPoints.Length; i++)
		{
			m_currDataPoints[i].renderer.enabled = false;
			m_legendObjects[i].renderer.enabled = false;

			m_legend[i].text = string.Empty;
		}

		m_chartPlane.renderer.enabled = false;

		m_yAxisNameText.text = string.Empty;
	}

	void Update()
	{
		byte[] chartData = null;

		lock(m_lockObj)
		{
			chartData = m_chartData;
		}

		if((chartData != null) && (chartData.Length > 0))
		{
			Texture2D texture = new Texture2D(1280, 648);
			texture.LoadImage(chartData);

			UnityEngine.Debug.Log("change texture");

			m_chartPlane.renderer.material.mainTexture = texture;
			m_chartPlane.renderer.enabled = true;

			m_yAxisNameText.text = m_chartViewModel.CurrentIndicator.Name;

			m_chartData = null;
		}

		/*lock(m_chartViewModel.LockObject)
		{
			WorldBankIndicator indicator = m_chartViewModel.CurrentIndicator;

			//axes
			if(indicator != null)
			{
				m_yAxisNameText.text = indicator.Name;
			}
			else
			{
				m_yAxisNameText.text = "Y";
			}

			if(m_chartViewModel.Max > 0)
			{
				m_yAxisHalfText.text = string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("de-AT"),
				                                     "{0:0,0}", (m_chartViewModel.Max / 2));
				m_yAxisMaxText.text = string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("de-AT"),
				                                    "{0:0,0}", m_chartViewModel.Max);
			}
			else
			{
				m_yAxisHalfText.text = string.Empty;
				m_yAxisMaxText.text = string.Empty;
			}

			m_xAxisNameText.text = "Year";

			//m_prevYearText.text = m_chartViewModel.PreviousDataPoint != null ? m_chartViewModel.PreviousDataPoint.Year.ToString() : string.Empty;
			m_prevYearText.text = string.Empty;
			m_currYearText.text = m_chartViewModel.CurrentDataPoint != null ? m_chartViewModel.CurrentDataPoint.Year.ToString() : string.Empty;
			//m_nextYearText.text = m_chartViewModel.NextDataPoint != null ? m_chartViewModel.NextDataPoint.Year.ToString() : string.Empty;
			m_nextYearText.text = string.Empty;

			if(m_chartViewModel.ReactOnInput == true)
			{
				m_xAxis.renderer.material = m_focusedObjectMaterial;
				m_yAxis.renderer.material = m_focusedObjectMaterial;
			}
			else
			{
				m_xAxis.renderer.material = m_unfocusedObjectMaterial;
				m_yAxis.renderer.material = m_unfocusedObjectMaterial;
			}

			//legend and data
			string[] seriesNames = m_chartViewModel.SeriesNames;

			if(m_chartViewModel.CurrentDataPoint != null)
			{
				for(int i = 0; i < m_currDataPoints.Length; i++)
				{
					//legend
					m_legend[i].text = seriesNames[i];

					if(string.IsNullOrEmpty(seriesNames[i]) == false)
					{
						m_legendObjects[i].renderer.enabled = true;
					}
					else
					{
						m_legendObjects[i].renderer.enabled = false;
					}

					//data
					m_currDataPoints[i].renderer.enabled = true;

					float yScale = (float)((3.5 * m_chartViewModel.CurrentDataPoint[i]) / m_chartViewModel.Max);
					float yPosition = yScale / 2;

					m_currDataPoints[i].transform.localScale = new Vector3(m_currDataPoints[i].transform.localScale.x,
					                                                       		yScale,
					                                                       		m_currDataPoints[i].transform.localScale.z);
					m_currDataPoints[i].transform.position = new Vector3(m_currDataPoints[i].transform.position.x,
					                                                     	yPosition,
					                                                     	m_currDataPoints[i].transform.position.z);
				}
			}
		}*/
	}

	public void Init(ChartViewModel chartViewModel,
	                 	GameObject[] currDataPoints,
	                 	GameObject chartPlane,
	                 	GameObject xAxis, GameObject yAxis,
	                 	GUIText xAxisNameText, GUIText yAxisNameText,
	                 	GUIText prevYearText, GUIText currYearText, GUIText nextYearText,
	                 	GUIText yAxisHalfText, GUIText yAxisMaxText, 
	                 	GameObject[] legendObjects, GUIText[] legend,
	                 	Material focusedObjectMaterial, Material unfocusedObjectMaterial)
	{
		m_chartViewModel = chartViewModel;
		m_chartViewModel.PropertyChanged += PropertyChangedHandler;

		m_currDataPoints = currDataPoints;

		m_chartPlane = chartPlane;

		m_xAxis = xAxis;
		m_yAxis = yAxis;

		m_xAxisNameText = xAxisNameText;
		m_yAxisNameText = yAxisNameText;

		m_prevYearText = prevYearText;
		m_currYearText = currYearText;
		m_nextYearText = nextYearText;

		m_yAxisHalfText = yAxisHalfText;
		m_yAxisMaxText = yAxisMaxText;

		m_legendObjects = legendObjects;
		m_legend = legend;

		m_focusedObjectMaterial = focusedObjectMaterial;
		m_unfocusedObjectMaterial = unfocusedObjectMaterial;

		m_chartData = null;
	}

	public void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
	{
		lock(m_lockObj)
		{
			if(sender == m_chartViewModel)
			{
				if(args.PropertyName == "ChartData")
				{
					m_chartData = m_chartViewModel.ChartData;
				}
			}
		}
	}
}
