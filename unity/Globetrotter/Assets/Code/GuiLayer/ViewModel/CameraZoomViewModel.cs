using System;
using System.Collections.Generic;

using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class CameraZoomViewModel : ViewModelBase
	{
		private float m_z;
		private float[] m_limits;
		private float m_speed;

		public float Z
		{
			get
			{
				lock(m_lockObj)
				{
					return m_z;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_z = value;
				}
			}
		}

		public CameraZoomViewModel(float[] limits, float speed)
			: base()
		{
			m_limits = limits;
			m_speed = speed;
		}

		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					if(args.HasInputType(InputType.ZoomIn) == true)
					{
						float z = m_z + m_speed;
						
						if(z <= m_limits[0])
						{
							m_z = z;
						}
					}
					
					if(args.HasInputType(InputType.ZoomOut) == true)
					{
						float z = m_z - m_speed;
						
						if(z >= m_limits[1])
						{
							m_z = z;
						}
					}
				}
			}
		}
	}
}
