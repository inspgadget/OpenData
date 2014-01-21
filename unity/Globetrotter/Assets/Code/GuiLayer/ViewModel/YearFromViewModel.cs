using UnityEngine;
using System.Collections;

using Globetrotter.ApplicationLayer;
using Globetrotter.InputLayer;
using System;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class YearFromViewModel : ViewModelBase
	{
		private DataController m_dataController;

		private int m_min;

		private DateTime m_lastChange;
		private int m_lastYear;
		private int m_max;
		private int m_yearCurrent;
		private bool m_loaded = false;
		
		public bool Loaded
		{
			get {
				return m_loaded;
			}
			set {
				m_loaded = value;
			}
		}		
		
		public int YearCurrent {
			get {
				return m_yearCurrent;
			}
		}
		
		public DateTime LastChange {
			get {
				return m_lastChange;
			}
		}
		
		public int LastYear {
			get {
				return m_lastYear;
			}
		}

		public int Min
		{
			get
			{
				lock(m_lockObj)
				{
					return m_min;
				}
			}
		}

		public int YearFrom
		{
			get
			{
				lock(m_lockObj)
				{
					return m_dataController.YearFrom;
				}
			}
		}

		public YearFromViewModel(DataController dataController, int min)
			: base()
		{
			m_dataController = dataController;

			m_min = min;
		}

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			if(ReactOnInput == true)
			{
				int delta = 0;

				if(args.HasInputType(InputType.ClickDouble) == true)
				{
					IndicatorSelectorViewModel vm = ObjectDepot.Instance.Retrive<IndicatorSelectorViewModel>();
					vm.Fetch();
				}

				if(args.InputTypes.And(InputType.ScrollLeft) == InputType.ScrollLeft)
				{
					delta = -1;
				}

				if(args.InputTypes.And(InputType.ScrollRight) == InputType.ScrollRight)
				{
					delta = 1;
				}
				
				if(args.InputTypes.And(InputType.WipeLeft) == InputType.WipeLeft)
				{
					delta = -10;
				}
				
				if(args.InputTypes.And(InputType.WipeRight) == InputType.WipeRight)
				{
					delta = 10;
				}

				if(delta != 0)
				{
					lock(m_lockObj)
					{
						int year = m_dataController.YearFrom + delta;

						if(year < m_min)
						{
							year = m_min;
						}
						else if(year > m_dataController.YearTo)
						{
							year = m_dataController.YearTo;
						}

						m_dataController.YearFrom = year;
						m_yearCurrent = year;
						m_lastChange = DateTime.Now;
						m_loaded = false;
					}
				}
			}
		}
	}
}
