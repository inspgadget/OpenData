using UnityEngine;
using System.Collections;

using GlobeTest.InputLayer;

public class GlobeBehavior : MonoBehaviour
{
	private object m_lockObj = new object();
	
	public bool reactOnKeyboard = true;
	
	public KeyCode rotateUp = KeyCode.W;
	public KeyCode rotateDown = KeyCode.S;
	public KeyCode rotateLeft = KeyCode.A;
	public KeyCode rotateRight = KeyCode.D;
	
	public float rotateFactor = 0.5f;
	
	private IInputController m_inputController;
	
	private float m_rotateX;
	private float m_rotateY;
	
	private Quaternion m_startQuaternation;
	
	void Start()
	{
		m_rotateX = 0.0f;
		m_rotateY = 0.0f;
		
		m_startQuaternation = transform.rotation;
	}
	
	void FixedUpdate()
	{
		if(reactOnKeyboard == true)
		{
			float rotateVertical = 0.0f;
			float rotateHorizontal = 0.0f;
			
			if(Input.GetKey(rotateUp) == true)
			{
				rotateHorizontal = rotateHorizontal + rotateFactor;
			}
			
			if(Input.GetKey(rotateDown) == true)
			{
				rotateHorizontal = rotateHorizontal - rotateFactor;
			}
			
			if(Input.GetKey(rotateLeft) == true)
			{
				rotateVertical = rotateVertical - rotateFactor;
			}
			
			if(Input.GetKey(rotateRight) == true)
			{
				rotateVertical = rotateVertical + rotateFactor;
			}
			
			transform.Rotate(rotateHorizontal, rotateVertical, 0.0f, Space.World);
		}
		
		lock(m_lockObj)
		{
			if(m_rotateX != 0.0f)
			{
				transform.Rotate(m_rotateX, 0.0f, 0.0f, Space.World);
				m_rotateX = 0.0f;
			}
			
			if(m_rotateY != 0.0f)
			{
				transform.Rotate(0.0f, m_rotateY, 0.0f, Space.World);
				m_rotateY = 0.0f;
			}
		}
	}
	
	public void Init(IInputController inputController)
	{
		m_inputController = inputController;
		
		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;
	}
	
	public void RotateTo(float x, float y, float z)
	{
		//rotate to 0, 0, 0
		transform.rotation = m_startQuaternation;
		transform.Rotate(-270.0f, 0, 0, Space.Self);
		
		//rotate to position
		transform.Rotate(x, y, z, Space.Self);
	}
	
	public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		lock(m_lockObj)
		{
			if(args.HasDirection(InputDirection.Up) == true)
			{
				m_rotateX = m_rotateX - rotateFactor;
			}
			
			if(args.HasDirection(InputDirection.Down) == true)
			{
				m_rotateX = m_rotateX + rotateFactor;
			}
			
			if(args.HasDirection(InputDirection.Left) == true)
			{
				m_rotateY = m_rotateY - rotateFactor;
			}
			
			if(args.HasDirection(InputDirection.Right) == true)
			{
				m_rotateY = m_rotateY + rotateFactor;
			}
		}
	}
}
