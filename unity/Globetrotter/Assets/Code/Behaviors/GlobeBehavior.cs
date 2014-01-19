using UnityEngine;
using System.Collections;

using Globetrotter.GuiLayer.ViewModel;
using System;
using System.IO;

public class GlobeBehavior : MonoBehaviour
{
	private GlobeViewModel m_globeViewModel;
	private DateTime m_firstRunDt = DateTime.MinValue;

	void Update()
	{
		lock(m_globeViewModel.LockObject)
		{
			float horizontalAngle = m_globeViewModel.HorizontalAngle;
			m_globeViewModel.HorizontalAngle = 0.0f;

			float verticalAngle = m_globeViewModel.VerticalAngle;
			m_globeViewModel.VerticalAngle = 0.0f;

			transform.Rotate(verticalAngle, horizontalAngle, 0.0f, Space.World);
		}
	}

	void OnGUI()
	{
		if(m_globeViewModel.ReactOnInput == true)
		{
			DateTime now = DateTime.Now;
			if(m_firstRunDt == DateTime.MinValue){
				m_firstRunDt = now;
			}
			TimeSpan ts = (now - m_firstRunDt);
			if(ts.TotalSeconds <= 6){
				Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/doubletap.png");
				GUI.DrawTexture( new Rect( 0, 
				                          0,
				                          texture.width,
				                          texture.height ), 
				                texture );
			} else if (ts.TotalSeconds <= 12){
				Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/longpress.png");
				GUI.DrawTexture( new Rect( 0, 
				                          0,
				                          texture.width,
				                          texture.height ), 
				                texture );
			} else if (ts.TotalSeconds <= 18){
				//Zoom
			} else if (ts.TotalSeconds <= 24){
				//Sensor
			}
		}
	}

	private Texture2D loadTexture(string path){
		Texture2D texture = new Texture2D(256,200);
		FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
		byte[] imageData = new byte[fs.Length];
		fs.Read(imageData, 0, (int) fs.Length);
		texture.LoadImage(imageData);
		return texture;
	}

	public void Init(GlobeViewModel globeViewModel)
	{
		m_globeViewModel = globeViewModel;
	}
}
