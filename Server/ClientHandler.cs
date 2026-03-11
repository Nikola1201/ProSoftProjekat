using Common.Communication;
using Common.Domain;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Server
{
    public class ClientHandler
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

        internal void Close()
        {
            throw new NotImplementedException();
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
                        response.Result = Controller.Instance.Login((Admin)request.Argument);
                        break;
                    case Operation.KreirajKandidata:
                        response.Result = Controller.Instance.KreirajKandidata((Kandidat)request.Argument);
                        break;
                    case Operation.GetAllKategorije:
                        response.Result = Controller.Instance.GetAllKategorije();
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