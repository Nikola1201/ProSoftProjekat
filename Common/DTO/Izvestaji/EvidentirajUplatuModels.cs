using Common.Domain;
using System;

namespace Common.DTO.Izvestaji
{
    /// <summary>Zahtev za evidentiranje uplate kandidata za obuku.</summary>
    [Serializable]
    public class EvidentirajUplatuRequest
    {
        /// <summary>Identifikator kandidata koji vrši uplatu.</summary>
        public int KandidatId { get; set; }
        /// <summary>Identifikator upisa za koji se uplata evidentira; <see langword="null"/> ako se uplata ne vezuje za konkretan upis.</summary>
        public int? UpisId { get; set; }
        /// <summary>Iznos uplate u dinarima.</summary>
        public decimal Iznos { get; set; }
        /// <summary>Način plaćanja (npr. "Gotovina", "Kartica").</summary>
        public string NacinPlacanja { get; set; }
        /// <summary>Datum kada je uplata izvršena.</summary>
        public DateTime DatumPlacanja { get; set; }
        /// <summary>Opciona napomena uz uplatu.</summary>
        public string Napomena { get; set; }
    }

    /// <summary>Odgovor operacije evidentiranja uplate sa podacima o plaćanju i preostalom dugovanju.</summary>
    [Serializable]
    public class EvidentirajUplatuResponse
    {
        /// <summary>Novoevidentirana uplata.</summary>
        public Placanje Placanje { get; set; }
        /// <summary>Identifikator upisa za koji je uplata evidentirana.</summary>
        public int UpisId { get; set; }
        /// <summary>Preostalo dugovanje kandidata za dati upis nakon evidentiranja uplate.</summary>
        public decimal PreostaloDugovanje { get; set; }
        /// <summary>Poruka za korisnika koja opisuje ishod operacije.</summary>
        public string Poruka { get; set; }
    }
}
