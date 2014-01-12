using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Globetrotter.GuiLayer.Controllers;

namespace Globetrotter.InputLayer
{
	public class TcpInputController : GuiController, IInputController
	{
		public object m_lockObj = new object();

		public event InputReceivedEventHandler InputReceived;

		private IPEndPoint m_ipEndPoint;

		private Thread m_thread;

		public String IpAddress
		{
			get
			{
				lock(m_lockObj)
				{
					try
					{
						return m_ipEndPoint.Address.ToString();
					}
					catch(NullReferenceException)
					{
						return null;
					}
				}
			}
		}

		public int Port
		{
			get
			{
				lock(m_lockObj)
				{
					try
					{
						return m_ipEndPoint.Port;
					}
					catch(NullReferenceException)
					{
						return -1;
					}
				}
			}
		}

		public TcpInputController()
		{
			m_thread = null;
		}

		public void StartController ()
		{
			m_thread = new Thread(StartListening);
			m_thread.IsBackground = true;
			m_thread.Start();
		}

		public void StopController ()
		{
		}

		public ManualResetEvent allDone = new ManualResetEvent(false);

		public void StartListening() {
			// Data buffer for incoming data.
			byte[] bytes = new Byte[1024];
			
			// Establish the local endpoint for the socket.
			// The DNS name of the computer
			// running the listener is "host.contoso.com".
			IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			int port = 33000;

			/*while(!isAvailable(port)){
				port++;
			}*/

			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
			UdpClient u = new UdpClient(localEndPoint);

			lock(m_lockObj)
			{
				m_ipEndPoint = localEndPoint;
			}
			


			// Bind the socket to the local endpoint and listen for incoming connections.
			try {
					UdpState s = new UdpState();
					s.e = localEndPoint;
					s.u = u;

					u.BeginReceive(new AsyncCallback(ReadCallback), s);
			} catch (Exception e) {
				UnityEngine.Debug.LogError(e);
			}
			
			
		}

		int count = 0;
		DateTime _lastTime = DateTime.MinValue ;
		bool changed = false;

		public void ReadCallback(IAsyncResult ar) {
			if(!changed){
				OnChangeScene("GlobeScene");
				changed = true;
			}

			UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
			IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;
			
			Byte[] receiveBytes = u.EndReceive(ar, ref e);
			string content = Encoding.ASCII.GetString(receiveBytes);
				
			if (content.IndexOf("<EOF>") > -1) {
				// All the data has been read from the 
				// client. Display it on the console.
				//UnityEngine.Debug.Log("Read "+content.Length+" bytes from socket. "+content+" Data : {1}");

				content = content.Remove(content.Length - 5);

				InputType type = InputType.None;

				if(content.StartsWith("DoubleTap")){
					type = InputType.ClickDouble;
				} else if (content.StartsWith("SwipeLeft")){
//						type = InputType.WipeLeft;	//10er Schritte
					type = InputType.ScrollLeft;
				} else if (content.StartsWith("SwipeRight")){
//						type = InputType.WipeRight;	//10er Schritte
					type = InputType.ScrollRight;
				} else if (content.StartsWith("VolumeUp")){
					type = InputType.FocusPrevious;
				} else if (content.StartsWith("VolumeDown")){
					type = InputType.FocusNext;
				} else if (content.StartsWith("LongPress")){
					type = InputType.ClickLong;
				} else if (content.StartsWith("ZoomIn")){
					type = InputType.ZoomIn;
				} else if (content.StartsWith("ZoomOut")){
					type = InputType.ZoomOut;
				} else if (content.StartsWith("Acc")){

					string[] tmp = content.Split(';');
					float x = float.Parse(tmp[1]);
					float y = float.Parse(tmp[2]);
					float z = float.Parse(tmp[3]);

					if(x > 5f){
						type = InputType.RotateRight;
					} else if(x < -5f){
						type = InputType.RotateLeft;
					} else if(y > 1f){
						type = InputType.RotateDown;
					} else if(y < -1f){
						type = InputType.RotateUp;
					}
				}

				if(type != InputType.None){
					if(_lastTime == DateTime.MinValue){
						_lastTime = DateTime.Now;
					} else if ((DateTime.Now - _lastTime).TotalSeconds >= 10){
						UnityEngine.Debug.Log(count);
						_lastTime = DateTime.Now;
						count = 0;
					}
					OnInputReceived(this, type);
					//UnityEngine.Debug.Log(type.ToString());
					count++;
				}
			}
			Thread.Sleep(20);

			UdpState s = new UdpState();
			s.e = e;
			s.u = u;
			
			u.BeginReceive(new AsyncCallback(ReadCallback), s);
		}
		
		private void Send(Socket handler, String data) {
			// Convert the string data to byte data using ASCII encoding.
			byte[] byteData = Encoding.ASCII.GetBytes(data);
			
			// Begin sending the data to the remote device.
			handler.BeginSend(byteData, 0, byteData.Length, 0,
			                  new AsyncCallback(SendCallback), handler);
		}
		
		private void SendCallback(IAsyncResult ar) {
			try {
				// Retrieve the socket from the state object.
				Socket handler = (Socket) ar.AsyncState;
				
				// Complete sending the data to the remote device.
				int bytesSent = handler.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to client.", bytesSent);
				
				handler.Shutdown(SocketShutdown.Both);
				handler.Close();
				
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}

		protected void OnInputReceived(object sender, InputType inputTypes)
		{
			if(InputReceived != null)
			{
				InputReceived(this, new InputReceivedEventArgs(inputTypes));
			}
		}
		
		public class UdpState
			
		{
			
			
			
			public IPEndPoint e ;
			
			
			
			public UdpClient u ;
			
		}

		/*private bool isPortAvailable(int port);
		{
			bool isAvailable = true;
			
			// Evaluate current system tcp connections. This is the same information provided
			// by the netstat command line application, just in .Net strongly-typed object
			// form.  We will look through the list, and if our port we would like to use
			// in our TcpClient is occupied, we will set isAvailable to false.
			IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

			foreach(TcpConnectionInformation tcpi in tcpConnInfoArray)
			{
				if (tcpi.LocalEndPoint.Port==port)
				{
					isAvailable = false;
					break;
				}
			}

			return isAvailable;
		}*/
	}
}
