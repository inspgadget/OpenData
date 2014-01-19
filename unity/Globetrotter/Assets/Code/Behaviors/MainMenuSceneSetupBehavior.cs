using UnityEngine;
using System.Collections;

using Globetrotter;
using Globetrotter.GuiLayer.Controllers;
using Globetrotter.InputLayer;

public class MainMenuSceneSetupBehavior : MonoBehaviour
{
	private object m_lockObj = new object();

	public GUIText loadingText;

	private IInputController m_inputController;
	private IInputController m_keyboardInputController;

	private string m_sceneName;

	void OnGUI()
	{
	}

	void Start()
	{
		loadingText.enabled = false;

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

		m_keyboardInputController = gameObject.AddComponent<KeyboardInputController>();
		m_keyboardInputController.InputReceived += InputReveivedHandler;

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
				loadingText.enabled = true;

				m_keyboardInputController.InputReceived -= InputReveivedHandler;

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

	public void InputReveivedHandler(object sender, InputReceivedEventArgs args)
	{
		if(sender == m_keyboardInputController)
		{
			if(args.HasInputType(InputType.ClickLong) == true)
			{
				lock(m_lockObj)
				{
					m_sceneName = "GlobeScene";
				}
			}
		}
	}
}
