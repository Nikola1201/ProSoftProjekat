using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class VratiSveNeupisaneKandidateSO : SystemOperationBase
    {
        public List<Kandidat> Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            List<Kandidat> svi = _broker.GetAll(new Kandidat()).Cast<Kandidat>().ToList();
            List<Upis> sviUpisi = _broker.GetAll(new Upis()).Cast<Upis>().ToList();

            HashSet<int> upisaniIds = new HashSet<int>(
                sviUpisi.Select(u => u.KandidatId)
            );

            Result = svi.Where(k => !upisaniIds.Contains(k.KandidatId)).ToList();
        }
    }
}