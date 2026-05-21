using System;

namespace Common.Domain
{
    [Serializable]
    public class KandidatDugovanjeDto
    {
        public int KandidatId { get; set; }
        public string PunoIme { get; set; }
        public string JMBG { get; set; }
        public decimal UkupnaCena { get; set; }
        public decimal UkupnoPlaceno { get; set; }
        public decimal Dugovanje { get; set; }
    }
}
