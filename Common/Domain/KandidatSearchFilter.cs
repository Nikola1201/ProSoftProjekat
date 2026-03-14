using System;

namespace Common.Domain
{
    [Serializable]
    public class KandidatSearchFilter
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string Email { get; set; }
        public bool SamoAktivni { get; set; }
    }
}
