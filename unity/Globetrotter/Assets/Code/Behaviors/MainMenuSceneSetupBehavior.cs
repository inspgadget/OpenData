using UnityEngine;
using System.Collections;

using Globetrotter;
using Globetrotter.GuiLayer.Controllers;
using Globetrotter.InputLayer;

public class MainMenuSceneSetupBehavior : MonoBehaviour
{
	private object m_lockObj = new object();

	private IInputController m_inputController;

	private string m_sceneName;

	void OnGUI()
	{
	}

	void Start()
	{
		m_sceneName = null;
		
		//input controller
		m_inputController = new TcpInputController();
		
		if(m_inputController == null)
		{
			m_inputController = gameObject.AddComponent<KeyboardInputController>();
		}
		else
		{
			ObjectDepot.Instance.Store<IInputController>(m_inputController);

			((TcpInputController)m_inputController).ChangeScene += ChangeSceneHandler;

			m_inputController.StartController();
		}

		//connection setup behavior
		ConnectionSetupBehavior connectionSetupBehavior = gameObject.AddComponent<ConnectionSetupBehavior>();
		connectionSetupBehavior.Init(m_inputController is TcpInputController ? (TcpInputController)m_inputController : null);
	}
	
	void FixedUpdate()
	{
		lock(m_lockObj)
		{
			if(string.IsNullOrEmpty(m_sceneName) == false)
			{
				Application.LoadLevel(m_sceneName);
			}
		}
	}
	
	public void ChangeSceneHandler(object sender, ChangeSceneEventArgs args)
	{
		lock(m_lockObj)
		{
			if(m_inputController is TcpInputController)
			{
				((TcpInputController)m_inputController).ChangeScene -= ChangeSceneHandler;
			}

			m_sceneName = args.SceneName;
		}
	}
}
