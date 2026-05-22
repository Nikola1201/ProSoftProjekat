using System;
using Common.Domain;

namespace Common.DTO
{
    [Serializable]
    public class KreirajInstruktoraRequest
    {
        public Instruktor Instruktor { get; set; }
        public int KategorijaID { get; set; }
    }
}
