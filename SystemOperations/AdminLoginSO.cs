using Common.Domain;
using DBBroker;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za autentifikaciju administratora.
    /// Pronalazi admina na osnovu kredencijala (korisničko ime i lozinka).
    /// </summary>
    public class AdminLoginSO : SystemOperationBase
    {
        private readonly Admin _admin;

        /// <summary>Pronađeni admin nakon uspešne autentifikacije; <see langword="null"/> ako admin nije pronađen.</summary>
        public IEntity Result { get; set; }

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="argument">Admin objekat sa kredencijalima za proveru.</param>
        public AdminLoginSO(Admin argument) : this(argument, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="argument">Admin objekat sa kredencijalima za proveru.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public AdminLoginSO(Admin argument, IBroker? broker) : base(broker)
        {
            _admin = argument;
        }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetEntityByQuery(_admin);
        }
    }
}