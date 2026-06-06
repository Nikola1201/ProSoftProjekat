using System;
using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class KandidatValidatorTests
    {
        private readonly KandidatValidator _v = new();
        private static Kandidat Valid() => SampleData.ValidKandidat();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_Ime_empty() { var e = Valid(); e.Ime = "   "; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Ime_too_long() { var e = Valid(); e.Ime = new string('a', 51); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Prezime_empty() { var e = Valid(); e.Prezime = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Prezime_too_long() { var e = Valid(); e.Prezime = new string('b', 51); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_JMBG_wrong_length() { var e = Valid(); e.JMBG = "123"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_JMBG_not_numeric() { var e = Valid(); e.JMBG = "12345678901AB"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Telefon_too_short() { var e = Valid(); e.Telefon = "123"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Telefon_has_letters() { var e = Valid(); e.Telefon = "06abc123"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Email_invalid() { var e = Valid(); e.Email = "nije-email"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Adresa_empty() { var e = Valid(); e.Adresa = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Adresa_too_long() { var e = Valid(); e.Adresa = new string('x', 101); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_DatumUpisa_default() { var e = Valid(); e.DatumUpisa = default; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
