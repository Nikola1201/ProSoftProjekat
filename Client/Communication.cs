using Common.Communication;
using System;
using System.Net.Sockets;

namespace Client
{
    internal class Communication
    {
        private static Communication _instance;
        public static Communication Instance => _instance ?? (_instance = new Communication());
        private Communication()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private Socket _socket;
        private Sender _sender;
        private Sender _receiver;

        public void Connect()
        {
            try
            {
                _socket.Connect("127.0.0.1", 9999);
            }
            catch (Exception)
            {
                throw;
            }
            _sender = new Sender(_socket);
            _receiver = new Sender(_socket);
        }
    }
}
