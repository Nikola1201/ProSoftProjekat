using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Kategorija"/> prema dozvoljenim vrednostima atributa.</summary>
    public class KategorijaValidator : IValidator<Kategorija>
    {
        /// <inheritdoc/>
        public void Validate(Kategorija k)
        {
            Guard.NijeNull(k, "Kategorija");
            Guard.Obavezno(k.NazivKategorije, "Naziv kategorije");
            Guard.MaxDuzina(k.NazivKategorije, 2, "Naziv kategorije");
        }
    }
}
