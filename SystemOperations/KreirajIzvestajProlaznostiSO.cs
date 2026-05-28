using Common.DTO.Izvestaji;
using Common.Validation;
using DBBroker;
using DBBroker.Reports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za generisanje izveštaja prolaznosti kandidata na ispitima.
    /// Validira kriterijum, izvršava SQL izveštaj i izračunava sumarnu statistiku
    /// (broj položenih, palih, u toku i procenat prolaznosti).
    /// </summary>
    public class KreirajIzvestajProlaznostiSO : SystemOperationBase
    {
        private readonly IzvestajProlaznostiKriterijum _kriterijum;

        /// <summary>Rezultat operacije — izveštaj sa stavkama i sumarnom statistikom.</summary>
        public IzvestajProlaznostiResponseDto Result { get; private set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="kriterijum">Kriterijum za filtriranje izveštaja (period, kategorija, tip ispita).</param>
        public KreirajIzvestajProlaznostiSO(IzvestajProlaznostiKriterijum kriterijum) : this(kriterijum, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="kriterijum">Kriterijum za filtriranje izveštaja (period, kategorija, tip ispita).</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public KreirajIzvestajProlaznostiSO(IzvestajProlaznostiKriterijum kriterijum, IBroker? broker) : base(broker)
        {
            _kriterijum = kriterijum;
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Proverava ispravnost kriterijuma za izveštaj:
        /// prisustvo objekta, validnost datumskog opsega, obaveznost kategorije i tip ispita.
        /// </summary>
        /// <param name="kriterijum">Kriterijum za validaciju.</param>
        /// <exception cref="ValidacijaException">Baca se ako neko od ograničenja nije zadovoljeno.</exception>
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
