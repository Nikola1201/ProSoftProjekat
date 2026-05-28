using System;

namespace Common.DTO.Izvestaji
{
    /// <summary>Sažeti prikaz dugovanja jednog kandidata, koristi se u pregledu kandidata sa dugovanjem.</summary>
    [Serializable]
    public class KandidatDugovanjeDto
    {
        /// <summary>Identifikator kandidata.</summary>
        public int KandidatId { get; set; }
        /// <summary>Puno ime kandidata (ime i prezime).</summary>
        public string PunoIme { get; set; }
        /// <summary>Jedinstveni matični broj građana kandidata.</summary>
        public string JMBG { get; set; }
        /// <summary>Ukupna cena svih paketa obuke u koje je kandidat upisan.</summary>
        public decimal UkupnaCena { get; set; }
        /// <summary>Ukupan iznos koji je kandidat do sada platio.</summary>
        public decimal UkupnoPlaceno { get; set; }
        /// <summary>Neizmireno dugovanje kandidata (razlika između ukupne cene i plaćenog iznosa).</summary>
        public decimal Dugovanje { get; set; }
    }
}
