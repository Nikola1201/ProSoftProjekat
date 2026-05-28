using Common.Domain;
using DBBroker;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za dohvatanje kandidata filtrirano prema statusu upisa.
    /// Ako je parametar <c>upisani</c> <see langword="true"/>, vraća kandidate sa aktivnim upisom;
    /// u suprotnom vraća aktivne kandidate koji trenutno nisu upisani.
    /// </summary>
    public class VratiSveKandidateSO : SystemOperationBase
    {
        /// <summary>Lista kandidata vraćena iz baze prema zadatom kriterijumu upisa.</summary>
        public List<Kandidat> Result { get; private set; }

        private readonly bool _upisani;

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="upisani">
        /// <see langword="true"/> — vraća kandidate sa aktivnim upisom;
        /// <see langword="false"/> — vraća aktivne kandidate bez upisa.
        /// </param>
        public VratiSveKandidateSO(bool upisani) : this(upisani, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="upisani">
        /// <see langword="true"/> — vraća kandidate sa aktivnim upisom;
        /// <see langword="false"/> — vraća aktivne kandidate bez upisa.
        /// </param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public VratiSveKandidateSO(bool upisani, IBroker? broker) : base(broker)
        {
            _upisani = upisani;
        }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            List<Kandidat> svi = _broker.GetAll(new Kandidat()).Cast<Kandidat>().ToList();
            List<Upis> sviUpisi = _broker.GetAll(new Upis()).Cast<Upis>().ToList();

            HashSet<int> upisaniIds = new HashSet<int>(
                sviUpisi
                    .Where(u => string.Equals(u.Status, "aktivan", StringComparison.OrdinalIgnoreCase))
                    .Select(u => u.KandidatId)
            );
            if (!_upisani)
            {
                Result = svi.Where(k => !upisaniIds.Contains(k.KandidatId) && k.Aktivan).ToList();
            }
            else
            {
                Result = svi.Where(k => upisaniIds.Contains(k.KandidatId)).ToList();
            }
        }
    }
}