using Common.Communication;
using Common.Domain;
using System;
using System.Collections.Generic;
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

        internal List<Kategorija> GetAllKategorije()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllKategorije
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return (List<Kategorija>)response.Result;
        }

        internal Response CreateKandidat(Kandidat kandidat)
        {
            Request request = new Request()
            {
                Argument = kandidat,
                Operation = Operation.KreirajKandidata
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }

        internal List<Kandidat> GetAllKandidati(bool upisani)
        {
            Request request = new Request()
            {
                Argument = upisani,
                Operation = Operation.GetAllKandidati
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return (List<Kandidat>)response.Result;
        }

        internal List<PaketObuke> GetAllPaketiObuke()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllPaketiObuke
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return (List<PaketObuke>)response.Result;
        }

        internal Response UpisiKandidata(Upis upis)
        {
            Request request = new Request()
            {
                Argument = upis,
                Operation = Operation.UpisiKandidata
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }

        internal Response ObrisiKandidata(Kandidat kandidat)
        {
            Request request = new Request()
            {
                Argument = kandidat,
                Operation = Operation.ObrisiKandidata
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }

        internal Response CreateInstruktor(Instruktor instruktor)
        {
            Request request = new Request()
            {
                Argument = instruktor,
                Operation = Operation.KreirajInstruktora
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }

        internal List<Instruktor> GetAllInstruktori()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllInstruktori
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return (List<Instruktor>)response.Result;
        }

        internal Response ObrisiInstruktora(Instruktor instruktor)
        {
            Request request = new Request()
            {
                Argument = instruktor,
                Operation = Operation.ObrisiInstruktora
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }

        internal List<Vozilo> GetAllVozila()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllVozila
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return (List<Vozilo>)response.Result;
        }

        internal List<Upis> GetAllUpisi()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllUpisi
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return (List<Upis>)response.Result;
        }

        internal Response ZakaziCasVoznje(CasVoznje casVoznje)
        {
            Request request = new Request()
            {
                Argument = casVoznje,
                Operation = Operation.ZakaziCasVoznje
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }

        internal List<CasVoznje> GetAllCasVoznje()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllCasVoznje
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return (List<CasVoznje>)response.Result;
        }

        internal Response OtkaziCasVoznje(CasVoznje casVoznje)
        {
            Request request = new Request()
            {
                Argument = casVoznje,
                Operation = Operation.OtkaziCasVoznje
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();
            return response;
        }

        internal List<Kandidat> PretraziKandidate(KandidatSearchFilter filter)
        {
            Request request = new Request()
            {
                Argument = filter,
                Operation = Operation.PretraziKandidate
            };
            _sender.Send(request);
            Response response = (Response)_receiver.Receive();

            if (response.Exception != null)
            {
                throw response.Exception;
            }

            return (List<Kandidat>)response.Result;
        }
    }
}
