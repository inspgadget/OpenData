using UnityEngine;
using System.Collections;

using Globetrotter.DomainLayer;
using Globetrotter.GuiLayer;
using Globetrotter.GuiLayer.ViewModel;
using System;
using System.IO;

public class SelectedCountriesBehavior : MonoBehaviour
{
	private SelectedCountriesViewModel m_selectedCountriesViewModel;
	private DateTime m_firstRunDt = DateTime.MinValue;

	void OnGUI()
	{
		lock(m_selectedCountriesViewModel.LockObject)
		{
			Country[] selectedCountries = m_selectedCountriesViewModel.SelectedCountries;

			//left, top, width, height
			int screenWidth = Screen.width;
			int screenHeight = Screen.height;
			
			//box
			GUIStyle style = StyleDepot.Instance.UnfocusedBoxStyle;
			
			if(m_selectedCountriesViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedBoxStyle;
			}

			GUI.Box(new Rect(screenWidth - 220, (screenHeight / 2) + 5, 210, (screenHeight / 2) - 10), string.Empty, style);

			//labels
			style = StyleDepot.Instance.UnfocusedTextStyle;
			
			if(m_selectedCountriesViewModel.ReactOnInput == true)
			{
				style = StyleDepot.Instance.FocusedTextStyle;
				DateTime now = DateTime.Now;
				if(m_firstRunDt == DateTime.MinValue){
					m_firstRunDt = now;
				}
				TimeSpan ts = (now - m_firstRunDt);
				if(ts.TotalSeconds <= 6){
					Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/doubletap2.png");
					GUI.DrawTexture( new Rect( 0, 
					                          0,
					                          texture.width,
					                          texture.height ), 
					                texture );
				}  else if (ts.TotalSeconds <= 12){
					Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/swipedownupselectedcountries.png");
					GUI.DrawTexture( new Rect( 0, 
					                          0,
					                          texture.width,
					                          texture.height ), 
					                texture );
				}
			}

			if((selectedCountries != null) && (selectedCountries.Length > 0))
			{
				int top = (screenHeight / 2) + 15;

				for(int i = 0; i < selectedCountries.Length; i++)
				{
					GUIStyle s = style;

					if((i == m_selectedCountriesViewModel.CurrentCountryIndex) && (m_selectedCountriesViewModel.ReactOnInput == true))
					{
						s = StyleDepot.Instance.SelectedCountryHoverStyle;
					}

					GUI.Label(new Rect( screenWidth - 210, top, 200, 25), selectedCountries[i].Name, s);

					top = top + 30;
				}
			}
			else
			{
				GUI.Label(new Rect( screenWidth - 210, (screenHeight / 2) + 15, 200, 25), "No countries selected.", style);
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

	public void Init(SelectedCountriesViewModel selectedCountriesViewModel)
	{
		m_selectedCountriesViewModel = selectedCountriesViewModel;
	}
}
