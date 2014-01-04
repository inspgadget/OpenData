using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

using Globetrotter.DomainLayer;
using Globetrotter.GuiLayer;
using Globetrotter.GuiLayer.ViewModel;
using Globetrotter.PersistenceLayer;

public class CountrySelectorBehavior : MonoBehaviour
{
	private CountrySelectorViewModel m_countrySelectorViewModel;

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

				GUIStyle style = StyleDepot.Instance.UnfocusedBoxStyle;

				if(m_countrySelectorViewModel.ReactOnInput == true)
				{
					style = StyleDepot.Instance.FocusedBoxStyle;
				}

				//boxes
				GUI.Box(new Rect(screenWidth - 220, 10, 210, (screenHeight / 2) - 15), string.Empty, style);
				GUI.Box(new Rect(boxCenter - 37, 15, 74, 74), string.Empty);

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
				StringBuilder sb = new StringBuilder();

				sb.Append(currCountry.Name).Append(" - ").Append(currCountry.IsoAlphaThreeCode).Append("\n\n");
				sb.Append("Capital city: ").Append(currCountry.CapitalCity).Append("\n");
				sb.Append("Population: ").Append(currCountry.Population).Append("\n");
				sb.Append("Surface Area: ").Append(currCountry.SurfaceArea).Append("sq. km");
				
				GUI.Label(new Rect(Screen.width - 210, 90, 200, 180), sb.ToString(), style);
			}
		}
	}

	public void Init(CountrySelectorViewModel countrySelectorViewModel)
	{
		m_countrySelectorViewModel = countrySelectorViewModel;
	}
}
