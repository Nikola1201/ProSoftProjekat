using Common.Domain;
using DBBroker;
using Server.SystemOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Controller
    {
        private Broker _broker;

        private static Controller _instance;

        public static Controller Instance => _instance ?? (_instance = new Controller());
        private Controller() { _broker = new Broker(); }

        internal Admin Login(Admin argument)
        {
            AdminLoginSO so = new AdminLoginSO(argument);
            so.ExecuteTemplate();

            return (Admin)so.Result;
        }

        internal Kandidat KreirajKandidata(Kandidat argument)
        {
            KreirajKandidataSO so = new KreirajKandidataSO(argument);
            so.ExecuteTemplate();
            return (Kandidat)so.Result;

        }

        internal List<Kategorija> GetAllKategorije()
        {
            VratiSveKategorijeSO so = new VratiSveKategorijeSO();
            so.ExecuteTemplate();
            return so.Result;
        }

        internal List<Kandidat> GetAllKandidati(bool upisani)
        {
            VratiSveKandidateSO so = new VratiSveKandidateSO(upisani);
            so.ExecuteTemplate();
            return so.Result;
        }

        internal List<PaketObuke> GetAllPaketiObuke()
        {
            VratiSvePaketeObukeSO so = new VratiSvePaketeObukeSO();
            so.ExecuteTemplate();
            return so.Result;
        }

        internal Upis UpisiKandidata(Upis argument)
        {
            UpisiKandidataSO so = new UpisiKandidataSO(argument);
            so.ExecuteTemplate();
            return (Upis)so.Result;
        }

        internal void ObrisiKandidata(Kandidat argument)
        {
            ObrisiKandidataSO so = new ObrisiKandidataSO(argument);
            so.ExecuteTemplate();
            
        }

        internal Instruktor KreirajInstruktora(Instruktor argument)
        {
            KreirajInstruktoraSO so = new KreirajInstruktoraSO(argument);
            so.ExecuteTemplate();
            return (Instruktor)so.Result;
        }

        internal List<Instruktor> GetAllInstruktori()
        {
            VratiSveInstruktoreSO so = new VratiSveInstruktoreSO();
            so.ExecuteTemplate();
            return so.Result;
        }

        internal void ObrisiInstruktora(Instruktor argument)
        {
            ObrisiInstruktoraSO so = new ObrisiInstruktoraSO(argument);
            so.ExecuteTemplate();
        }

        internal List<Vozilo> GetAllVozila()
        {
            VratiSveVozilaSO so = new VratiSveVozilaSO();
            so.ExecuteTemplate();
            return so.Result;
        }

        internal List<Upis> GetAllUpisi()
        {
            VratiSveUpiseSO so = new VratiSveUpiseSO();
            so.ExecuteTemplate();
            return so.Result;
        }

        internal CasVoznje ZakaziCasVoznje(CasVoznje argument)
        {
            ZakaziCasVoznjeSO so = new ZakaziCasVoznjeSO(argument);
            so.ExecuteTemplate();
            return (CasVoznje)so.Result;
        }
    }
}
