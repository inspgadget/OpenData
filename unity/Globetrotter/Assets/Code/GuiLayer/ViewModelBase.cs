using System;
using System.Collections.Generic;

namespace Globetrotter.GuiLayer
{
	public abstract class ViewModelBase
	{
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
	}
}
