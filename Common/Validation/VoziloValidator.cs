using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Vozilo"/> prema dozvoljenim vrednostima atributa.</summary>
    public class VoziloValidator : IValidator<Vozilo>
    {
        /// <inheritdoc/>
        public void Validate(Vozilo v)
        {
            Guard.NijeNull(v, "Vozilo");
            Guard.Obavezno(v.Marka, "Marka");   Guard.MaxDuzina(v.Marka, 30, "Marka");
            Guard.Obavezno(v.Model, "Model");   Guard.MaxDuzina(v.Model, 30, "Model");
            Guard.Opseg(v.Godiste, 1950, 2100, "Godište");
            Guard.Obavezno(v.Tablica, "Tablica"); Guard.MaxDuzina(v.Tablica, 15, "Tablica");
            Guard.Pozitivan(v.KategorijaID, "Kategorija");
        }
    }
}
