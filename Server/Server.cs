using Common.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        Socket socket;
        public static List<ClientHandler> clients = new List<ClientHandler>();
        public static List<Admin> loggedIn = new List<Admin>();
        public Server()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ip"]), int.Parse(ConfigurationManager.AppSettings["port"]));
            socket.Bind(endPoint);
            socket.Listen(5);

            Thread thread = new Thread(AcceptClient);
            thread.Start();
        }

        private void AcceptClient()
        {
            try
            {
                while (true)
                {
                    Socket clientSocket = socket.Accept();
                    ClientHandler handler = new ClientHandler(clientSocket);
                    lock (clients) { clients.Add(handler); }
                    Thread clientThread = new Thread(handler.Handle);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public void Stop()
        {
            ClientHandler[] snapshot;
            lock (clients) { snapshot = clients.ToArray(); }
            foreach (ClientHandler handler in snapshot)
            {
                handler.Close();
            }
            lock (loggedIn) { loggedIn.Clear(); }
            lock (clients) { clients.Clear(); }
            socket.Close();
        }
    }
}
