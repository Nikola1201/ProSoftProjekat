using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class VratiSveKandidateSO : SystemOperationBase
    {
        public List<Kandidat> Result { get; private set; }
        private bool _upisani;
        public VratiSveKandidateSO(bool upisani)
        {
            _upisani = upisani;
        }
        protected override void ExecuteConcreteOperation()
        {
            List<Kandidat> svi = _broker.GetAll(new Kandidat()).Cast<Kandidat>().ToList();
            List<Upis> sviUpisi = _broker.GetAll(new Upis()).Cast<Upis>().ToList();

            HashSet<int> upisaniIds = new HashSet<int>(
                sviUpisi.Select(u => u.KandidatId)
            );
            if (_upisani == false)
            {
                Result = svi.Where(k => !upisaniIds.Contains(k.KandidatId) && k.Aktivan == true).ToList();
            }
            else { 
                Result = svi.Where(k => upisaniIds.Contains(k.KandidatId)).ToList();
            }
        }
    }
}