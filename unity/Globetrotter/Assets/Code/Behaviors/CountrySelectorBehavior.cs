using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

using Globetrotter.DomainLayer;
using Globetrotter.GuiLayer;
using Globetrotter.GuiLayer.ViewModel;
using Globetrotter.PersistenceLayer;
using System.IO;

public class CountrySelectorBehavior : MonoBehaviour
{
	private CountrySelectorViewModel m_countrySelectorViewModel;
	private DateTime m_firstRunDt = DateTime.MinValue;

	void OnGUI()
	{
		lock(m_countrySelectorViewModel.LockObject)
		{
			IFlagLoader flagLoader = new FlagResourcesLoader();
			Country currCountry = m_countrySelectorViewModel.CurrentCountry;

			if(currCountry != null)
			{
				//left, top, width, height
				int screenWidth = Screen.width;
				int screenHeight = Screen.height;
				int boxCenter = screenWidth - 110;

				//boxes
				GUIStyle style = StyleDepot.Instance.UnfocusedBoxStyle;
				
				if(m_countrySelectorViewModel.ReactOnInput == true)
				{
					style = StyleDepot.Instance.FocusedBoxStyle;
				}

				GUI.Box(new Rect(screenWidth - 220, 60, 210, (screenHeight / 2) - 65), string.Empty, style);
				GUI.Box(new Rect(boxCenter - 37, 10, 74, 50), string.Empty, style);

				//flags
				Texture prevCountryTexture = flagLoader.LoadFlag(m_countrySelectorViewModel.PreviousCountry.IsoAlphaThreeCode);

				if(prevCountryTexture == null)
				{
					prevCountryTexture = Resources.Load<Texture2D>("no_image");
				}

				Texture currCountryTexture = flagLoader.LoadFlag(currCountry.IsoAlphaThreeCode);

				if(currCountryTexture == null)
				{
					currCountryTexture = Resources.Load<Texture2D>("no_image");
				}

				Texture nextCountryTexture = flagLoader.LoadFlag(m_countrySelectorViewModel.NextCountry.IsoAlphaThreeCode);

				if(nextCountryTexture == null)
				{
					nextCountryTexture = Resources.Load<Texture2D>("no_image");
				}

				GUI.Label(new Rect(boxCenter - 106, 20, 64, 32), prevCountryTexture);
				GUI.Label(new Rect(boxCenter - 32, 20, 64, 32), currCountryTexture);
				GUI.Label(new Rect(boxCenter + 42, 20, 64, 32), nextCountryTexture);

				//country info
				style = StyleDepot.Instance.UnfocusedTextStyle;

				if(m_countrySelectorViewModel.ReactOnInput == true)
				{
					style = StyleDepot.Instance.FocusedTextStyle;
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
						Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/swipedownup.png");
						GUI.DrawTexture( new Rect( 0, 
						                          0,
						                          texture.width,
						                          texture.height ), 
						                texture );
					} else if (ts.TotalSeconds <= 18){
						Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/leftright.png");
						GUI.DrawTexture( new Rect( 0, 
						                          0,
						                          texture.width,
						                          texture.height ), 
						                texture );
					} else if (ts.TotalSeconds <= 24){
						Texture2D texture = loadTexture(Application.dataPath + "/Images/Resources/longpress.png");
						GUI.DrawTexture( new Rect( 0, 
						                          0,
						                          texture.width,
						                          texture.height ), 
						                texture );
					}
				}

				StringBuilder sb = new StringBuilder();

				sb.Append(currCountry.Name).Append(" - ").Append(currCountry.IsoAlphaThreeCode).Append("\n\n");
				sb.Append("Capital city: ").Append(currCountry.CapitalCity).Append("\n");
				sb.Append("Population: ").Append(string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("de-AT"),
				                                               	"{0:0,0}",
				                                               	currCountry.Population)).Append("\n");
				sb.Append("Surface Area: ").Append(string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("de-AT"),
				                                                 	"{0:#,##0.00}",
				                                                 	currCountry.SurfaceArea)).Append(" km\xb2");
				
				GUI.Label(new Rect(Screen.width - 210, 90, 200, 180), sb.ToString(), style);
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

	public void Init(CountrySelectorViewModel countrySelectorViewModel)
	{
		m_countrySelectorViewModel = countrySelectorViewModel;
	}
}
