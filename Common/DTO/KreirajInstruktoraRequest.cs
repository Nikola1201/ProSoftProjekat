using System;
using Common.Domain;

namespace Common.DTO
{
    /// <summary>Zahtev za kreiranje novog instruktora sa dodeljenom kategorijom vozačke dozvole.</summary>
    [Serializable]
    public class KreirajInstruktoraRequest
    {
        /// <summary>Podaci o instruktoru koji se kreira.</summary>
        public Instruktor Instruktor { get; set; }
        /// <summary>Identifikator kategorije vozačke dozvole koja se dodeljuje instruktoru.</summary>
        public int KategorijaID { get; set; }
    }
}
