using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Globetrotter.NetworkLayer
{
	public class TcpRequestController
	{
		public TcpRequestController()
		{
		}

		public string GetRepsonseData(string url, int port)
		{
			string host = GetHost(url);
			string path = GetPath(url);

			string request = "GET " + path + " HTTP/1.1\nHost: " + host + "\r\n\r\n";

			using(TcpClient socket = new TcpClient(host, port))
			{
				using(BufferedStream bs = new BufferedStream(socket.GetStream()))
				{
					//request
					using(StreamWriter sw = new StreamWriter(new BufferedStream(socket.GetStream())))
					{
						sw.Write(request);
						sw.Flush();
						
						//response
						using(StreamReader sr = new StreamReader(new BufferedStream(socket.GetStream())))
						{
							if(sr.EndOfStream == false)
							{
								string line = sr.ReadLine();
								
								if(line.Contains("200"))
								{
									line = null;
									
									//read over header
									while((sr.EndOfStream == false) && ((line = sr.ReadLine()) != ""))
									{
									}
									
									//read content
									StringBuilder sb = new StringBuilder();
									line = null;
									
									while((sr.EndOfStream == false) && ((line = sr.ReadLine()) != null))
									{
										sb.Append(line);
									}

									return sb.ToString();
								}
							}
						}
					}
				}
			}

			return null;
		}
		
		private string GetHost(string url)
		{
			string withoutProtocol = url.Substring(7);
			int hostEnd = withoutProtocol.IndexOf("/");
			
			return withoutProtocol.Substring(0, hostEnd);
		}
		
		private string GetPath(string url)
		{
			string withoutProtocol = url.Substring(7);
			int hostEnd = withoutProtocol.IndexOf("/");
			
			return withoutProtocol.Substring(hostEnd);
		}
	}
}
