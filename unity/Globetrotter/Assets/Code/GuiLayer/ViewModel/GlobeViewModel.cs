using System;
using System.Collections.Generic;

using Globetrotter.InputLayer;

namespace Globetrotter.GuiLayer.ViewModel
{
	public class GlobeViewModel : ViewModelBase
	{
		private float m_horizontalAngle;
		private float m_verticalAngle;

		private float m_speed;

		public float HorizontalAngle
		{
			get
			{
				lock(m_lockObj)
				{
					return m_horizontalAngle;
				}
			}

			set
			{
				lock(m_lockObj)
				{
					m_horizontalAngle = value;
				}
			}
		}
		
		public float Speed
		{
			get
			{
				lock(m_lockObj)
				{
					return m_speed;
				}
			}
			
			set
			{
				lock(m_lockObj)
				{
					m_speed = value;
				}
			}
		}
		
		public float VerticalAngle
		{
			get
			{
				lock(m_lockObj)
				{
					return m_verticalAngle;
				}
			}
			
			set
			{
				lock(m_lockObj)
				{
					m_verticalAngle = value;
				}
			}
		}

		public GlobeViewModel(float speed)
			: base()
		{
			m_horizontalAngle = 0.0f;
			m_verticalAngle = 0.0f;

			m_speed = speed;
			m_reactOnInput = false;
		}
		
		public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
		{
			lock(m_lockObj)
			{
				if(ReactOnInput == true)
				{
					CameraZoomViewModel czvm = ObjectDepot.Instance.Retrive<CameraZoomViewModel>();
					float speed = czvm.Slow ? m_speed / 5.0f : m_speed;
					UnityEngine.Debug.Log("-----");
					UnityEngine.Debug.Log(czvm.Slow.ToString());
					UnityEngine.Debug.Log(speed.ToString());
					UnityEngine.Debug.Log("-----");
					if(args.HasInputType(InputType.ClickDouble) == true)
					{
						CountrySelectorViewModel cc = ObjectDepot.Instance.Retrive<CountrySelectorViewModel>();
						cc.AddCountry();
					}

					if(args.HasInputType(InputType.RotateUp) == true)
					{
						VerticalAngle = VerticalAngle - speed;
					}
					
					if(args.HasInputType(InputType.RotateDown) == true)
					{
						VerticalAngle = VerticalAngle + speed;
					}
					
					if(args.HasInputType(InputType.RotateLeft) == true)
					{
						HorizontalAngle = HorizontalAngle + speed;
					}
					
					if(args.HasInputType(InputType.RotateRight) == true)
					{
						HorizontalAngle = HorizontalAngle - speed;
					}
				}
			}
		}
	}
}
