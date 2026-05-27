using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveKandidateSO : SystemOperationBase
    {
        public List<Kandidat> Result { get; private set; }
        private readonly bool _upisani;
        public VratiSveKandidateSO(bool upisani) : this(upisani, null) { }
        public VratiSveKandidateSO(bool upisani, IBroker? broker) : base(broker)
        {
            _upisani = upisani;
        }
        protected override void ExecuteConcreteOperation()
        {
            List<Kandidat> svi = _broker.GetAll(new Kandidat()).Cast<Kandidat>().ToList();
            List<Upis> sviUpisi = _broker.GetAll(new Upis()).Cast<Upis>().ToList();

            HashSet<int> upisaniIds = new HashSet<int>(
                sviUpisi
                    .Where(u => string.Equals(u.Status, "aktivan", StringComparison.OrdinalIgnoreCase))
                    .Select(u => u.KandidatId)
            );
            if (!_upisani)
            {
                Result = svi.Where(k => !upisaniIds.Contains(k.KandidatId) && k.Aktivan).ToList();
            }
            else
            {
                Result = svi.Where(k => upisaniIds.Contains(k.KandidatId)).ToList();
            }
        }
    }
}