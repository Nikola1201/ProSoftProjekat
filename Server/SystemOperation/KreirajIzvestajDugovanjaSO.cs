using Common.Domain.Izvestaji;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class KreirajIzvestajDugovanjaSO : SystemOperationBase
    {
        private readonly IzvestajDugovanjaKriterijum _kriterijum;
        public IzvestajDugovanjaResponseDto Result { get; private set; }

        public KreirajIzvestajDugovanjaSO(IzvestajDugovanjaKriterijum kriterijum)
        {
            _kriterijum = kriterijum;
        }

        protected override void ExecuteConcreteOperation()
        {
            Validate(_kriterijum);

            List<IzvestajDugovanjaStavkaDto> stavke = _broker.KreirajIzvestajDugovanja(_kriterijum);
            IzvestajDugovanjaSummaryDto summary = BuildSummary(stavke);

            Result = new IzvestajDugovanjaResponseDto
            {
                Stavke = stavke,
                Summary = summary
            };
        }

        private void Validate(IzvestajDugovanjaKriterijum kriterijum)
        {
            if (kriterijum == null)
            {
                throw new Exception("Kriterijum za izvestaj dugovanja nije prosledjen.");
            }

            if (kriterijum.DatumOd.Date > kriterijum.DatumDo.Date)
            {
                throw new Exception("Datum od ne moze biti veci od datuma do.");
            }

            if (string.IsNullOrWhiteSpace(kriterijum.Kategorija))
            {
                throw new Exception("Kategorija je obavezna za izvestaj dugovanja.");
            }
        }

        private IzvestajDugovanjaSummaryDto BuildSummary(List<IzvestajDugovanjaStavkaDto> stavke)
        {
            decimal ukupnoZaduzenje = stavke.Sum(s => s.UkupnaCenaObuke);
            decimal ukupnoPlaceno = stavke.Sum(s => s.UkupnoPlaceno);
            decimal ukupnoDugovanje = stavke.Sum(s => s.Dugovanje);
            int brojKandidataSaDugom = stavke.Count(s => s.Dugovanje > 0m);

            decimal procenatNaplate = ukupnoZaduzenje <= 0m
                ? 0m
                : Math.Round((ukupnoPlaceno / ukupnoZaduzenje) * 100m, 2);

            return new IzvestajDugovanjaSummaryDto
            {
                UkupnoZaduzenje = ukupnoZaduzenje,
                UkupnoPlaceno = ukupnoPlaceno,
                UkupnoDugovanje = ukupnoDugovanje,
                BrojKandidataSaDugom = brojKandidataSaDugom,
                ProcenatNaplate = procenatNaplate
            };
        }
    }
}
