using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class UpisValidatorTests
    {
        private readonly UpisValidator _v = new();
        private static Upis Valid() => SampleData.ValidUpis();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_KandidatId_not_positive() { var e = Valid(); e.KandidatId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_PaketId_not_positive() { var e = Valid(); e.PaketId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_DatumUpisa_default() { var e = Valid(); e.DatumUpisa = default; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Status_empty() { var e = Valid(); e.Status = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Status_not_allowed() { var e = Valid(); e.Status = "nepoznato"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
