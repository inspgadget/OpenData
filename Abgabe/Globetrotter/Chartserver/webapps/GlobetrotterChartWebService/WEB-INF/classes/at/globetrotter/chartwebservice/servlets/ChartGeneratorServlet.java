package at.globetrotter.chartwebservice.servlets;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.HashMap;
import java.util.Map;

import javax.servlet.ServletConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletOutputStream;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.xpath.XPath;
import javax.xml.xpath.XPathConstants;
import javax.xml.xpath.XPathExpression;
import javax.xml.xpath.XPathFactory;

import org.w3c.dom.Document;

import at.globetrotter.chartwebservice.report.generator.BirtReportGenerator;
import at.globetrotter.chartwebservice.report.templates.ChartType;
import at.globetrotter.chartwebservice.report.templates.TemplateManager;

@WebServlet("/ChartGeneratorServlet")
public class ChartGeneratorServlet extends HttpServlet {
	
	private static final long serialVersionUID = 1L;
	
	private Object m_lockObj = new Object();
	
	private final String PARAM_CHART_TYPE = "charttype";
	private final String PARAM_COUNTRIES = "countries";
	private final String PARAM_IMAGE_PATH = "imagepath";
	private final String PARAM_INDICATOR = "indicator";
	private final String PARAM_YEAR_FROM = "yearfrom";
	private final String PARAM_YEAR_TO = "yearto";
	
    public ChartGeneratorServlet() {
        super();
    }
    
    @Override
	public void init(ServletConfig config) throws ServletException {
    	super.init(config);
    	
    	deleteChartsCache();
	}
	
    @Override
	public void destroy() {
    	super.destroy();
    	
    	deleteChartsCache();
	}
	
    @Override
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
    	String chartTypeStr = request.getParameter(PARAM_CHART_TYPE);
    	String countriesStr = request.getParameter(PARAM_COUNTRIES);
    	String imagePathStr = request.getParameter(PARAM_IMAGE_PATH);
    	String indicatorStr = request.getParameter(PARAM_INDICATOR);
    	String yearFromStr = request.getParameter(PARAM_YEAR_FROM);
    	String yearToStr = request.getParameter(PARAM_YEAR_TO);
    	
    	boolean useImagePath = false;
    	
    	if(isStringNullOrEmpty(imagePathStr) == false) {
    		useImagePath = Boolean.parseBoolean(imagePathStr);
    	}
    	
    	if(useImagePath == true) {
    		synchronized(m_lockObj) {
		    	if((isStringNullOrEmpty(countriesStr) == false) && (isStringNullOrEmpty(indicatorStr) == false) &&
		    		(isStringNullOrEmpty(yearFromStr) == false) && (isStringNullOrEmpty(yearToStr) == false)) {
		    		try {
		    			int yearFrom = Integer.parseInt(yearFromStr);
		    			int yearTo = Integer.parseInt(yearToStr);
		    			
		    			ChartType chartType = ChartType.LINECHART;
		    			
		    			if(isStringNullOrEmpty(chartTypeStr) == false) {
		    				chartType = ChartType.valueOf(ChartType.class, chartTypeStr.toUpperCase());
		    			}
		    			
		    			Map<String, Object> params = new HashMap<>();
		    			params.put(BirtReportGenerator.PARAM_URL,
		    						"http://api.worldbank.org/en/countries/" + countriesStr + "/indicators/" + indicatorStr +
		    							"?per_page=2048&date=" + yearFrom + ":" + yearTo);
		    			
		    			try(InputStream reportIs = TemplateManager.getInstance().getTemplateForChartType(chartType);
		    					ByteArrayOutputStream reportOs = new ByteArrayOutputStream();) {
		    				//delete old files
		    				deleteChartsCache();
		    				
		    				//create report
		    				BirtReportGenerator brg = new BirtReportGenerator();
		    				brg.runAndRenderReport(reportIs, reportOs, params,
		    						getServletContext().getRealPath("/charts"));
		    				
		    				//sent the image name
		    				try(BufferedReader br = new BufferedReader(new InputStreamReader(
		    															new ByteArrayInputStream(reportOs.toByteArray())));) {
		    					File imageFile = new File(getServletContext().getRealPath("/charts/custom1.png"));
		    					byte[] bytes = imageFile.getAbsolutePath().getBytes();
		    					response.setStatus(HttpServletResponse.SC_OK);
		    					response.setContentLength(bytes.length);
		    					ServletOutputStream sos = response.getOutputStream();
		    					sos.write(bytes);
		    					sos.flush();
		    					sos.close();
		    				} catch(Exception exc) {
		    					throw exc;
		    				}
		    			} catch(Exception exc) {
		        			exc.printStackTrace();
		        			
		        			response.sendError(HttpServletResponse.SC_INTERNAL_SERVER_ERROR, "Internal Server Error!!!");
		    			}
		    		} catch(Exception exc) {
		    			exc.printStackTrace();
		    			
		    			response.sendError(HttpServletResponse.SC_BAD_REQUEST, "Bad Request!!!");
		    		}
		    	} else {
					response.sendError(HttpServletResponse.SC_BAD_REQUEST, "Bad Request!!!");
		    	}
	    	}
    	} else {
	    	synchronized(m_lockObj) {
		    	
		    	if((isStringNullOrEmpty(countriesStr) == false) && (isStringNullOrEmpty(indicatorStr) == false) &&
		    		(isStringNullOrEmpty(yearFromStr) == false) && (isStringNullOrEmpty(yearToStr) == false)) {
		    		try {
		    			int yearFrom = Integer.parseInt(yearFromStr);
		    			int yearTo = Integer.parseInt(yearToStr);
		    			
		    			ChartType chartType = ChartType.LINECHART;
		    			
		    			if(isStringNullOrEmpty(chartTypeStr) == false) {
		    				chartType = ChartType.valueOf(ChartType.class, chartTypeStr.toUpperCase());
		    			}
		    			
		    			Map<String, Object> params = new HashMap<>();
		    			params.put(BirtReportGenerator.PARAM_URL,
		    						"http://api.worldbank.org/en/countries/" + countriesStr + "/indicators/" + indicatorStr +
		    							"?per_page=2048&date=" + yearFrom + ":" + yearTo);
		    			
		    			try(InputStream reportIs = TemplateManager.getInstance().getTemplateForChartType(chartType);
		    					ByteArrayOutputStream reportOs = new ByteArrayOutputStream();) {
		    				//delete old files
		    				deleteChartsCache();
		    				
		    				//create report
		    				BirtReportGenerator brg = new BirtReportGenerator();
		    				brg.runAndRenderReport(reportIs, reportOs, params,
		    						getServletContext().getRealPath("/charts"));
		    				
		    				//get the image name
		    				try(BufferedReader br = new BufferedReader(new InputStreamReader(
		    															new ByteArrayInputStream(reportOs.toByteArray())));) {
		    					/*DocumentBuilderFactory documentFactory = DocumentBuilderFactory.newInstance();
		    					DocumentBuilder documentBuilder = documentFactory.newDocumentBuilder();
		    					
		    					StringBuilder sb = new StringBuilder();
		    					String line = null;
		    					br.readLine();
		    					
		    					while((line = br.readLine()) != null) {
		    						sb.append(line);
		    						sb.append("\r\n");
		    					}
		    					
		    					Document document = documentBuilder.parse(sb.toString());
		    					
		    					XPathFactory xPathFactory = XPathFactory.newInstance();
		    					XPath xPath = xPathFactory.newXPath();
		    					XPathExpression xPathExpression = xPath.compile("string(//img/@src)");
		    					
		    					String imageUrl = (String)xPathExpression.evaluate(document, XPathConstants.STRING);
		    					String imageName = imageUrl.substring(imageUrl.lastIndexOf('/') + 1);*/
		    					
		    					//response
		    					//File imageFile = new File(getServletContext().getRealPath("/charts") + "/" + imageName);
		    					//File imageFile = new File(getServletContext().getRealPath("/charts") + "/custom1.png");
		    					File imageFile = null;
		    			    	File chartsDirectory = new File(getServletContext().getRealPath("/charts"));
		    		    		File[] files = chartsDirectory.listFiles();
		    		    		
		    		    		if((files != null) && (files.length == 1)) {
		    		    			imageFile = files[0];
		    		    		}
		    					
		    					try(BufferedInputStream imageBis = new BufferedInputStream(
		    							new FileInputStream(imageFile))) {
		    						response.setStatus(HttpServletResponse.SC_OK);
		    						response.setHeader("Cache-Control", "no-cache");
		    						response.setContentType("image/png");
		    						response.setContentLength((int)imageFile.length());
		    						
		    						ServletOutputStream sos = response.getOutputStream();
		    						byte[] data = new byte[1024];
		    						int read = -1;
		    						
		    						while((read = imageBis.read(data)) > -1) {
		    							sos.write(data, 0, read);
		    						}
		    						
		    						sos.flush();
		    						sos.close();
		    					} catch(Exception exc) {
		    						throw exc;
		    					}
		    				} catch(Exception exc) {
		    					throw exc;
		    				}
		    			} catch(Exception exc) {
		        			exc.printStackTrace();
		        			
		        			response.sendError(HttpServletResponse.SC_INTERNAL_SERVER_ERROR, "Internal Server Error!!!");
		    			}
		    		} catch(Exception exc) {
		    			exc.printStackTrace();
		    			
		    			response.sendError(HttpServletResponse.SC_BAD_REQUEST, "Bad Request!!!");
		    		}
		    	} else {
					response.sendError(HttpServletResponse.SC_BAD_REQUEST, "Bad Request!!!");
		    	}
	    	}
    	}
	}
	
    @Override
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
    	doGet(request, response);
	}
    
    private void deleteChartsCache() {
    	File chartsDirectory = new File(getServletContext().getRealPath("/charts"));
    	
    	if((chartsDirectory.exists() == true) && (chartsDirectory.isDirectory() == true)) {
    		System.out.println("Delete generated chart files...");
    		
    		File[] files = chartsDirectory.listFiles();
    		
    		for(int i = 0; i < files.length; i++) {
    			try {
	    			boolean deleted = files[i].delete();
	    			
	    			System.out.println(files[i].getAbsolutePath() + " deleted: " + deleted);
    			} catch(Exception exc) {
    				exc.printStackTrace();
    			}
    		}
    	}
    }
    
    private boolean isStringNullOrEmpty(String str) {
    	return ((str == null) || str.equals(""));
    }

}
