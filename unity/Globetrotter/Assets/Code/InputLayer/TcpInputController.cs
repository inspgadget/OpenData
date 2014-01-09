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

			int port = 60123;

			/*while(!isAvailable(port)){
				port++;
			}*/

			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);


			lock(m_lockObj)
			{
				m_ipEndPoint = localEndPoint;
			}
			
			// Create a TCP/IP socket.
			Socket listener = new Socket(AddressFamily.InterNetwork,
			                             SocketType.Stream, ProtocolType.Tcp );
			
			// Bind the socket to the local endpoint and listen for incoming connections.
			try {
				lock(m_lockObj)
				{
					listener.Bind(localEndPoint);
				}

				listener.Listen(100);
				
				while (true) {
					// Set the event to nonsignaled state.
					allDone.Reset();
					
					// Start an asynchronous socket to listen for connections.
					//UnityEngine.Debug.Log("Waiting for a connection...");
					listener.BeginAccept( 
					                     new AsyncCallback(AcceptCallback),
					                     listener );
					
					// Wait until a connection is made before continuing.
					allDone.WaitOne();
				}
				
			} catch (Exception e) {
				UnityEngine.Debug.LogError(e);
			}
			
			
		}
		
		public void AcceptCallback(IAsyncResult ar) {
			// Signal the main thread to continue.
			allDone.Set();
			
			// Get the socket that handles the client request.
			Socket listener = (Socket) ar.AsyncState;
			Socket handler = listener.EndAccept(ar);

			OnChangeScene("GlobeScene");
			
			// Create the state object.
			StateObject state = new StateObject();
			state.workSocket = handler;
			handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
			                     new AsyncCallback(ReadCallback), state);
		}
		int count = 0;
		int c2 = 0;
		DateTime dt, onRecivedDt;

		public void ReadCallback(IAsyncResult ar) {
			String content = String.Empty;
			
			// Retrieve the state object and the handler socket
			// from the asynchronous state object.
			StateObject state = (StateObject) ar.AsyncState;
			Socket handler = state.workSocket;
			
			// Read data from the client socket. 
			int bytesRead = handler.EndReceive(ar);
			
			if (bytesRead > 0) {
				// There  might be more data, so store the data received so far.
				state.sb.Append(Encoding.ASCII.GetString(
					state.buffer,0,bytesRead));
				
				// Check for end-of-file tag. If it is not there, read 
				// more data.
				content = state.sb.ToString();
				if (content.IndexOf("<EOF>") > -1) {
					onRecivedDt = DateTime.Now;
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


						if(c2 == 0){
							dt = DateTime.Now;
						} else if (c2 % 50 == 0){
							TimeSpan ts = (DateTime.Now - dt);
							UnityEngine.Debug.Log("{0} "+ts.Minutes +" min - "+ts.Seconds+" sec : "+ c2);

							DateTime dt2 = DateTime.Parse(tmp[5]);

							UnityEngine.Debug.Log("Last Package took " + (onRecivedDt - dt2).TotalSeconds + "seconds over network");
						}
						if(x > 5f){
							type = InputType.RotateRight;
						} else if(x < -5f){
							type = InputType.RotateLeft;
						} else if(y > 1f){
							type = InputType.RotateDown;
						} else if(y < -1f){
							type = InputType.RotateUp;
						}
						c2++;
					}

					if(type != InputType.None){
					UnityEngine.Debug.Log(type.ToString());
					}

					if(count % 5 == 0){
						if(type != InputType.None){
							OnInputReceived(this, type);
							count = 0;
						}
					} else { count++; }
				} else {
					// Not all data received. Get more.
					handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
					                     new AsyncCallback(ReadCallback), state);
				}
			}
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
		
		// State object for reading client data asynchronously
		public class StateObject {
			// Client  socket.
			public Socket workSocket = null;
			// Size of receive buffer.
			public const int BufferSize = 1024;
			// Receive buffer.
			public byte[] buffer = new byte[BufferSize];
			// Received data string.
			public StringBuilder sb = new StringBuilder();  
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
