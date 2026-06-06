using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="PaketObuke"/> prema dozvoljenim vrednostima atributa.</summary>
    public class PaketObukeValidator : IValidator<PaketObuke>
    {
        /// <inheritdoc/>
        public void Validate(PaketObuke p)
        {
            Guard.NijeNull(p, "Paket obuke");
            Guard.Obavezno(p.Naziv, "Naziv");   Guard.MaxDuzina(p.Naziv, 50, "Naziv");
            Guard.NijeNull(p.Kategorija, "Kategorija");
            Guard.Opseg(p.BrojCasova, 1, 200, "Broj časova");
            Guard.Pozitivan(p.Cena, "Cena");
            Guard.MaxDuzina(p.Opis, 500, "Opis");
        }
    }
}
