using System;

namespace Common.Domain
{
    [Serializable]
    public class EvidentirajUplatuRequest
    {
        public int KandidatId { get; set; }
        public int? UpisId { get; set; }
        public decimal Iznos { get; set; }
        public string NacinPlacanja { get; set; }
        public DateTime DatumPlacanja { get; set; }
        public string Napomena { get; set; }
    }

    [Serializable]
    public class EvidentirajUplatuResponse
    {
        public Placanje Placanje { get; set; }
        public int UpisId { get; set; }
        public decimal PreostaloDugovanje { get; set; }
        public string Poruka { get; set; }
    }
}
