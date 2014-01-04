using System;

namespace Globetrotter.GuiLayer.Controllers
{
	public class ChangeSceneEventArgs : EventArgs
	{
		private string m_sceneName;

		public string SceneName
		{
			get
			{
				return m_sceneName;
			}
		}

		public ChangeSceneEventArgs(string sceneName)
		{
			m_sceneName = sceneName;
		}
	}
}
