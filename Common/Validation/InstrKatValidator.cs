using Common.Domain;

namespace Common.Validation
{
    /// <summary>Validira <see cref="InstrKat"/> prema dozvoljenim vrednostima atributa.</summary>
    public class InstrKatValidator : IValidator<InstrKat>
    {
        /// <inheritdoc/>
        public void Validate(InstrKat ik)
        {
            Guard.NijeNull(ik, "Instruktor-kategorija");
            Guard.Pozitivan(ik.InstruktorId, "Instruktor");
            Guard.Pozitivan(ik.KategorijaID, "Kategorija");
            Guard.Datum(ik.DatumDodele, "Datum dodele");
        }
    }
}
