using System;
using System.Collections.Generic;

namespace Common.Domain.Izvestaji
{
    [Serializable]
    public class IzvestajDugovanjaKriterijum
    {
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string Kategorija { get; set; }
        public bool IncludeBezDuga { get; set; }
        public bool IncludeOnlyAktivanUpis { get; set; }
    }

    [Serializable]
    public class IzvestajDugovanjaStavkaDto
    {
        public int KandidatId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Jmbg { get; set; }
        public string Kategorija { get; set; }
        public int BrojUpisa { get; set; }
        public decimal UkupnaCenaObuke { get; set; }
        public decimal UkupnoPlaceno { get; set; }
        public decimal Dugovanje { get; set; }
        public string StatusDuga { get; set; }
        public DateTime? DatumPoslednjeUplate { get; set; }
    }

    [Serializable]
    public class IzvestajDugovanjaSummaryDto
    {
        public decimal UkupnoZaduzenje { get; set; }
        public decimal UkupnoPlaceno { get; set; }
        public decimal UkupnoDugovanje { get; set; }
        public int BrojKandidataSaDugom { get; set; }
        public decimal ProcenatNaplate { get; set; }
    }

    [Serializable]
    public class IzvestajDugovanjaResponseDto
    {
        public List<IzvestajDugovanjaStavkaDto> Stavke { get; set; }
        public IzvestajDugovanjaSummaryDto Summary { get; set; }
    }
}
