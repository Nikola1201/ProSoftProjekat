using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za pretragu kandidata po jednom ili više kriterijuma filtera.
    /// Rezultat je sortiran po prezimenu, pa imenu.
    /// </summary>
    public class PretraziKandidateSO : SystemOperationBase
    {
        private readonly KandidatSearchFilter _filter;

        /// <summary>Lista kandidata koji zadovoljavaju zadate kriterijume pretrage.</summary>
        public List<Kandidat> Result { get; private set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="filter">Kriterijumi pretrage; <see langword="null"/> vraća sve kandidate.</param>
        public PretraziKandidateSO(KandidatSearchFilter filter) : this(filter, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="filter">Kriterijumi pretrage; <see langword="null"/> vraća sve kandidate.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public PretraziKandidateSO(KandidatSearchFilter filter, IBroker? broker) : base(broker)
        {
            _filter = NormalizeFilter(filter);
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Normalizuje filter pre primene — uklanja prazne vrednosti i razmake,
        /// a <see langword="null"/> filter zamenjuje praznim objektom.
        /// </summary>
        /// <param name="filter">Filter za normalizaciju.</param>
        /// <returns>Normalizovan filter spreman za primenu.</returns>
        private KandidatSearchFilter NormalizeFilter(KandidatSearchFilter filter)
        {
            KandidatSearchFilter normalized = filter ?? new KandidatSearchFilter();

            normalized.Ime = NormalizeString(normalized.Ime);
            normalized.Prezime = NormalizeString(normalized.Prezime);
            normalized.JMBG = NormalizeString(normalized.JMBG);
            normalized.Email = NormalizeString(normalized.Email);

            return normalized;
        }

        /// <summary>
        /// Trimmuje vrednost stringa ili vraća <see langword="null"/> ako je prazna/whitespace.
        /// </summary>
        /// <param name="value">Vrednost za normalizaciju.</param>
        /// <returns>Trimmovana vrednost, ili <see langword="null"/> ako je prazna.</returns>
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
