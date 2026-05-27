using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class PretraziKandidateSO : SystemOperationBase
    {
        private readonly KandidatSearchFilter _filter;
        public List<Kandidat> Result { get; private set; }

        public PretraziKandidateSO(KandidatSearchFilter filter) : this(filter, null) { }
        public PretraziKandidateSO(KandidatSearchFilter filter, IBroker? broker) : base(broker)
        {
            _filter = NormalizeFilter(filter);
        }

        protected override void ExecuteConcreteOperation()
        {
            IEnumerable<Kandidat> kandidati = _broker.GetAll(new Kandidat()).Cast<Kandidat>();

            if (!string.IsNullOrWhiteSpace(_filter.Ime))
            {
                kandidati = kandidati.Where(k =>
                    !string.IsNullOrWhiteSpace(k.Ime)
                    && k.Ime.ToLower().Contains(_filter.Ime.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(_filter.Prezime))
            {
                kandidati = kandidati.Where(k =>
                    !string.IsNullOrWhiteSpace(k.Prezime)
                    && k.Prezime.ToLower().Contains(_filter.Prezime.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(_filter.JMBG))
            {
                kandidati = kandidati.Where(k => k.JMBG == _filter.JMBG);
            }

            if (!string.IsNullOrWhiteSpace(_filter.Email))
            {
                kandidati = kandidati.Where(k =>
                    !string.IsNullOrWhiteSpace(k.Email)
                    && k.Email.ToLower().Contains(_filter.Email.ToLower()));
            }

            if (_filter.SamoAktivni)
            {
                kandidati = kandidati.Where(k => k.Aktivan);
            }

            Result = kandidati
                .OrderBy(k => k.Prezime)
                .ThenBy(k => k.Ime)
                .ToList();
        }

        private KandidatSearchFilter NormalizeFilter(KandidatSearchFilter filter)
        {
            KandidatSearchFilter normalized = filter ?? new KandidatSearchFilter();

            normalized.Ime = NormalizeString(normalized.Ime);
            normalized.Prezime = NormalizeString(normalized.Prezime);
            normalized.JMBG = NormalizeString(normalized.JMBG);
            normalized.Email = NormalizeString(normalized.Email);

            return normalized;
        }

        private string NormalizeString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.Trim();
        }
    }
}
