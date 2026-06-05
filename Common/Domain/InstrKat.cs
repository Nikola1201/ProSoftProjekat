using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>Predstavlja vezu između instruktora i kategorije koju je ovlašćen da predaje (asociativna tabela InstrKat).</summary>
    [Serializable]
    public class InstrKat : IEntity
    {
        /// <summary>Identifikator instruktora (deo složenog PK, FK na Instruktor). Dozvoljene vrednosti: deo složenog PK / strani ključ; mora biti veće od nule. Validira <see cref="Common.Validation.InstrKatValidator"/>.</summary>
        public int InstruktorId { get; set; }

        /// <summary>Identifikator kategorije (deo složenog PK, FK na Kategorija). Dozvoljene vrednosti: deo složenog PK / strani ključ; mora biti veće od nule. Validira <see cref="Common.Validation.InstrKatValidator"/>.</summary>
        public int KategorijaID { get; set; }

        /// <summary>Datum i vreme kada je kategorija dodeljena instruktoru. Dozvoljene vrednosti: obavezno; ne sme biti podrazumevani datum. Validira <see cref="Common.Validation.InstrKatValidator"/>.</summary>
        public DateTime DatumDodele { get; set; }

        /// <summary>Označava da li je veza između instruktora i kategorije trenutno aktivna. Dozvoljene vrednosti: true ili false; ne validira se.</summary>
        public bool Aktivno { get; set; }

        /// <inheritdoc/>
        public string TableName => "InstrKat";

        /// <inheritdoc/>
        public string Values =>
            $"{InstruktorId}, {KategorijaID}, '{DatumDodele:yyyy-MM-dd HH:mm:ss}', {(Aktivno ? 1 : 0)}";

        /// <inheritdoc/>
        public string Query => $"InstruktorId = {InstruktorId}";

        /// <summary>
        /// Naziv kolone(a) primarnog ključa u tabeli.
        /// InstrKat koristi složeni PK: "InstruktorId, KategorijaID".
        /// </summary>
        public string TableKeyColumn => "InstruktorId, KategorijaID";

        /// <inheritdoc/>
        public string TableKeyQuery =>
            $"InstruktorId = {InstruktorId} AND KategorijaID = {KategorijaID}";

        /// <inheritdoc/>
        public string Update =>
            $"UPDATE InstrKat SET " +
            $"DatumDodele = '{DatumDodele:yyyy-MM-dd HH:mm:ss}', " +
            $"Aktivno = {(Aktivno ? 1 : 0)} " +
            $"WHERE InstruktorId = {InstruktorId} AND KategorijaID = {KategorijaID}";

        /// <inheritdoc/>
        public List<IEntity> GetReaderList(DbDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new InstrKat
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    KategorijaID = (int)reader["KategorijaID"],
                    DatumDodele = (DateTime)reader["DatumDodele"],
                    Aktivno = (bool)reader["Aktivno"]
                });
            }
            return list;
        }

        /// <inheritdoc/>
        public IEntity GetReaderResult(DbDataReader reader)
        {
            if (reader.Read())
            {
                return new InstrKat
                {
                    InstruktorId = (int)reader["InstruktorId"],
                    KategorijaID = (int)reader["KategorijaID"],
                    DatumDodele = (DateTime)reader["DatumDodele"],
                    Aktivno = (bool)reader["Aktivno"]
                };
            }
            return null;
        }
    }
}
