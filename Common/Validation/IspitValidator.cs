using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Ispit"/> prema dozvoljenim vrednostima atributa.</summary>
    public class IspitValidator : IValidator<Ispit>
    {
        /// <inheritdoc/>
        public void Validate(Ispit i)
        {
            Guard.NijeNull(i, "Ispit");
            Guard.Pozitivan(i.UpisId, "Upis");
            Guard.Datum(i.DatumIspita, "Datum ispita");
            Guard.JednaOd(i.Tip, "Tip", "teorijski", "prakticni");
            Guard.JednaOd(i.Rezultat, "Rezultat", "polozio", "pao");
            Guard.MaxDuzina(i.Napomena, 200, "Napomena");
        }
    }
}
