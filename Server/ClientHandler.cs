using Common.Communication;
using Common.Domain;
using Common.Domain.Izvestaji;
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
                    case Operation.GetAllKandidati:
                        response.Result = Controller.Instance.GetAllKandidati((bool)request.Argument);
                        break;
                    case Operation.GetAllPaketiObuke:
                        response.Result = Controller.Instance.GetAllPaketiObuke();
                        break;
                    case Operation.UpisiKandidata:
                        response.Result = Controller.Instance.UpisiKandidata((Upis)request.Argument);
                        break;
                    case Operation.ObrisiKandidata:
                        Controller.Instance.ObrisiKandidata((Kandidat)request.Argument);
                        break;
                    case Operation.KreirajInstruktora:
                        response.Result = Controller.Instance.KreirajInstruktora((Instruktor)request.Argument);
                        break;
                    case Operation.GetAllInstruktori:
                        response.Result = Controller.Instance.GetAllInstruktori();
                        break;
                    case Operation.ObrisiInstruktora:
                        Controller.Instance.ObrisiInstruktora((Instruktor)request.Argument);
                        break;
                    case Operation.GetAllVozila:
                        response.Result = Controller.Instance.GetAllVozila();
                        break;
                    case Operation.GetAllUpisi:
                        response.Result = Controller.Instance.GetAllUpisi();
                        break;
                    case Operation.ZakaziCasVoznje:
                        response.Result = Controller.Instance.ZakaziCasVoznje((CasVoznje)request.Argument);
                        break;
                    case Operation.GetAllCasVoznje:
                        response.Result = Controller.Instance.GetAllCasVoznje();
                        break;
                    case Operation.OtkaziCasVoznje:
                        response.Result = Controller.Instance.OtkaziCasVoznje((CasVoznje)request.Argument);
                        break;
                    case Operation.PretraziKandidate:
                        response.Result = Controller.Instance.PretraziKandidate((KandidatSearchFilter)request.Argument);
                        break;
                    case Operation.EvidentirajIspit:
                        response.Result = Controller.Instance.EvidentirajIspit((EvidentirajIspitRequest)request.Argument);
                        break;
                    case Operation.KreirajIzvestajProlaznosti:
                        response.Result = Controller.Instance.KreirajIzvestajProlaznosti((IzvestajProlaznostiKriterijum)request.Argument);
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