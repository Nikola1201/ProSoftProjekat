using System;
using System.Collections.Generic;

namespace Common.DTO.Izvestaji
{
    /// <summary>Kriterijumi filtriranja za izveštaj dugovanja kandidata.</summary>
    [Serializable]
    public class IzvestajDugovanjaKriterijum
    {
        /// <summary>Početak vremenskog perioda za koji se analiziraju uplate.</summary>
        public DateTime DatumOd { get; set; }
        /// <summary>Kraj vremenskog perioda za koji se analiziraju uplate.</summary>
        public DateTime DatumDo { get; set; }
        /// <summary>Naziv kategorije vozačke dozvole po kojoj se filtrira; prazan string znači sve kategorije.</summary>
        public string Kategorija { get; set; }
        /// <summary>Ako je <see langword="true"/>, u izveštaj se uključuju i kandidati bez dugovanja.</summary>
        public bool IncludeBezDuga { get; set; }
        /// <summary>Ako je <see langword="true"/>, prikazuju se samo kandidati sa aktivnim upisom.</summary>
        public bool IncludeOnlyAktivanUpis { get; set; }
    }

    /// <summary>Jedna stavka izveštaja dugovanja koja prikazuje finansijske podatke za jednog kandidata.</summary>
    [Serializable]
    public class IzvestajDugovanjaStavkaDto
    {
        /// <summary>Identifikator kandidata.</summary>
        public int KandidatId { get; set; }
        /// <summary>Ime kandidata.</summary>
        public string Ime { get; set; }
        /// <summary>Prezime kandidata.</summary>
        public string Prezime { get; set; }
        /// <summary>JMBG kandidata.</summary>
        public string Jmbg { get; set; }
        /// <summary>Kategorija vozačke dozvole za koju je kandidat upisan.</summary>
        public string Kategorija { get; set; }
        /// <summary>Ukupan broj upisa kandidata u pakete obuke.</summary>
        public int BrojUpisa { get; set; }
        /// <summary>Ukupna cena svih paketa obuke u koje je kandidat upisan.</summary>
        public decimal UkupnaCenaObuke { get; set; }
        /// <summary>Ukupan iznos koji je kandidat do sada platio.</summary>
        public decimal UkupnoPlaceno { get; set; }
        /// <summary>Razlika između ukupne cene obuke i uplaćenog iznosa.</summary>
        public decimal Dugovanje { get; set; }
        /// <summary>Tekstualni opis statusa dugovanja (npr. "Duguje", "Izmireno").</summary>
        public string StatusDuga { get; set; }
        /// <summary>Datum poslednje evidentirane uplate; <see langword="null"/> ako nema uplata.</summary>
        public DateTime? DatumPoslednjeUplate { get; set; }
    }

    /// <summary>Zbirni finansijski pokazatelji izveštaja dugovanja za sve kandidate.</summary>
    [Serializable]
    public class IzvestajDugovanjaSummaryDto
    {
        /// <summary>Ukupno zaduženje svih kandidata (suma cena obuke).</summary>
        public decimal UkupnoZaduzenje { get; set; }
        /// <summary>Ukupno naplaćeni iznos od svih kandidata.</summary>
        public decimal UkupnoPlaceno { get; set; }
        /// <summary>Ukupno neizmireno dugovanje svih kandidata.</summary>
        public decimal UkupnoDugovanje { get; set; }
        /// <summary>Broj kandidata koji imaju neizmireno dugovanje.</summary>
        public int BrojKandidataSaDugom { get; set; }
        /// <summary>Procenat naplaćenog iznosa u odnosu na ukupno zaduženje (0–100).</summary>
        public decimal ProcenatNaplate { get; set; }
    }

    /// <summary>Odgovor operacije generisanja izveštaja dugovanja: stavke po kandidatu i zbirni podaci.</summary>
    [Serializable]
    public class IzvestajDugovanjaResponseDto
    {
        /// <summary>Lista stavki izveštaja, po jedna za svakog kandidata koji ispunjava kriterijume.</summary>
        public List<IzvestajDugovanjaStavkaDto> Stavke { get; set; }
        /// <summary>Zbirni finansijski pokazatelji za prikazane kandidate.</summary>
        public IzvestajDugovanjaSummaryDto Summary { get; set; }
    }
}
