using System;
using System.Collections.Generic;
using System.ComponentModel;

using GlobeTest.DataLayer;
using GlobeTest.DomainLayer;

namespace GlobeTest.ApplicationLayer
{
	public class DataCubeController : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		private object m_lockObject = new object();
		
		private CountrySelectorController m_countrySelectorController;
		private DataController m_dataController;
		
		private DataCube m_dataCube;
		
		public DataCube DataCube
		{
			get
			{
				lock(m_lockObject)
				{
					return m_dataCube;
				}
			}
		}
		
		public DataCubeController(CountrySelectorController countrySelectorController, DataController dataController)
		{
			m_countrySelectorController = countrySelectorController;
			m_dataController = dataController;
			
			m_dataCube = null;
		}
		
		public void FetchData()
		{
			m_dataController.PropertyChanged -= PropertyChangedHandler;
			m_dataController.PropertyChanged += PropertyChangedHandler;
			
			List<string> isoAlphaThreeCodes = new List<string>();
			
			foreach(Country country in m_countrySelectorController.PinnedCountries)
			{
				isoAlphaThreeCodes.Add(country.IsoAlphaThreeCode);
			}
			
			m_dataController.FetchData(isoAlphaThreeCodes.ToArray());
		}
		
		protected void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
		{
			if(sender == m_dataController)
			{
				lock(m_lockObject)
				{
					m_dataController.PropertyChanged -= PropertyChangedHandler;
					
					m_dataCube = new DataCube(m_dataController.Data);
					
					OnPropertyChanged("DataCube");
				}
			}
		}
		
		protected void OnPropertyChanged(string propertyName)
		{
			if((PropertyChanged != null) && (string.IsNullOrEmpty(propertyName) == false))
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
