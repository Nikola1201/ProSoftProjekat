using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class PaketObukeValidatorTests
    {
        private readonly PaketObukeValidator _v = new();
        private static PaketObuke Valid() => SampleData.ValidPaketObuke();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_Naziv_empty() { var e = Valid(); e.Naziv = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Naziv_too_long() { var e = Valid(); e.Naziv = new string('a', 51); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Kategorija_null() { var e = Valid(); e.Kategorija = null!; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_BrojCasova_below_min() { var e = Valid(); e.BrojCasova = 0; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_BrojCasova_above_max() { var e = Valid(); e.BrojCasova = 201; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Cena_zero() { var e = Valid(); e.Cena = 0m; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Cena_negative() { var e = Valid(); e.Cena = -1m; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Opis_too_long() { var e = Valid(); e.Opis = new string('o', 501); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
