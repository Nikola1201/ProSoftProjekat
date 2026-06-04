using System;
using Common.Domain;
using Common.Validation;
using Tests.Helpers;
using Xunit;

namespace Tests.Validation
{
    public class AdminValidatorTests
    {
        private readonly AdminValidator _v = new();
        private static Admin Valid() => SampleData.ValidAdmin();

        [Fact] public void Valid_passes() => _v.Validate(Valid());

        [Fact] public void Throws_when_Ime_empty() { var e = Valid(); e.Ime = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Ime_too_long() { var e = Valid(); e.Ime = new string('a', 51); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Prezime_empty() { var e = Valid(); e.Prezime = ""; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Prezime_too_long() { var e = Valid(); e.Prezime = new string('b', 51); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Username_too_short() { var e = Valid(); e.Username = "ab"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Username_too_long() { var e = Valid(); e.Username = new string('u', 31); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Username_has_space() { var e = Valid(); e.Username = "pe ra"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Lozinka_too_short() { var e = Valid(); e.Lozinka = "12345"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Lozinka_too_long() { var e = Valid(); e.Lozinka = new string('p', 101); Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_Email_invalid() { var e = Valid(); e.Email = "nije-email"; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
        [Fact] public void Throws_when_DatumKreiranja_default() { var e = Valid(); e.DatumKreiranja = default; Assert.Throws<ValidacijaException>(() => _v.Validate(e)); }
    }
}
