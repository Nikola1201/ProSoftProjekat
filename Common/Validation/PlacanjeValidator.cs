using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Placanje"/> prema dozvoljenim vrednostima atributa.</summary>
    public class PlacanjeValidator : IValidator<Placanje>
    {
        /// <inheritdoc/>
        public void Validate(Placanje p)
        {
            Guard.NijeNull(p, "Plaćanje");
            Guard.Pozitivan(p.UpisId, "Upis");
            Guard.Pozitivan(p.Iznos, "Iznos");
            Guard.Datum(p.DatumPlacanja, "Datum plaćanja");
            Guard.JednaOd(p.NacinPlacanja, "Način plaćanja", "gotovina", "kartica", "transfer");
            Guard.MaxDuzina(p.Napomena, 200, "Napomena");
        }
    }
}
