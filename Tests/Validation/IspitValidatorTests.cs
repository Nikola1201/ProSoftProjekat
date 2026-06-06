using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class IspitValidatorTests
    {
        private readonly IspitValidator _v = new();
        private static Ispit Valid() => SampleData.ValidIspit();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_UpisId_not_positive() { var e = Valid(); e.UpisId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_DatumIspita_default() { var e = Valid(); e.DatumIspita = default; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Tip_empty() { var e = Valid(); e.Tip = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Tip_not_allowed() { var e = Valid(); e.Tip = "usmeni"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Rezultat_empty() { var e = Valid(); e.Rezultat = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Rezultat_not_allowed() { var e = Valid(); e.Rezultat = "nije polozio"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Napomena_too_long() { var e = Valid(); e.Napomena = new string('n', 201); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
