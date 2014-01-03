using System;
using System.IO;
using System.Net.Sockets;

namespace Globetrotter.NetworkLayer
{
	public class HttpRequestController
	{
		public HttpRequestController()
		{
		}

		public HttpResponse GetResponse(string url, int port)
		{
			string host = GetHost(url);
			string path = GetPath(url);

			string request = "GET " + path + " HTTP/1.1\r\nHost: " + host + "\r\n\r\n";

			using(TcpClient socket = new TcpClient(host, port))
			{
				using(BufferedStream bs = new BufferedStream(socket.GetStream()))
				{
					//request
					using(StreamWriter sw = new StreamWriter(bs))
					{
						sw.Write(request);
						sw.Flush();

						//response
						using(StreamReader sr = new StreamReader(bs, System.Text.Encoding.ASCII))
						{
							HttpResponse response = new HttpResponse();

							if(sr.EndOfStream == false)
							{
								string line = sr.ReadLine();

								if(line.Contains("200"))
								{
									line = null;

									//read header
									int contentLength = -1;

									while((sr.EndOfStream == false) && ((line = sr.ReadLine()) != ""))
									{
										string[] header = line.Trim().Split(':');

										if(header.Length == 2)
										{
											string key = header[0].Trim();
											string value = header[1].Trim();

											response.SetHeaderValue(key, value);

											if(key == "Content-Length")
											{
												contentLength = int.Parse(value);
											}
										}
									}

									//read content
									if(contentLength > -1)
									{
										byte[] data = new byte[contentLength];

										/*for(int i = 0; i < contentLength; i++)
										{
											data[i] = (byte)bs.ReadByte();
										}*/

										int nBytesReceived = 0;
										int nBytesToRead = contentLength;

										while(nBytesToRead > 0)
										{
											int n = bs.Read(data, nBytesReceived, contentLength - nBytesReceived);

											if(n == 0)
											{
												break;
											}

											nBytesReceived = nBytesReceived + n;
											nBytesToRead = nBytesToRead - n;
										}

										response.Data = data;
									}
									else
									{
										UnityEngine.Debug.LogWarning("Content-Length is not available. Data will not be read.");
									}

									return response;
								}
								else
								{
									UnityEngine.Debug.LogError(line);
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
