using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Globetrotter.GuiLayer.ViewModel
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected object m_lockObj = new object();

		protected bool m_reactOnInput;

		public object LockObject
		{
			get
			{
				return m_lockObj;
			}
		}

		public virtual bool ReactOnInput
		{
			get
			{
				lock(m_lockObj)
				{
					return m_reactOnInput;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_reactOnInput = value;
				}
			}
		}

		public ViewModelBase()
		{
			m_reactOnInput = true;
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
