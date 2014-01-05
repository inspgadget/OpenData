using UnityEngine;
using System.Collections;

namespace Globetrotter.GuiLayer
{
	public class StyleDepot
	{
		private static object m_staticLockObject = new object();
		private object m_lockObject = new object();

		private static StyleDepot m_instance = null;

		private GUIStyle m_focusedBoxStyle;
		private GUIStyle m_unfocusedBoxStyle;

		private GUIStyle m_focusedTextStyle;
		private GUIStyle m_unfocusedTextStyle;

		private GUIStyle m_selectedCountryStyle;
		private GUIStyle m_selectedCountryHoverStyle;

		public static StyleDepot Instance
		{
			get
			{
				lock(m_staticLockObject)
				{
					m_instance = new StyleDepot();
				}

				return m_instance;
			}
		}

		public GUIStyle FocusedBoxStyle
		{
			get
			{
				lock(m_lockObject)
				{
					return m_focusedBoxStyle;
				}
			}
		}
		
		public GUIStyle FocusedTextStyle
		{
			get
			{
				lock(m_lockObject)
				{
					return m_focusedTextStyle;
				}
			}
		}
		
		public GUIStyle SelectedCountryHoverStyle
		{
			get
			{
				lock(m_lockObject)
				{
					return m_selectedCountryHoverStyle;
				}
			}
		}

		public GUIStyle SelectedCountryStyle
		{
			get
			{
				lock(m_lockObject)
				{
					return m_selectedCountryStyle;
				}
			}
		}

		public GUIStyle UnfocusedBoxStyle
		{
			get
			{
				lock(m_lockObject)
				{
					return m_unfocusedBoxStyle;
				}
			}
		}
		
		public GUIStyle UnfocusedTextStyle
		{
			get
			{
				lock(m_lockObject)
				{
					return m_unfocusedTextStyle;
				}
			}
		}

		private StyleDepot()
		{
			m_focusedBoxStyle = new GUIStyle();
			m_focusedBoxStyle.normal.background = Resources.Load<Texture2D>("box_focused");

			m_unfocusedBoxStyle = new GUIStyle();
			m_unfocusedBoxStyle.normal.background = Resources.Load<Texture2D>("box_unfocused");

			m_focusedTextStyle = new GUIStyle();
			m_focusedTextStyle.normal.textColor = Color.black;

			m_unfocusedTextStyle = new GUIStyle();
			m_unfocusedTextStyle.normal.textColor = Color.white;

			m_selectedCountryStyle = new GUIStyle();
			m_selectedCountryStyle.normal.textColor = Color.white;

			m_selectedCountryHoverStyle = new GUIStyle();
			m_selectedCountryHoverStyle.normal.textColor = Color.blue;
		}
	}
}
