using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class CasVoznjeValidatorTests
    {
        private readonly CasVoznjeValidator _v = new();
        private static CasVoznje Valid() => SampleData.ValidCasVoznje();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_UpisId_not_positive() { var e = Valid(); e.UpisId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_InstruktorId_not_positive() { var e = Valid(); e.InstruktorId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_VoziloId_not_positive() { var e = Valid(); e.VoziloId = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_DatumCas_default() { var e = Valid(); e.DatumCas = default; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_TrajanjMin_below_min() { var e = Valid(); e.TrajanjMin = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_TrajanjMin_above_max() { var e = Valid(); e.TrajanjMin = 601; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Status_empty() { var e = Valid(); e.Status = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Status_not_allowed() { var e = Valid(); e.Status = "nepoznato"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Napomena_too_long() { var e = Valid(); e.Napomena = new string('n', 201); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
