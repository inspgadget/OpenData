using UnityEngine;
using System;
using System.Collections;

using GlobeTest;
using GlobeTest.InputLayer;

public class DataCubeCameraBehavior : MonoBehaviour
{
	private object m_lockObject = new object();
	
	public float moveAngle = 5.0f;
	
	private IInputController m_inputController;
	
	private Vector2D m_currPosVector;
	private double m_distance;
	
	void Start()
	{
		lock(m_lockObject)
		{
			m_currPosVector = new Vector2D(transform.position.x, transform.position.z);
			m_distance = m_currPosVector.AbsoluteValue;
		}
	}
	
	void Update()
	{
		lock(m_lockObject)
		{
			transform.position = new Vector3((float)m_currPosVector.X, 1.0f, (float)m_currPosVector.Y);
			transform.LookAt(new Vector3(0.0f, transform.position.y, 0.0f));
		}
	}
	
	public void Init(IInputController inputController)
	{
		m_inputController = inputController;
		
		m_inputController.InputReceived -= InputReceivedHandler;
		m_inputController.InputReceived += InputReceivedHandler;
	}
	
	private void MoveAndRotate(float angle)
	{
		lock(m_lockObject)
		{
			Vector2D newPosVector = new RotationMatrix2D((angle * Math.PI) / 180).Rotate(m_currPosVector);
			
			if(newPosVector.X > 0)
			{
				newPosVector.X = 0;
				newPosVector.Y = m_distance * (-1);
			}
			
			if(newPosVector.Y > 0)
			{
				newPosVector.X = m_distance * (-1);
				newPosVector.Y = 0;
			}
			
			m_currPosVector = newPosVector;
		}
	}
	
	public void InputReceivedHandler(object sender, InputReceivedEventArgs args)
	{
		if(args.HasDirection(InputDirection.Left) == true)
		{
			MoveAndRotate(moveAngle * (-1));
		}
		
		if(args.HasDirection(InputDirection.Right) == true)
		{
			MoveAndRotate(moveAngle);
		}
	}
}
