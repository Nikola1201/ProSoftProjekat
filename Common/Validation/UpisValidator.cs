using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Upis"/> prema dozvoljenim vrednostima atributa.</summary>
    public class UpisValidator : IValidator<Upis>
    {
        /// <inheritdoc/>
        public void Validate(Upis u)
        {
            Guard.NijeNull(u, "Upis");
            Guard.Pozitivan(u.KandidatId, "Kandidat");
            Guard.Pozitivan(u.PaketId, "Paket obuke");
            Guard.Datum(u.DatumUpisa, "Datum upisa");
            Guard.JednaOd(u.Status, "Status", "aktivan", "polozio", "pao", "odustao");
        }
    }
}
