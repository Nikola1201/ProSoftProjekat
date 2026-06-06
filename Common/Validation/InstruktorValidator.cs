using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Instruktor"/> prema dozvoljenim vrednostima atributa.</summary>
    public class InstruktorValidator : IValidator<Instruktor>
    {
        /// <inheritdoc/>
        public void Validate(Instruktor i)
        {
            Guard.NijeNull(i, "Instruktor");
            Guard.Obavezno(i.Ime, "Ime");           Guard.MaxDuzina(i.Ime, 50, "Ime");
            Guard.Obavezno(i.Prezime, "Prezime");   Guard.MaxDuzina(i.Prezime, 50, "Prezime");
            Guard.TacanBrojCifara(i.JMBG, 13, "JMBG");
            Guard.Telefon(i.Telefon, "Telefon");
            Guard.Email(i.Email, "Email");
            Guard.Datum(i.DatumZaposlenja, "Datum zaposlenja");
        }
    }
}
