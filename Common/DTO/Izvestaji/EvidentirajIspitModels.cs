using Common.Domain;
using System;

namespace Common.DTO.Izvestaji
{
    /// <summary>Zahtev za evidentiranje rezultata ispita kandidata.</summary>
    [Serializable]
    public class EvidentirajIspitRequest
    {
        /// <summary>Identifikator kandidata za kojeg se evidentira ispit.</summary>
        public int KandidatId { get; set; }
        /// <summary>Datum kada je ispit održan.</summary>
        public DateTime DatumIspita { get; set; }
        /// <summary>Tip ispita (npr. "Teorijski" ili "Prakticni").</summary>
        public string Tip { get; set; }
        /// <summary>Rezultat ispita (npr. "Polozio" ili "Pao").</summary>
        public string Rezultat { get; set; }
        /// <summary>Opciona napomena uz evidentiranje ispita.</summary>
        public string Napomena { get; set; }
    }

    /// <summary>Odgovor operacije evidentiranja ispita sa podacima o ispitu i eventualnoj promeni statusa upisa.</summary>
    [Serializable]
    public class EvidentirajIspitResponse
    {
        /// <summary>Novokreirani ili ažurirani ispit.</summary>
        public Ispit Ispit { get; set; }
        /// <summary>Identifikator upisa koji je povezan sa evidentiranim ispitom.</summary>
        public int UpisId { get; set; }
        /// <summary>Novi status upisa nakon evidentiranja ispita.</summary>
        public string UpisStatus { get; set; }
        /// <summary>Označava da li je status upisa promenjen kao posledica evidentiranja ispita.</summary>
        public bool StatusPromenjen { get; set; }
        /// <summary>Poruka za korisnika koja opisuje ishod operacije.</summary>
        public string Poruka { get; set; }
    }
}
