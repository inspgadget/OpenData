package at.globetrotter.chartwebservice.report.templates;

import java.io.InputStream;

public class TemplateManager {
	
	private static TemplateManager m_instance = null;
	
	private TemplateManager() {
	}
	
	public static synchronized TemplateManager getInstance() {
		if(m_instance == null) {
			m_instance = new TemplateManager();
		}
		
		return m_instance;
	}
	
	public synchronized InputStream getTemplateForChartType(ChartType chartType) {
		switch(chartType) {
			case BARCHART:
				return getClass().getResourceAsStream("WorldBankBarChart.rptdesign");
				
			case DETAILEDBARCHART:
				return getClass().getResourceAsStream("WorldBankDetailedBarChart.rptdesign");
				
			case DETAILEDLINECHART:
				return getClass().getResourceAsStream("WorldBankDetailedLineChart.rptdesign");
				
			case LINECHART:
				return getClass().getResourceAsStream("WorldBankLineChart.rptdesign");
				
			default:
				return null;
		}
	}
}
