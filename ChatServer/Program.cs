using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
	class Program
	{
		static ServerObject server;
		static Thread listenThread;
		static void Main(string[] args)
		{
			try
			{
				server = new ServerObject();
				listenThread = new Thread(new ThreadStart(server.Listen));
				listenThread.Start();
			}
			catch(Exception e)
			{
				server.Disconnect();
				Console.WriteLine(e.Message);
			}
		}
	}
}
