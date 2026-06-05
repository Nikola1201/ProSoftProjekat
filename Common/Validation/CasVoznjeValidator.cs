using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="CasVoznje"/> prema dozvoljenim vrednostima atributa.</summary>
    public class CasVoznjeValidator : IValidator<CasVoznje>
    {
        /// <inheritdoc/>
        public void Validate(CasVoznje c)
        {
            Guard.NijeNull(c, "Čas vožnje");
            Guard.Pozitivan(c.UpisId, "Upis");
            Guard.Pozitivan(c.InstruktorId, "Instruktor");
            Guard.Pozitivan(c.VoziloId, "Vozilo");
            Guard.Datum(c.DatumCas, "Datum časa");
            Guard.Opseg(c.TrajanjMin, 1, 600, "Trajanje (min)");
            Guard.JednaOd(c.Status, "Status", "zakazan", "odrzan", "otkazan");
            Guard.MaxDuzina(c.Napomena, 200, "Napomena");
        }
    }
}
