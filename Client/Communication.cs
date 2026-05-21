using Common.Communication;
using Common.Domain;
using Common.DTO.Izvestaji;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Client
{
    internal class Communication
    {
        private static Communication _instance;
        public static Communication Instance => _instance ?? (_instance = new Communication());
        private Communication() { }

        private Socket _socket;
        private Sender _sender;
        private Receiver _receiver;

        public bool IsConnected { get; private set; }
        public event EventHandler ConnectionLost;

        public void Connect()
        {
            if (_socket != null)
            {
                try { _socket.Shutdown(SocketShutdown.Both); } catch { }
                try { _socket.Close(); } catch { }
            }

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect("127.0.0.1", 9999);

            // Detect truly-dead connections via TCP keep-alive instead of a blanket read timeout —
            // a 15s ReceiveTimeout used to fire ConnectionLost on any slow legitimate response
            // (large reports, slow joins), and after it fires the socket is tainted and unrecoverable.
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            try
            {
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 30);
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 5);
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 3);
            }
            catch (SocketException) { }

            _sender = new Sender(_socket);
            _receiver = new Receiver(_socket);
            IsConnected = true;
        }

        private TResult SafeCall<TResult>(Func<TResult> call)
        {
            try { return call(); }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                IsConnected = false;
                ConnectionLost?.Invoke(this, EventArgs.Empty);
                throw new ConnectionLostException(ex);
            }
        }

        private void SafeCall(Action call) => SafeCall<object>(() => { call(); return null; });

        internal T ResultAs<T>(Response response) => _receiver.ReadType<T>(response.Result!);

        internal Response Login(Admin admin) => SafeCall(() =>
        {
            Request request = new Request() { Argument = admin, Operation = Operation.Login };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal void Logout(Admin admin) => SafeCall(() =>
        {
            Request request = new Request() { Argument = admin, Operation = Operation.Logout };
            _sender.Send(request);
            _receiver.Receive<Response>();
        });

        internal List<Kategorija> GetAllKategorije() => SafeCall(() =>
        {
            Request request = new Request() { Operation = Operation.GetAllKategorije };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Kategorija>>(response.Result!);
        });

        internal Response CreateKandidat(Kandidat kandidat) => SafeCall(() =>
        {
            Request request = new Request() { Argument = kandidat, Operation = Operation.KreirajKandidata };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal List<Kandidat> GetAllKandidati(bool upisani) => SafeCall(() =>
        {
            Request request = new Request() { Argument = upisani, Operation = Operation.GetAllKandidati };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Kandidat>>(response.Result!);
        });

        internal List<PaketObuke> GetAllPaketiObuke() => SafeCall(() =>
        {
            Request request = new Request() { Operation = Operation.GetAllPaketiObuke };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<PaketObuke>>(response.Result!);
        });

        internal Response UpisiKandidata(Upis upis) => SafeCall(() =>
        {
            Request request = new Request() { Argument = upis, Operation = Operation.UpisiKandidata };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal Response ObrisiKandidata(Kandidat kandidat) => SafeCall(() =>
        {
            Request request = new Request() { Argument = kandidat, Operation = Operation.ObrisiKandidata };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal Response CreateInstruktor(Instruktor instruktor) => SafeCall(() =>
        {
            Request request = new Request() { Argument = instruktor, Operation = Operation.KreirajInstruktora };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal List<Instruktor> GetAllInstruktori() => SafeCall(() =>
        {
            Request request = new Request() { Operation = Operation.GetAllInstruktori };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Instruktor>>(response.Result!);
        });

        internal Response ObrisiInstruktora(Instruktor instruktor) => SafeCall(() =>
        {
            Request request = new Request() { Argument = instruktor, Operation = Operation.ObrisiInstruktora };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal List<Vozilo> GetAllVozila() => SafeCall(() =>
        {
            Request request = new Request() { Operation = Operation.GetAllVozila };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Vozilo>>(response.Result!);
        });

        internal List<Upis> GetAllUpisi() => SafeCall(() =>
        {
            Request request = new Request() { Operation = Operation.GetAllUpisi };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<Upis>>(response.Result!);
        });

        internal Response ZakaziCasVoznje(CasVoznje casVoznje) => SafeCall(() =>
        {
            Request request = new Request() { Argument = casVoznje, Operation = Operation.ZakaziCasVoznje };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal List<CasVoznje> GetAllCasVoznje() => SafeCall(() =>
        {
            Request request = new Request() { Operation = Operation.GetAllCasVoznje };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            return _receiver.ReadType<List<CasVoznje>>(response.Result!);
        });

        internal Response OtkaziCasVoznje(CasVoznje casVoznje) => SafeCall(() =>
        {
            Request request = new Request() { Argument = casVoznje, Operation = Operation.OtkaziCasVoznje };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal List<Kandidat> PretraziKandidate(KandidatSearchFilter filter) => SafeCall(() =>
        {
            Request request = new Request() { Argument = filter, Operation = Operation.PretraziKandidate };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            if (!string.IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);
            return _receiver.ReadType<List<Kandidat>>(response.Result!);
        });

        internal Response EvidentirajIspit(EvidentirajIspitRequest requestModel) => SafeCall(() =>
        {
            Request request = new Request() { Argument = requestModel, Operation = Operation.EvidentirajIspit };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal Response KreirajIzvestajProlaznosti(IzvestajProlaznostiKriterijum kriterijum) => SafeCall(() =>
        {
            Request request = new Request() { Argument = kriterijum, Operation = Operation.KreirajIzvestajProlaznosti };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });

        internal List<KandidatDugovanjeDto> VratiKandidatiSaDugovanjem() => SafeCall(() =>
        {
            Request request = new Request() { Operation = Operation.VratiKandidatiSaDugovanjem };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            if (!string.IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);
            return _receiver.ReadType<List<KandidatDugovanjeDto>>(response.Result!);
        });

        internal EvidentirajUplatuResponse EvidentirajUplatu(EvidentirajUplatuRequest req) => SafeCall(() =>
        {
            Request request = new Request() { Argument = req, Operation = Operation.EvidentirajUplatu };
            _sender.Send(request);
            Response response = _receiver.Receive<Response>();
            if (!string.IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);
            return _receiver.ReadType<EvidentirajUplatuResponse>(response.Result!);
        });

        internal Response KreirajIzvestajDugovanja(IzvestajDugovanjaKriterijum kriterijum) => SafeCall(() =>
        {
            Request request = new Request() { Argument = kriterijum, Operation = Operation.KreirajIzvestajDugovanja };
            _sender.Send(request);
            return _receiver.Receive<Response>();
        });
    }
}
