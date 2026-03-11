using Common.Domain;
using DBBroker;
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
    }
}
