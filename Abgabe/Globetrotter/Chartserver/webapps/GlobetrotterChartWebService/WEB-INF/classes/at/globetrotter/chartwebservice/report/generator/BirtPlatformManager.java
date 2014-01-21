package at.globetrotter.chartwebservice.report.generator;

import org.eclipse.birt.core.exception.BirtException;
import org.eclipse.birt.core.framework.Platform;
import org.eclipse.birt.report.engine.api.EngineConfig;
import org.eclipse.birt.report.engine.api.IReportEngine;
import org.eclipse.birt.report.engine.api.IReportEngineFactory;

public class BirtPlatformManager {
	
	private static BirtPlatformManager m_instance = null;
	
	private boolean m_started;
	
	private EngineConfig m_engineConfig;
	IReportEngineFactory m_factory;
	
	private BirtPlatformManager() {
		m_started = false;
		
		m_engineConfig = null;
		m_factory = null;
	}
	
	public static synchronized BirtPlatformManager getInstance() {
		if(m_instance == null) {
			m_instance = new BirtPlatformManager();
		}
		
		return m_instance;
	}
	
	public synchronized IReportEngine getEngine() throws BirtException {
		startPlatform();
		
		return m_factory.createReportEngine(m_engineConfig);
	}
	
	private void startPlatform() throws BirtException {
		if(m_started == false) {
			m_engineConfig = new EngineConfig();
			Platform.startup(m_engineConfig);
			m_factory = (IReportEngineFactory) Platform.createFactoryObject(IReportEngineFactory.EXTENSION_REPORT_ENGINE_FACTORY);
			
			m_started = true;
		}
	}
}
