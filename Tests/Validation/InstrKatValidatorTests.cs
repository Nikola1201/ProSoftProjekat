using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class InstrKatValidatorTests
    {
        private readonly InstrKatValidator _v = new();
        private static InstrKat Valid() => SampleData.ValidInstrKat();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_InstruktorId_not_positive() { var e = Valid(); e.InstruktorId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_KategorijaID_not_positive() { var e = Valid(); e.KategorijaID = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_DatumDodele_default() { var e = Valid(); e.DatumDodele = default; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
