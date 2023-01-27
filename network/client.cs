// A C# program for Client
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Collections.Generic;

namespace Client
{

	public class Program
	{
		public static List<string> ipList = new List<string>();

		static void Main(string[] args)
		{
			
			for (int i = 1; i <= 255; i++)
			{
				string hostname = "192.168.0." + i;
				Ping p = new Ping();
				p.PingCompleted += new PingCompletedEventHandler(PingCompleted);
				p.SendAsync(hostname, 100, hostname);
			}
				
			ExecuteClient();
		}

		// ExecuteClient() Method
		static void ExecuteClient()
		{			
			try
			{
				foreach(string item in ipList){
					// Establish the remote endpoint
					// for the socket. This example
					// uses port 11111 on the local
					// computer.
					IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
					IPAddress ipAddr = IPAddress.Parse(item);
					IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

					// Creation TCP/IP Socket using
					// Socket Class Constructor
					Socket sender = new Socket(ipAddr.AddressFamily,
							SocketType.Stream, ProtocolType.Tcp);

					try
					{

						// Connect Socket to the remote
						// endpoint using method Connect()
						sender.Connect(localEndPoint);

						// We print EndPoint information
						// that we are connected
						Console.WriteLine("Socket connected to -> {0} ",
									sender.RemoteEndPoint.ToString());

						// Creation of message that
						// we will send to Server
						byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>");
						int byteSent = sender.Send(messageSent);

						// Data buffer
						byte[] messageReceived = new byte[1024];

						// We receive the message using
						// the method Receive(). This
						// method returns number of bytes
						// received, that we'll use to
						// convert them to string
						int byteRecv = sender.Receive(messageReceived);
						Console.WriteLine("Message from Server -> {0}",
							Encoding.ASCII.GetString(messageReceived,
														0, byteRecv));


						// Close Socket using
						// the method Close()
						sender.Shutdown(SocketShutdown.Both);
						sender.Close();
					}

					// Manage of Socket's Exceptions
					catch (ArgumentNullException ane)
					{

						Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
					}

					catch (SocketException se)
					{

						Console.WriteLine("SocketException : {0}", se.ToString());
					}

					catch (Exception e)
					{
						Console.WriteLine("Unexpected exception : {0}", e.ToString());
					}
				}
				
			}

			catch (Exception e)
			{

				Console.WriteLine(e.ToString());
			}
		}

		static void PingCompleted(object sender, PingCompletedEventArgs e)
		{
	
			string ip = (string)e.UserState;
			if (e.Reply != null && e.Reply.Status == IPStatus.Success)
			{
				Console.WriteLine(ip);
				ipList.Add(ip);
			}
		}
	}
}
