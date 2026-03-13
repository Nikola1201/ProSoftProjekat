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
            List<Upis> upisiKandidata = _broker.GetEntitiesByQuery(new Upis())
                                        .Cast<Upis>()
                                        .ToList();
            foreach(Upis up in upisiKandidata)
            {
                _broker.Delete(up);
            }
            _broker.Delete(argument);
            
        }
    }
}
