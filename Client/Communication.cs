using Common.Communication;
using Common.Domain;
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
        private Receiver _receiver;

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
            _receiver = new Receiver(_socket);
        }

        internal Response Login(Admin admin)
        {
            Request request = new Request()
            {
                Argument = admin,
                Operation = Operation.Login
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }
    }
}
