using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SystemOperation
{
    internal class ObrisiKandidataSO : SystemOperationBase
    {
        private Kandidat argument;

        public ObrisiKandidataSO(Kandidat argument)
        {
            this.argument = argument;
        }

        protected override void ExecuteConcreteOperation()
        {
            Upis upisKandidata = (Upis)_broker.GetEntityByQuery(new Upis() { KandidatId = argument.KandidatId});
            _broker.Delete(upisKandidata);
            _broker.Delete(argument);
            
        }
    }
}
