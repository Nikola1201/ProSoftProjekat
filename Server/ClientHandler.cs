using Common.Communication;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Server
{
    internal class ClientHandler
    {
        private Socket _clientSocket;
        private Receiver _receiver;
        private Sender _sender;

        public ClientHandler(Socket clientSocket)
        {
            _clientSocket = clientSocket;
            _sender = new Sender(clientSocket);
            _receiver = new Receiver(clientSocket);
        }

        internal void Handle()
        {
            try
            {
                while (true)
                {
                    Request request = (Request)_receiver.Receive();
                    Response response = ProcessRequest(request);
                    _sender.Send(response);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private Response ProcessRequest(Request request)
        {
            Response response = new Response();
            try
            {
                switch (request.Operation)
                {
                    case Operation.Login:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
    }
}