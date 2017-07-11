using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ChatServer
{
	class ServerObject
	{
		static TcpListener tcpListener;
		List<ClientObject> clients = new List<ClientObject>();

		protected internal void AddConnection(ClientObject client)
		{
			clients.Add(client);
		}

		protected internal void RemoveConnection(string id)
		{
			ClientObject client = clients.FirstOrDefault(x => x.ID == id);
			if (client != null) clients.Remove(client);
		}

		protected internal void Listen()
		{
			try
			{
				tcpListener = new TcpListener(IPAddress.Any, 8888);
				tcpListener.Start();
				Console.WriteLine("Server is working. Waiting for connections...");
				while(true)
				{
					TcpClient tcpClient = tcpListener.AcceptTcpClient();
					ClientObject clientObject = new ClientObject(tcpClient, this);
					Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
					clientThread.Start();
				}
			}
			catch(Exception e)
            {
				Console.WriteLine(e.Message);
				Disconnect();
			}
			
		}

		protected internal void BroadcastMessage(string message, string id)
		{
			byte[] data = Encoding.Unicode.GetBytes(message);
			foreach (var client in clients)
			{
				if (client.ID != id)
					client.Stream.Write(data, 0, data.Length);
			}
		}

		protected internal void Disconnect()
		{
			tcpListener.Stop();
			foreach (var client in clients)
				client.Close();
			Environment.Exit(0);
        }
	}
}
