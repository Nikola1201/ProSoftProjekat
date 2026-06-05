using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class PlacanjeValidatorTests
    {
        private readonly PlacanjeValidator _v = new();
        private static Placanje Valid() => SampleData.ValidPlacanje();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_UpisId_not_positive() { var e = Valid(); e.UpisId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Iznos_zero() { var e = Valid(); e.Iznos = 0m; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Iznos_negative() { var e = Valid(); e.Iznos = -100m; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_DatumPlacanja_default() { var e = Valid(); e.DatumPlacanja = default; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_NacinPlacanja_empty() { var e = Valid(); e.NacinPlacanja = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_NacinPlacanja_not_allowed() { var e = Valid(); e.NacinPlacanja = "ceka"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Napomena_too_long() { var e = Valid(); e.Napomena = new string('n', 201); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
