using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="Admin"/> prema dozvoljenim vrednostima atributa.</summary>
    public class AdminValidator : IValidator<Admin>
    {
        /// <inheritdoc/>
        public void Validate(Admin a)
        {
            Guard.NijeNull(a, "Admin");
            Guard.Obavezno(a.Ime, "Ime");           Guard.MaxDuzina(a.Ime, 50, "Ime");
            Guard.Obavezno(a.Prezime, "Prezime");   Guard.MaxDuzina(a.Prezime, 50, "Prezime");
            Guard.StringDuzina(a.Username, 3, 30, "Korisničko ime");
            Guard.BezRazmaka(a.Username, "Korisničko ime");
            Guard.StringDuzina(a.Lozinka, 6, 100, "Lozinka");
            Guard.Email(a.Email, "Email");
            Guard.Datum(a.DatumKreiranja, "Datum kreiranja");
        }
    }
}
