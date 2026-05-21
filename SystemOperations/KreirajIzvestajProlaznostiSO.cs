using Common.Domain.Izvestaji;
using Common.Validation;
using DBBroker.Reports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SystemOperations
{
    public class KreirajIzvestajProlaznostiSO : SystemOperationBase
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

            List<IzvestajProlaznostiStavkaDto> stavke =
                _broker.ExecuteReport(new IzvestajProlaznostiReport(_kriterijum));

            IzvestajProlaznostiSummaryDto summary = new IzvestajProlaznostiSummaryDto
            {
                UkupnoPolozilo = stavke.Count(s => s.Status == StatusProlaznosti.Polozio),
                UkupnoPalo = stavke.Count(s => s.Status == StatusProlaznosti.Pao),
                UkupnoUToku = stavke.Count(s => s.Status == StatusProlaznosti.UToku)
            };

            int denominator = summary.UkupnoPolozilo + summary.UkupnoPalo;
            summary.ProcenatProlaznosti = denominator == 0
                ? 0m
                : Math.Round((decimal)summary.UkupnoPolozilo * 100m / denominator, 2);

            Result = new IzvestajProlaznostiResponseDto
            {
                Stavke = stavke,
                Summary = summary
            };

            Debug.WriteLine(string.Format(
                "[KreirajIzvestajProlaznosti] stavki={0} Polozilo={1} Palo={2} UToku={3} %={4}",
                stavke.Count, summary.UkupnoPolozilo, summary.UkupnoPalo,
                summary.UkupnoUToku, summary.ProcenatProlaznosti));
        }

        private void Validate(IzvestajProlaznostiKriterijum kriterijum)
        {
            if (kriterijum == null)
            {
                throw new ValidacijaException("Kriterijum za izvestaj nije prosledjen.");
            }

            if (kriterijum.DatumOd.Date > kriterijum.DatumDo.Date)
            {
                throw new ValidacijaException("Datum od ne moze biti veci od datuma do.");
            }

            if (string.IsNullOrWhiteSpace(kriterijum.Kategorija))
            {
                throw new ValidacijaException("Kategorija je obavezna za izvestaj prolaznosti.");
            }

            if (!Enum.IsDefined(typeof(TipIspitaFilter), kriterijum.TipIspita))
            {
                throw new ValidacijaException("Tip ispita nije validan.");
            }
        }
    }
}
