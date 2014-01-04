using System;

namespace Globetrotter.GuiLayer.Controllers
{
	public abstract class GuiController
	{
		public delegate void ChangeSceneEventHandler(object sender, ChangeSceneEventArgs args);
		public event ChangeSceneEventHandler ChangeScene;

		public GuiController()
		{
		}

		protected void OnChangeScene(string sceneName)
		{
			if((ChangeScene != null) && (string.IsNullOrEmpty(sceneName) == false))
			{
				ChangeScene(this, new ChangeSceneEventArgs(sceneName));
			}
		}
	}
}
