using System;
using System.Collections.Generic;

namespace Common.Domain.Izvestaji
{
    [Serializable]
    public enum TipIspitaFilter
    {
        Teorijski = 0,
        Prakticni = 1,
        Oba = 2
    }

    [Serializable]
    public class IzvestajProlaznostiKriterijum
    {
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public TipIspitaFilter TipIspita { get; set; }
        public string Kategorija { get; set; }
        public bool IncludeNoData { get; set; }
        public bool IncludeOnlyAktivanUpis { get; set; }
    }

    [Serializable]
    public class IzvestajProlaznostiStavkaDto
    {
        public int KandidatId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Jmbg { get; set; }
        public string Kategorija { get; set; }
        public string Status { get; set; }
        public DateTime? DatumPoslednjegIspita { get; set; }
        public int BrojPokusajaTeorijski { get; set; }
        public int BrojPokusajaPrakticni { get; set; }
    }

    [Serializable]
    public class IzvestajProlaznostiSummaryDto
    {
        public int UkupnoPolozilo { get; set; }
        public int UkupnoPalo { get; set; }
        public int UkupnoUToku { get; set; }
        public decimal ProcenatProlaznosti { get; set; }
    }

    [Serializable]
    public class IzvestajProlaznostiResponseDto
    {
        public List<IzvestajProlaznostiStavkaDto> Stavke { get; set; }
        public IzvestajProlaznostiSummaryDto Summary { get; set; }
    }
}
