using System.Collections.Generic;
using System.Data.Common;

namespace Common.Domain
{
    /// <summary>
    /// Kontrakt za svaku domensku klasu koja se može upisati i pročitati iz baze.
    /// Definiše SQL fragmente koje broker koristi za INSERT/UPDATE/DELETE/SELECT i metode hidracije iz <see cref="DbDataReader"/>.
    /// </summary>
    public interface IEntity
    {
        /// <summary>Naziv tabele u bazi koja odgovara ovoj entitetskoj klasi.</summary>
        string TableName { get; }

        /// <summary>Naziv kolone primarnog ključa u tabeli.</summary>
        string TableKeyColumn { get; }

        /// <summary>SQL WHERE klauzula koja izoluje red po primarnom ključu.</summary>
        string TableKeyQuery { get; }

        /// <summary>SQL WHERE klauzula za pretragu po sadržajnim atributima (npr. JMBG, Username).</summary>
        string Query { get; }

        /// <summary>SQL VALUES tuple za INSERT u tabelu (bez zagrada).</summary>
        /// <remarks>Pažnja: stringovi se interpoliraju direktno bez parametara — videti CLAUDE.md §11 #1.</remarks>
        string Values { get; }

        /// <summary>Kompletna UPDATE SQL naredba (sa WHERE klauzulom).</summary>
        string Update { get; }

        /// <summary>Hidrira listu entiteta čitajući redove iz datog reader-a.</summary>
        /// <param name="reader">Otvoren reader pozicioniran pre prvog reda.</param>
        /// <returns>Lista popunjenih entiteta; prazna ako reader nema rezultate.</returns>
        List<IEntity> GetReaderList(DbDataReader reader);

        /// <summary>Hidrira jedan entitet iz prve dostupne vrste reader-a.</summary>
        /// <param name="reader">Otvoren reader pozicioniran pre prvog reda.</param>
        /// <returns>Popunjen entitet, ili <c>null</c> ako reader nema redova.</returns>
        IEntity GetReaderResult(DbDataReader reader);
    }
}
