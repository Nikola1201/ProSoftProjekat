using Common.Domain.Izvestaji;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.SystemOperation
{
    internal class KreirajIzvestajProlaznostiSO : SystemOperationBase
    {
        private readonly IzvestajProlaznostiKriterijum _kriterijum;
        public IzvestajProlaznostiResponseDto Result { get; private set; }

        public KreirajIzvestajProlaznostiSO(IzvestajProlaznostiKriterijum kriterijum)
        {
            _kriterijum = kriterijum;
        }

        protected override void ExecuteConcreteOperation()
        {
            Validate(_kriterijum);

            List<IzvestajProlaznostiStavkaDto> stavke = _broker.KreirajIzvestajProlaznosti(_kriterijum);
            IzvestajProlaznostiSummaryDto summary = BuildSummary(stavke);

            Result = new IzvestajProlaznostiResponseDto
            {
                Stavke = stavke,
                Summary = summary
            };
        }

        private void Validate(IzvestajProlaznostiKriterijum kriterijum)
        {
            if (kriterijum == null)
            {
                throw new Exception("Kriterijum za izvestaj nije prosledjen.");
            }

            if (kriterijum.DatumOd.Date > kriterijum.DatumDo.Date)
            {
                throw new Exception("Datum od ne moze biti veci od datuma do.");
            }

            if (string.IsNullOrWhiteSpace(kriterijum.Kategorija))
            {
                throw new Exception("Kategorija je obavezna za izvestaj prolaznosti.");
            }

            if (!Enum.IsDefined(typeof(TipIspitaFilter), kriterijum.TipIspita))
            {
                throw new Exception("Tip ispita nije validan.");
            }
        }

        private IzvestajProlaznostiSummaryDto BuildSummary(List<IzvestajProlaznostiStavkaDto> stavke)
        {
            int ukupnoPolozilo = stavke.Count(s => string.Equals(s.Status, "Polozio", StringComparison.OrdinalIgnoreCase));
            int ukupnoPalo = stavke.Count(s => string.Equals(s.Status, "Pao", StringComparison.OrdinalIgnoreCase));
            int ukupnoUToku = stavke.Count(s => string.Equals(s.Status, "UToku", StringComparison.OrdinalIgnoreCase));

            int denominator = ukupnoPolozilo + ukupnoPalo;
            decimal procenat = denominator == 0 ? 0 : Math.Round((decimal)ukupnoPolozilo * 100m / denominator, 2);

            return new IzvestajProlaznostiSummaryDto
            {
                UkupnoPolozilo = ukupnoPolozilo,
                UkupnoPalo = ukupnoPalo,
                UkupnoUToku = ukupnoUToku,
                ProcenatProlaznosti = procenat
            };
        }
    }
}
