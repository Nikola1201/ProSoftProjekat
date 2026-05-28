using System.Collections.Generic;
using Common.Domain;
using Common.DTO.Izvestaji;

namespace DBBroker
{
    /// <summary>
    /// Kontrakt za pristup bazi iz sistemskih operacija.
    /// Metode pretpostavljaju otvorenu konekciju i aktivnu transakciju (osim Open/Close).
    /// </summary>
    public interface IBroker
    {
        /// <summary>Otvara konekciju ka bazi (idempotentno).</summary>
        void OpenConnection();

        /// <summary>Zatvara konekciju ka bazi.</summary>
        void CloseConnection();

        /// <summary>Započinje novu transakciju nad otvorenom konekcijom.</summary>
        void BeginTransaction();

        /// <summary>Potvrđuje (commit) trenutnu transakciju.</summary>
        void Commit();

        /// <summary>Poništava (rollback) trenutnu transakciju.</summary>
        void Rollback();

        /// <summary>Upisuje entitet u tabelu i vraća prosleđen entitet.</summary>
        /// <param name="entity">Entitet za upis.</param>
        /// <returns>Prosleđen entitet.</returns>
        IEntity Add(IEntity entity);

        /// <summary>Ažurira entitet u tabeli koristeći njegovu <c>Update</c> SQL naredbu.</summary>
        /// <param name="entity">Entitet sa novim vrednostima.</param>
        void Update(IEntity entity);

        /// <summary>Briše red iz tabele po primarnom ključu entiteta.</summary>
        /// <param name="entity">Entitet čiji ključ određuje šta se briše.</param>
        void Delete(IEntity entity);

        /// <summary>Vraća entitet sa zadatim primarnim ključem ili null ako ne postoji.</summary>
        /// <param name="entity">Entitet sa popunjenim primarnim ključem.</param>
        /// <returns>Pronađeni entitet ili null.</returns>
        IEntity GetEntityByID(IEntity entity);

        /// <summary>Vraća jedan entitet koji zadovoljava <see cref="IEntity.Query"/> klauzulu.</summary>
        /// <param name="entity">Entitet čiji Query određuje uslov pretrage.</param>
        /// <returns>Pronađeni entitet ili null.</returns>
        IEntity GetEntityByQuery(IEntity entity);

        /// <summary>Vraća sve entitete iz tabele.</summary>
        /// <param name="entity">Prazan entitet koji određuje tabelu.</param>
        /// <returns>Lista entiteta.</returns>
        List<IEntity> GetAll(IEntity entity);

        /// <summary>Vraća sve entitete koji zadovoljavaju <see cref="IEntity.Query"/> klauzulu.</summary>
        /// <param name="entity">Entitet čiji Query određuje uslov pretrage.</param>
        /// <returns>Lista entiteta.</returns>
        List<IEntity> GetEntitiesByQuery(IEntity entity);

        /// <summary>Izvršava parametrizovani izveštajni upit i hidrira rezultate u T.</summary>
        /// <typeparam name="T">Tip reda izveštaja.</typeparam>
        /// <param name="report">Definicija izveštaja: SQL + parametri + hidrator.</param>
        /// <returns>Lista redova izveštaja.</returns>
        List<T> ExecuteReport<T>(IReport<T> report);
    }
}
