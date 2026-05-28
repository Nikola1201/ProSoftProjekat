using System;

namespace Common.Domain
{
    /// <summary>Filter kriterijumi za pretragu kandidata na osnovu jednog ili više atributa.</summary>
    [Serializable]
    public class KandidatSearchFilter
    {
        /// <summary>Filter po imenu (case-insensitive substring match). <c>null</c> ili prazan string znači bez filtera.</summary>
        public string Ime { get; set; }

        /// <summary>Filter po prezimenu (case-insensitive substring match). <c>null</c> ili prazan string znači bez filtera.</summary>
        public string Prezime { get; set; }

        /// <summary>Filter po tačnom JMBG-u. <c>null</c> ili prazan string znači bez filtera.</summary>
        public string JMBG { get; set; }

        /// <summary>Filter po email adresi (case-insensitive substring match). <c>null</c> ili prazan string znači bez filtera.</summary>
        public string Email { get; set; }

        /// <summary>Ako je <c>true</c>, vraćaju se samo aktivni kandidati; ako je <c>false</c>, vraćaju se svi.</summary>
        public bool SamoAktivni { get; set; }
    }
}
