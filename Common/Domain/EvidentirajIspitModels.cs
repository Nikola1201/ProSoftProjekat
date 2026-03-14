using System;

namespace Common.Domain
{
    [Serializable]
    public class EvidentirajIspitRequest
    {
        public int KandidatId { get; set; }
        public DateTime DatumIspita { get; set; }
        public string Tip { get; set; }
        public string Rezultat { get; set; }
        public string Napomena { get; set; }
    }

    [Serializable]
    public class EvidentirajIspitResponse
    {
        public Ispit Ispit { get; set; }
        public int UpisId { get; set; }
        public string UpisStatus { get; set; }
        public bool StatusPromenjen { get; set; }
        public string Poruka { get; set; }
    }
}
