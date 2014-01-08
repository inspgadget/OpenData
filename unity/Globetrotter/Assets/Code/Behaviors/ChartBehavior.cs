using UnityEngine;
using System.Collections.Generic;

using Globetrotter.DataLayer;
using Globetrotter.GuiLayer.ViewModel;

public class ChartBehavior : MonoBehaviour
{
	private ChartViewModel m_chartViewModel;

	private GameObject m_xAxis;
	private GameObject m_yAxis;
	
	public GameObject[] m_currDataPoints;

	private GUIText m_xAxisNameText;
	private GUIText m_yAxisNameText;

	private GUIText m_prevYearText;
	private GUIText m_currYearText;
	private GUIText m_nextYearText;

	private GUIText[] m_legend;

	private Material m_focusedObjectMaterial;
	private Material m_unfocusedObjectMaterial;

	void Update()
	{
		lock(m_chartViewModel.LockObject)
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

			m_xAxisNameText.text = "Year";

			m_prevYearText.text = m_chartViewModel.PreviousDataPoint != null ? m_chartViewModel.PreviousDataPoint.Year.ToString() : string.Empty;
			m_currYearText.text = m_chartViewModel.CurrentDataPoint != null ? m_chartViewModel.CurrentDataPoint.Year.ToString() : string.Empty;
			m_nextYearText.text = m_chartViewModel.NextDataPoint != null ? m_chartViewModel.NextDataPoint.Year.ToString() : string.Empty;

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


			//legend
			string[] seriesNames = m_chartViewModel.SeriesNames;

			for(int i = 0; i < seriesNames.Length; i++)
			{
				m_legend[i].text = seriesNames[i];
			}

			//data
			if(m_chartViewModel.CurrentDataPoint != null)
			{
				float yScale = (float)((3.5 * m_chartViewModel.CurrentDataPoint[0]) / m_chartViewModel.Max);
				float yPosition = yScale / 2;

				m_currDataPoints[0].transform.localScale = new Vector3(m_currDataPoints[0].transform.localScale.x,
				                                                         	yScale,
				                                                       		m_currDataPoints[0].transform.localScale.z);
				m_currDataPoints[0].transform.position = new Vector3(m_currDataPoints[0].transform.position.x,
				                                                     	yPosition,
				                                                     	m_currDataPoints[0].transform.position.z);

				yScale = (float)((3.5 * m_chartViewModel.CurrentDataPoint[1]) / m_chartViewModel.Max);
				yPosition = yScale / 2;
				
				m_currDataPoints[1].transform.localScale = new Vector3(m_currDataPoints[1].transform.localScale.x,
				                                                         	yScale,
				                                                       		m_currDataPoints[1].transform.localScale.z);
				m_currDataPoints[1].transform.position = new Vector3(m_currDataPoints[1].transform.position.x,
				                                                     	yPosition,
				                                                     	m_currDataPoints[1].transform.position.z);
				
				yScale = (float)((3.5 * m_chartViewModel.CurrentDataPoint[2]) / m_chartViewModel.Max);
				yPosition = yScale / 2;
				
				m_currDataPoints[2].transform.localScale = new Vector3(m_currDataPoints[2].transform.localScale.x,
				                                                           	yScale,
				                                                       		m_currDataPoints[2].transform.localScale.z);
				m_currDataPoints[2].transform.position = new Vector3(m_currDataPoints[2].transform.position.x,
				                                                     		yPosition,
				                                                     		m_currDataPoints[2].transform.position.z);

				yScale = (float)((3.5 * m_chartViewModel.CurrentDataPoint[3]) / m_chartViewModel.Max);
				yPosition = yScale / 2;
				
				m_currDataPoints[3].transform.localScale = new Vector3(m_currDataPoints[3].transform.localScale.x,
				                                                          	yScale,
				                                                       		m_currDataPoints[3].transform.localScale.z);
				m_currDataPoints[3].transform.position = new Vector3(m_currDataPoints[3].transform.position.x,
				                                                     	yPosition,
				                                                     	m_currDataPoints[3].transform.position.z);

				yScale = (float)((3.5 * m_chartViewModel.CurrentDataPoint[4]) / m_chartViewModel.Max);
				yPosition = yScale / 2;
				
				m_currDataPoints[4].transform.localScale = new Vector3(m_currDataPoints[4].transform.localScale.x,
				                                                          	yScale,
				                                                       		m_currDataPoints[4].transform.localScale.z);
				m_currDataPoints[4].transform.position = new Vector3(m_currDataPoints[4].transform.position.x,
				                                                     	yPosition,
				                                                     	m_currDataPoints[4].transform.position.z);
			}
		}
	}

	public void Init(ChartViewModel chartViewModel,
	                 	GameObject[] currDataPoints,
	                 	GameObject xAxis, GameObject yAxis,
	                 	GUIText xAxisNameText, GUIText yAxisNameText,
	                 	GUIText prevYearText, GUIText currYearText, GUIText nextYearText,
	                 	GUIText[] legend,
	                 	Material focusedObjectMaterial, Material unfocusedObjectMaterial)
	{
		m_chartViewModel = chartViewModel;

		m_currDataPoints = currDataPoints;

		m_xAxis = xAxis;
		m_yAxis = yAxis;

		m_xAxisNameText = xAxisNameText;
		m_yAxisNameText = yAxisNameText;

		m_prevYearText = prevYearText;
		m_currYearText = currYearText;
		m_nextYearText = nextYearText;

		m_legend = legend;

		m_focusedObjectMaterial = focusedObjectMaterial;
		m_unfocusedObjectMaterial = unfocusedObjectMaterial;
	}
}
