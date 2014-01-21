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
		private bool m_slow;

		public bool Slow {
			get {
				return m_slow;
			}
		}

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
						UnityEngine.Debug.Log(m_limits[0].ToString());
						float z = m_z + m_speed;
						
						if(z <= m_limits[0])
						{
							m_z = z;
							if( z >= -2.8f){
								m_slow = true;
							}
						}
					}
					
					if(args.HasInputType(InputType.ZoomOut) == true)
					{
						float z = m_z - m_speed;
						
						if(z >= m_limits[1])
						{
							m_z = z;

							if( z < -2.8f){
								m_slow = false;
							}
						}
					}
				}
			}
		}
	}
}
