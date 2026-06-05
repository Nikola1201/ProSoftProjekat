using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja zakazani ili održani čas vožnje u okviru određenog upisa.</summary>
    [Serializable]
    public class CasVoznje : IEntity
    {
        /// <summary>Jedinstveni identifikator časa vožnje (PK). Dozvoljene vrednosti: auto-generisan PK; ne validira se.</summary>
        public int CasId { get; set; }

        /// <summary>Identifikator upisa u okviru kojeg se odvija čas (FK na Upis). Dozvoljene vrednosti: strani ključ; mora biti veće od nule. Validira <see cref="Common.Validation.CasVoznjeValidator"/>.</summary>
        public int UpisId { get; set; }

        /// <summary>Identifikator instruktora koji vodi čas (FK na Instruktor). Dozvoljene vrednosti: strani ključ; mora biti veće od nule. Validira <see cref="Common.Validation.CasVoznjeValidator"/>.</summary>
        public int InstruktorId { get; set; }

        /// <summary>Identifikator vozila koje se koristi na času (FK na Vozilo). Dozvoljene vrednosti: strani ključ; mora biti veće od nule. Validira <see cref="Common.Validation.CasVoznjeValidator"/>.</summary>
        public int VoziloId { get; set; }

        /// <summary>Datum i vreme početka časa vožnje. Dozvoljene vrednosti: obavezno; ne sme biti podrazumevani datum. Validira <see cref="Common.Validation.CasVoznjeValidator"/>.</summary>
        public DateTime DatumCas { get; set; }

        /// <summary>Trajanje časa vožnje u minutima. Dozvoljene vrednosti: celobrojno, između 1 i 600 (minuta). Validira <see cref="Common.Validation.CasVoznjeValidator"/>.</summary>
        public int TrajanjMin { get; set; }

        /// <summary>Trenutni status časa: "zakazan", "odrzan" ili "otkazan". Dozvoljene vrednosti: jedna od: zakazan, odrzan, otkazan. Validira <see cref="Common.Validation.CasVoznjeValidator"/>.</summary>
        public string Status { get; set; }  // 'zakazan', 'odrzan', 'otkazan'

        /// <summary>Opcionalna napomena uz čas vožnje. Dozvoljene vrednosti: opciono; najviše 200 karaktera. Validira <see cref="Common.Validation.CasVoznjeValidator"/>.</summary>
        public string Napomena { get; set; }

        /// <inheritdoc/>
        public string TableName => "CasVoznje";

        /// <inheritdoc/>
        public string Values =>
            $"{UpisId}, {InstruktorId}, {VoziloId}, '{DatumCas:yyyy-MM-dd HH:mm}', {TrajanjMin}, '{Status}', '{Napomena}'";

        /// <inheritdoc/>
        public string Query => $"UpisId = {UpisId}";

        /// <inheritdoc/>
        public string TableKeyColumn => "CasId";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"{TableKeyColumn} = {CasId}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE CasVoznje SET " +
            $"UpisId = {UpisId}, " +
            $"InstruktorId = {InstruktorId}, " +
            $"VoziloId = {VoziloId}, " +
            $"DatumCas = '{DatumCas:yyyy-MM-dd HH:mm}', " +
            $"TrajanjMin = {TrajanjMin}, " +
            $"Status = '{Status}', " +
            $"Napomena = '{Napomena}' " +
            $"WHERE CasId = {CasId}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        /// <inheritdoc/>
        public IEntity GetReaderResult(DbDataReader reader)
        {
            return new CasVoznje
            {
                CasId = (int)reader["CasId"],
                UpisId = (int)reader["UpisId"],
                InstruktorId = (int)reader["InstruktorId"],
                VoziloId = (int)reader["VoziloId"],
                DatumCas = (DateTime)reader["DatumCas"],
                TrajanjMin = (int)reader["TrajanjMin"],
                Status = reader["Status"].ToString(),
                Napomena = reader["Napomena"].ToString()
            };
        }
    }
}
