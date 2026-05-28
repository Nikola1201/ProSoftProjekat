using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Common.DTO.Izvestaji
{
    /// <summary>Kontrakt za izveštajni upit: SQL + parametri + hidrator redova u <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">Tip jedne stavke izveštaja koja se popunjava iz čitača.</typeparam>
    public interface IReport<T>
    {
        /// <summary>SQL upit koji se izvršava nad bazom (parametrizovan).</summary>
        string Sql { get; }
        /// <summary>Parametri koji se prosleđuju SQL upitu.</summary>
        IEnumerable<SqlParameter> Parameters { get; }
        /// <summary>Mapira redove čitača u objekte tipa <typeparamref name="T"/>.</summary>
        /// <param name="reader">Otvoren <see cref="SqlDataReader"/> pozicioniran na rezultate upita.</param>
        /// <returns>Lista popunjenih objekata tipa <typeparamref name="T"/>.</returns>
        List<T> Hydrate(SqlDataReader reader);
    }
}
