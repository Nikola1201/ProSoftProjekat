using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class VoziloValidatorTests
    {
        private readonly VoziloValidator _v = new();
        private static Vozilo Valid() => SampleData.ValidVozilo();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_Marka_empty() { var e = Valid(); e.Marka = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Marka_too_long() { var e = Valid(); e.Marka = new string('a', 31); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Model_empty() { var e = Valid(); e.Model = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Model_too_long() { var e = Valid(); e.Model = new string('b', 31); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Godiste_too_old() { var e = Valid(); e.Godiste = 1949; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Godiste_too_new() { var e = Valid(); e.Godiste = 2101; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Tablica_empty() { var e = Valid(); e.Tablica = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Tablica_too_long() { var e = Valid(); e.Tablica = new string('t', 16); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_KategorijaID_not_positive() { var e = Valid(); e.KategorijaID = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
