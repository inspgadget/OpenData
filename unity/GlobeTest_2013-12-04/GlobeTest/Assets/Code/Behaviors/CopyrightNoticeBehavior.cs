using UnityEngine;
using System.Collections;

public class CopyrightNoticeBehavior : MonoBehaviour
{
	void OnGUI()
	{
		int left = 10;
		int height = 50;
		int top = Screen.height - height;
		int width = Screen.width - left;
		
		string text = "World map provided by the NASA Blue Marble project\n" +
						"Continental maps provided by www.ginkgomaps.com\n" +
						"Data provided by the World Bank";
		
		GUI.Label(new Rect(left, top, width, height), text);
	}
}
