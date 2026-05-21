using Common.Communication;
using Common.Domain;
using Common.Domain.Izvestaji;
using Common.Validation;
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
           _clientSocket?.Close();
        }

        internal void Handle()
        {
            try
            {
                while (true)
                {
                    Request request = _receiver.Receive<Request>();
                    Response response = ProcessRequest(request);
                    _sender.Send(response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Server.clients.Remove(this);
                _clientSocket?.Close();
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
                        response.Result = Controller.Instance.Login(_receiver.ReadType<Admin>(request.Argument!));
                        break;
                    case Operation.Logout:
                        Logout(_receiver.ReadType<Admin>(request.Argument!));
                        break;
                    case Operation.KreirajKandidata:
                        response.Result = Controller.Instance.KreirajKandidata(_receiver.ReadType<Kandidat>(request.Argument!));
                        break;
                    case Operation.GetAllKategorije:
                        response.Result = Controller.Instance.GetAllKategorije();
                        break;
                    case Operation.GetAllKandidati:
                        response.Result = Controller.Instance.GetAllKandidati(_receiver.ReadType<bool>(request.Argument!));
                        break;
                    case Operation.GetAllPaketiObuke:
                        response.Result = Controller.Instance.GetAllPaketiObuke();
                        break;
                    case Operation.UpisiKandidata:
                        response.Result = Controller.Instance.UpisiKandidata(_receiver.ReadType<Upis>(request.Argument!));
                        break;
                    case Operation.ObrisiKandidata:
                        Controller.Instance.ObrisiKandidata(_receiver.ReadType<Kandidat>(request.Argument!));
                        break;
                    case Operation.KreirajInstruktora:
                        response.Result = Controller.Instance.KreirajInstruktora(_receiver.ReadType<Instruktor>(request.Argument!));
                        break;
                    case Operation.GetAllInstruktori:
                        response.Result = Controller.Instance.GetAllInstruktori();
                        break;
                    case Operation.ObrisiInstruktora:
                        Controller.Instance.ObrisiInstruktora(_receiver.ReadType<Instruktor>(request.Argument!));
                        break;
                    case Operation.GetAllVozila:
                        response.Result = Controller.Instance.GetAllVozila();
                        break;
                    case Operation.GetAllUpisi:
                        response.Result = Controller.Instance.GetAllUpisi();
                        break;
                    case Operation.ZakaziCasVoznje:
                        response.Result = Controller.Instance.ZakaziCasVoznje(_receiver.ReadType<CasVoznje>(request.Argument!));
                        break;
                    case Operation.GetAllCasVoznje:
                        response.Result = Controller.Instance.GetAllCasVoznje();
                        break;
                    case Operation.OtkaziCasVoznje:
                        response.Result = Controller.Instance.OtkaziCasVoznje(_receiver.ReadType<CasVoznje>(request.Argument!));
                        break;
                    case Operation.PretraziKandidate:
                        response.Result = Controller.Instance.PretraziKandidate(_receiver.ReadType<KandidatSearchFilter>(request.Argument!));
                        break;
                    case Operation.EvidentirajIspit:
                        response.Result = Controller.Instance.EvidentirajIspit(_receiver.ReadType<EvidentirajIspitRequest>(request.Argument!));
                        break;
                    case Operation.KreirajIzvestajProlaznosti:
                        response.Result = Controller.Instance.KreirajIzvestajProlaznosti(_receiver.ReadType<IzvestajProlaznostiKriterijum>(request.Argument!));
                        break;
                    case Operation.VratiKandidatiSaDugovanjem:
                        response.Result = Controller.Instance.VratiKandidatiSaDugovanjem();
                        break;
                    case Operation.EvidentirajUplatu:
                        response.Result = Controller.Instance.EvidentirajUplatu(_receiver.ReadType<EvidentirajUplatuRequest>(request.Argument!));
                        break;
                    default:
                        break;
                }
            }
            catch (ValidacijaException ex)
            {
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("[ProcessRequest] {0}: {1}", request.Operation, ex));
                response.ErrorMessage = "Sistemska greska. Pokusajte ponovo ili kontaktirajte administratora.";
            }
            return response;
        }

        private void Logout(Admin admin)
        {
            Server.loggedIn.RemoveAll(a => a.AdminId == admin.AdminId);
            Server.clients.RemoveAll(c => c._clientSocket == _clientSocket);
            Close();
        }
    }
}
