package at.globetrotter.chartwebservice.report.generator;

import java.io.InputStream;
import java.io.OutputStream;
import java.util.Map;

import org.eclipse.birt.core.exception.BirtException;
import org.eclipse.birt.report.engine.api.HTMLRenderOption;
import org.eclipse.birt.report.engine.api.IReportEngine;
import org.eclipse.birt.report.engine.api.IReportRunnable;
import org.eclipse.birt.report.engine.api.IRunAndRenderTask;
import org.eclipse.birt.report.engine.api.RenderOption;

public class BirtReportGenerator {
	
	public static final String PARAM_URL = "paramUrl";
	
	public BirtReportGenerator() {
	}
	
	public void runAndRenderReport(InputStream reportIs, OutputStream os, Map<String, Object> params,
			String imageDirectory) throws BirtException {
		IReportEngine engine = null;
		IRunAndRenderTask task = null;
		
		try {
			engine = BirtPlatformManager.getInstance().getEngine();
			IReportRunnable report = engine.openReportDesign(reportIs);
			task = engine.createRunAndRenderTask(report);
			
			HTMLRenderOption renderOption = new HTMLRenderOption();
			renderOption.setImageDirectory(imageDirectory);
			renderOption.setOutputFormat(RenderOption.OUTPUT_FORMAT_HTML);
			renderOption.setSupportedImageFormats("JPG;PNG;BMP;SVG");
			renderOption.setOutputStream(os);
			
			task.setRenderOption(renderOption);
			task.setParameterValues(params);
			task.run();
		} finally {
			try {
				task.close();
			} catch(Exception exc) {}

			try {
				engine.destroy();
			} catch(Exception exc) {}
		}
	}
}
