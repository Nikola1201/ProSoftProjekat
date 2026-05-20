using Common.Communication;
using Common.Domain;
using Common.Domain.Izvestaji;
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
            _socket.Connect("127.0.0.1", 9999);
            _sender = new Sender(_socket);
            _receiver = new Receiver(_socket);
        }

        internal T ResultAs<T>(Response response) => _receiver.ReadType<T>(response.Result!);

        internal Response Login(Admin admin)
        {
            Request request = new Request()
            {
                Argument = admin,
                Operation = Operation.Login
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }
        internal void Logout(Admin admin)
        {
            Request request = new Request()
            {
                Argument = admin,
                Operation = Operation.Logout
            };
            _sender.Send(request);
        }

        internal List<Kategorija> GetAllKategorije()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllKategorije
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Kategorija>>(response.Result!);
        }

        internal Response CreateKandidat(Kandidat kandidat)
        {
            Request request = new Request()
            {
                Argument = kandidat,
                Operation = Operation.KreirajKandidata
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal List<Kandidat> GetAllKandidati(bool upisani)
        {
            Request request = new Request()
            {
                Argument = upisani,
                Operation = Operation.GetAllKandidati
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Kandidat>>(response.Result!);
        }

        internal List<PaketObuke> GetAllPaketiObuke()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllPaketiObuke
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<PaketObuke>>(response.Result!);
        }

        internal Response UpisiKandidata(Upis upis)
        {
            Request request = new Request()
            {
                Argument = upis,
                Operation = Operation.UpisiKandidata
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal Response ObrisiKandidata(Kandidat kandidat)
        {
            Request request = new Request()
            {
                Argument = kandidat,
                Operation = Operation.ObrisiKandidata
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal Response CreateInstruktor(Instruktor instruktor)
        {
            Request request = new Request()
            {
                Argument = instruktor,
                Operation = Operation.KreirajInstruktora
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal List<Instruktor> GetAllInstruktori()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllInstruktori
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Instruktor>>(response.Result!);
        }

        internal Response ObrisiInstruktora(Instruktor instruktor)
        {
            Request request = new Request()
            {
                Argument = instruktor,
                Operation = Operation.ObrisiInstruktora
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal List<Vozilo> GetAllVozila()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllVozila
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Vozilo>>(response.Result!);
        }

        internal List<Upis> GetAllUpisi()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllUpisi
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Upis>>(response.Result!);
        }

        internal Response ZakaziCasVoznje(CasVoznje casVoznje)
        {
            Request request = new Request()
            {
                Argument = casVoznje,
                Operation = Operation.ZakaziCasVoznje
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal List<CasVoznje> GetAllCasVoznje()
        {
            Request request = new Request()
            {
                Operation = Operation.GetAllCasVoznje
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<CasVoznje>>(response.Result!);
        }

        internal Response OtkaziCasVoznje(CasVoznje casVoznje)
        {
            Request request = new Request()
            {
                Argument = casVoznje,
                Operation = Operation.OtkaziCasVoznje
            };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal List<Kandidat> PretraziKandidate(KandidatSearchFilter filter)
        {
            Request request = new Request()
            {
                Argument = filter,
                Operation = Operation.PretraziKandidate
            };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new Exception(response.ErrorMessage);
            }

            return _receiver.ReadType<List<Kandidat>>(response.Result!);
        }

        internal Response EvidentirajIspit(EvidentirajIspitRequest requestModel)
        {
            Request request = new Request()
            {
                Argument = requestModel,
                Operation = Operation.EvidentirajIspit
            };

            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal Response KreirajIzvestajProlaznosti(IzvestajProlaznostiKriterijum kriterijum)
        {
            Request request = new Request()
            {
                Argument = kriterijum,
                Operation = Operation.KreirajIzvestajProlaznosti
            };

            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

        internal Response KreirajIzvestajDugovanja(IzvestajDugovanjaKriterijum kriterijum)
        {
            Request request = new Request()
            {
                Argument = kriterijum,
                Operation = Operation.KreirajIzvestajDugovanja
            };

            _sender.Send(request);
            return _receiver.Receive<Response>();
        }

    }
}
