using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class KategorijaValidatorTests
    {
        private readonly KategorijaValidator _v = new();
        private static Kategorija Valid() => SampleData.ValidKategorija();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_Naziv_empty() { var e = Valid(); e.NazivKategorije = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }

        [Fact] public void Valid_two_chars_passes() { var e = Valid(); e.NazivKategorije = "B1"; _v.Validate(e); }
    }
}
