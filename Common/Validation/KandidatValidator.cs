using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Kandidat"/> prema dozvoljenim vrednostima atributa.</summary>
    public class KandidatValidator : IValidator<Kandidat>
    {
        /// <inheritdoc/>
        public void Validate(Kandidat k)
        {
            Guard.NijeNull(k, "Kandidat");
            Guard.Obavezno(k.Ime, "Ime");           Guard.MaxDuzina(k.Ime, 50, "Ime");
            Guard.Obavezno(k.Prezime, "Prezime");   Guard.MaxDuzina(k.Prezime, 50, "Prezime");
            Guard.TacanBrojCifara(k.JMBG, 13, "JMBG");
            Guard.Telefon(k.Telefon, "Telefon");
            Guard.Email(k.Email, "Email");
            Guard.Obavezno(k.Adresa, "Adresa");     Guard.MaxDuzina(k.Adresa, 100, "Adresa");
            Guard.Datum(k.DatumUpisa, "Datum upisa");
        }
    }
}
