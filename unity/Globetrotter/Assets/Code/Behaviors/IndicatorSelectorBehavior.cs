using UnityEngine;
using System.Collections;

using Globetrotter.DataLayer;
using Globetrotter.GuiLayer;
using Globetrotter.GuiLayer.ViewModel;
using System;
using System.IO;

public class IndicatorSelectorBehavior : MonoBehaviour
{
	private IndicatorSelectorViewModel m_indicatorSelectorViewModel;
	private DateTime m_firstRunDt = DateTime.MinValue;

	void OnGUI()
	{
		lock(m_indicatorSelectorViewModel.LockObject)
		{
			//left, top, width, height
			int screenWidth = Screen.width;
			int screenWidthHalf = screenWidth / 2;

			//boxes
			GUIStyle style = StyleDepot.Instance.UnfocusedBoxStyle;

			if(m_indicatorSelectorViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedBoxStyle;

				DateTime now = DateTime.Now;
				if(m_firstRunDt == DateTime.MinValue){
					m_firstRunDt = now;
				}
				TimeSpan ts = (now - m_firstRunDt);
				if(ts.TotalSeconds <= 6){
					Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/swipe_d_ind.png");
					GUI.DrawTexture( new Rect( 0, 
					                          Screen.height - texture.height,
					                          texture.width,
					                          texture.height ), 
					                texture );
				} else if (ts.TotalSeconds <= 12){
					Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/doubletap_d.png");
					GUI.DrawTexture( new Rect( 0, 
					                          Screen.height - texture.height,
					                          texture.width,
					                          texture.height ), 
					                texture );
				} else if (ts.TotalSeconds <= 18){
					Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/longpress.png");
					GUI.DrawTexture( new Rect( 0, 
					                          Screen.height - texture.height,
					                          texture.width,
					                          texture.height ), 
					                texture );
				} else if (ts.TotalSeconds <= 24){
					Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/swipedownup.png");
					GUI.DrawTexture( new Rect( 0, 
					                          Screen.height - texture.height,
					                          texture.width,
					                          texture.height ), 
					                texture );
				}
			}

			GUI.Box(new Rect(screenWidthHalf - 100, 10, 200, 50), string.Empty, style);

			//labels
			GUI.Label(new Rect(screenWidthHalf - 420, 10, 200, 50), m_indicatorSelectorViewModel.PreviousIndicator.Name);

			/*style = StyleDepot.Instance.UnfocusedTextStyle;

			if(m_indicatorSelectorViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedTextStyle;
			}*/

			GUI.Label(new Rect(screenWidthHalf - 100, 10, 200, 50), m_indicatorSelectorViewModel.CurrentIndicator.Name/*, style*/);

			GUI.Label(new Rect(screenWidthHalf + 120, 10, 200, 50), m_indicatorSelectorViewModel.NextIndicator.Name);
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

	public void Init(IndicatorSelectorViewModel indicatorSelectorViewModel)
	{
		m_indicatorSelectorViewModel = indicatorSelectorViewModel;
	}
}
