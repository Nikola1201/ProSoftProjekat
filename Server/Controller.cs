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
    }
}
