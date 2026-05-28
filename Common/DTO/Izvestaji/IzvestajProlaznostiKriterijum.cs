using System;
using System.Collections.Generic;

namespace Common.DTO.Izvestaji
{
    /// <summary>Filter za tip ispita koji se uzima u obzir pri generisanju izveštaja prolaznosti.</summary>
    [Serializable]
    public enum TipIspitaFilter
    {
        /// <summary>Prikazuju se samo teorijski ispiti.</summary>
        Teorijski = 0,
        /// <summary>Prikazuju se samo praktični ispiti vožnje.</summary>
        Prakticni = 1,
        /// <summary>Prikazuju se i teorijski i praktični ispiti.</summary>
        Oba = 2
    }

    /// <summary>Status prolaznosti kandidata na ispitima u okviru obuke.</summary>
    [Serializable]
    public enum StatusProlaznosti
    {
        /// <summary>Kandidat je položio ispit.</summary>
        Polozio = 0,
        /// <summary>Kandidat nije položio ispit.</summary>
        Pao = 1,
        /// <summary>Obuka je u toku — kandidat još nije okončao polaganje.</summary>
        UToku = 2
    }

    /// <summary>Kriterijumi filtriranja za izveštaj prolaznosti kandidata na ispitima.</summary>
    [Serializable]
    public class IzvestajProlaznostiKriterijum
    {
        /// <summary>Početak vremenskog perioda za koji se analiziraju ispiti.</summary>
        public DateTime DatumOd { get; set; }
        /// <summary>Kraj vremenskog perioda za koji se analiziraju ispiti.</summary>
        public DateTime DatumDo { get; set; }
        /// <summary>Tip ispita koji se uzima u obzir (teorijski, praktični ili oba).</summary>
        public TipIspitaFilter TipIspita { get; set; }
        /// <summary>Naziv kategorije vozačke dozvole po kojoj se filtrira; prazan string znači sve kategorije.</summary>
        public string Kategorija { get; set; }
        /// <summary>Ako je <see langword="true"/>, u izveštaj se uključuju kandidati koji nemaju evidentiranih ispita.</summary>
        public bool IncludeNoData { get; set; }
        /// <summary>Ako je <see langword="true"/>, prikazuju se samo kandidati sa aktivnim upisom.</summary>
        public bool IncludeOnlyAktivanUpis { get; set; }
    }

    /// <summary>Jedna stavka izveštaja prolaznosti koja prikazuje podatke o ispitima za jednog kandidata.</summary>
    [Serializable]
    public class IzvestajProlaznostiStavkaDto
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
        /// <summary>Trenutni status prolaznosti kandidata.</summary>
        public StatusProlaznosti Status { get; set; }
        /// <summary>Datum poslednjeg evidentiranog ispita; <see langword="null"/> ako nema ispita.</summary>
        public DateTime? DatumPoslednjegIspita { get; set; }
        /// <summary>Ukupan broj pokušaja teorijskog ispita.</summary>
        public int BrojPokusajaTeorijski { get; set; }
        /// <summary>Ukupan broj pokušaja praktičnog ispita vožnje.</summary>
        public int BrojPokusajaPrakticni { get; set; }
    }

    /// <summary>Zbirni pokazatelji prolaznosti za sve kandidate u izveštaju.</summary>
    [Serializable]
    public class IzvestajProlaznostiSummaryDto
    {
        /// <summary>Ukupan broj kandidata koji su položili.</summary>
        public int UkupnoPolozilo { get; set; }
        /// <summary>Ukupan broj kandidata koji nisu položili.</summary>
        public int UkupnoPalo { get; set; }
        /// <summary>Ukupan broj kandidata čija je obuka još u toku.</summary>
        public int UkupnoUToku { get; set; }
        /// <summary>Procenat kandidata koji su položili u odnosu na ukupan broj (0–100).</summary>
        public decimal ProcenatProlaznosti { get; set; }
    }

    /// <summary>Odgovor operacije generisanja izveštaja prolaznosti: stavke po kandidatu i zbirni podaci.</summary>
    [Serializable]
    public class IzvestajProlaznostiResponseDto
    {
        /// <summary>Lista stavki izveštaja, po jedna za svakog kandidata koji ispunjava kriterijume.</summary>
        public List<IzvestajProlaznostiStavkaDto> Stavke { get; set; }
        /// <summary>Zbirni pokazatelji prolaznosti za prikazane kandidate.</summary>
        public IzvestajProlaznostiSummaryDto Summary { get; set; }
    }
}
