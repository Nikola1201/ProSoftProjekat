namespace Common.Validation
{
    /// <summary>
    /// Validira instancu domenske klase <typeparamref name="T"/>.
    /// Baca <see cref="ValidacijaException"/> na prvi prekršeni uslov.
    /// </summary>
    /// <typeparam name="T">Tip entiteta koji se validira.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>Proverava sva ograničenja atributa entiteta.</summary>
        /// <param name="entitet">Entitet koji se validira.</param>
        /// <exception cref="ValidacijaException">Ako neki uslov nije ispunjen.</exception>
        void Validate(T entitet);
    }
}
