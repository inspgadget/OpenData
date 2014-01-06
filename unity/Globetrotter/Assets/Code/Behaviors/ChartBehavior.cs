using UnityEngine;
using System.Collections.Generic;

using Globetrotter.DataLayer;
using Globetrotter.GuiLayer.ViewModel;

public class ChartBehavior : MonoBehaviour
{
	private ChartViewModel m_chartViewModel;

	private GUIText m_xAxisNameText;
	private GUIText m_yAxisNameText;

	private GUIText m_prevYearText;
	private GUIText m_currYearText;
	private GUIText m_nextYearText;

	private GUIText[] m_legend;

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

			m_prevYearText.text = m_chartViewModel.PreviousDataPoint != null ? m_chartViewModel.PreviousDataPoint.Year.ToString() : string.Empty;
			m_currYearText.text = m_chartViewModel.CurrentDataPoint != null ? m_chartViewModel.CurrentDataPoint.Year.ToString() : string.Empty;
			m_nextYearText.text = m_chartViewModel.PreviousDataPoint != null ? m_chartViewModel.PreviousDataPoint.Year.ToString() : string.Empty;


			//legend
			string[] seriesNames = m_chartViewModel.SeriesNames;

			for(int i = 0; i < seriesNames.Length; i++)
			{
				m_legend[i].text = seriesNames[i];
			}

			//data
		}
	}

	public void Init(ChartViewModel chartViewModel,
	                 	GUIText xAxisNameText, GUIText yAxisNameText,
	                 	GUIText prevYearText, GUIText currYearText, GUIText nextYearText,
	                 	GUIText[] legend)
	{
		m_chartViewModel = chartViewModel;

		m_xAxisNameText = xAxisNameText;
		m_yAxisNameText = yAxisNameText;

		m_prevYearText = prevYearText;
		m_currYearText = currYearText;
		m_nextYearText = nextYearText;

		m_legend = legend;
	}
}
