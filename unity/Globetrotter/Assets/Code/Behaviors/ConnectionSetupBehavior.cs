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
	
	void OnGUI()
	{
		if(m_encoded != null)
		{
			GUI.Label(new Rect(Screen.width - 266, Screen.height - 266, m_encoded.width, m_encoded.height), m_encoded);
			GUI.Label(new Rect(Screen.width - 570, Screen.height - 25, 400, 20), "Scan the QR-Code to control the application with your smartphone.");
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
				
				/*for(int i = 0; i < bm.Height; i++){
					ZXing.Common.BitArray ba = bm.getRow(i, null);
					ba.reverse();

					for(int j = 0; j < ba.Size; j++){
						ba.flip(j);
					}

					bm.setRow(i, ba);
				}*/
				
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
