using UnityEngine;
using System.Collections;

using ZXing;
using ZXing.Common;
using ZXing.QrCode;

using Globetrotter.GuiLayer.Controllers;
using Globetrotter.InputLayer;

public class ConnectionSetupBehavior : MonoBehaviour
{
	private TcpInputController m_inputController;

	private Texture2D m_encoded;
	private GUIStyle m_textStyle;

	void Start()
	{
		m_textStyle = new GUIStyle();
		m_textStyle.alignment = TextAnchor.MiddleCenter;
		m_textStyle.normal.textColor = Color.white;
	}
	
	void OnGUI()
	{
		if(m_encoded != null)
		{
			int screenWidth = Screen.width;
			int screenHeight = Screen.height;

			GUI.Label(new Rect((screenWidth / 2) - (m_encoded.width / 2),
			                   		(screenHeight / 2) - (m_encoded.height / 2),
			                   		m_encoded.width,
			                   		m_encoded.height),
			          		m_encoded);

			GUI.Label(new Rect(0, (screenHeight / 2) + (m_encoded.height / 2) + 10, screenWidth, 20),
			          	"Scan the QR-Code to control the application with your smartphone.",
			          	m_textStyle);
		}
	}

	void Update()
	{
		if((m_encoded == null) && (m_inputController != null))
		{
			string ipAddress = m_inputController.IpAddress;
			int port = m_inputController.Port;

			if((string.IsNullOrEmpty(ipAddress) == false) && (port > -1))
			{
				m_encoded = new Texture2D(256, 256);
				QRCodeWriter writer = new QRCodeWriter();
				
				BitMatrix bm = writer.encode(ipAddress + ";" + port, BarcodeFormat.QR_CODE, 256, 256);
				BarcodeWriter brwriter = new BarcodeWriter();
				Color32[] col = brwriter.Write(bm);
				
				m_encoded.SetPixels32(col);
				m_encoded.Apply();
			}
		}
	}

	public void Init(TcpInputController inputController)
	{
		m_inputController = inputController;

		m_encoded = null;
	}
}
