using Common.Domain.Izvestaji;
using Common.Validation;
using System;
using System.Diagnostics;

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

            Debug.WriteLine(string.Format(
                "[KreirajIzvestajProlaznosti] kategorija={0} tip={1} od={2:yyyy-MM-dd} do={3:yyyy-MM-dd} includeNoData={4} samoAktivni={5}",
                _kriterijum.Kategorija,
                _kriterijum.TipIspita,
                _kriterijum.DatumOd,
                _kriterijum.DatumDo,
                _kriterijum.IncludeNoData,
                _kriterijum.IncludeOnlyAktivanUpis));

            Result = _broker.KreirajIzvestajProlaznosti(_kriterijum);

            Debug.WriteLine(string.Format(
                "[KreirajIzvestajProlaznosti] vraceno stavki={0} (Polozilo={1} Palo={2} UToku={3} %={4})",
                Result?.Stavke?.Count ?? 0,
                Result?.Summary?.UkupnoPolozilo ?? 0,
                Result?.Summary?.UkupnoPalo ?? 0,
                Result?.Summary?.UkupnoUToku ?? 0,
                Result?.Summary?.ProcenatProlaznosti ?? 0m));
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
