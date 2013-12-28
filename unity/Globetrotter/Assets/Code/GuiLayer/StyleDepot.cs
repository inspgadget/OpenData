using UnityEngine;
using System.Collections;

namespace Globetrotter.GuiLayer
{
	public class StyleDepot
	{
		private static object m_staticLockObject = new object();

		private static StyleDepot m_instance = null;

		private GUIStyle m_focusedBoxStyle;
		private GUIStyle m_unfocusedBoxStyle;

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
				return m_focusedBoxStyle;
			}
		}

		public GUIStyle UnFocusedBoxStyle
		{
			get
			{
				return m_unfocusedBoxStyle;
			}
		}

		private StyleDepot()
		{
			m_focusedBoxStyle = new GUIStyle();

			m_unfocusedBoxStyle = new GUIStyle();
		}
	}
}
