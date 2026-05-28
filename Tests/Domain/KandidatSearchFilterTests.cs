using Common.Domain;
using Xunit;

namespace Tests.Domain
{
    public class KandidatSearchFilterTests
    {
        [Fact]
        public void Defaults_are_null_or_false()
        {
            var f = new KandidatSearchFilter();
            Assert.Null(f.Ime);
            Assert.Null(f.Prezime);
            Assert.Null(f.JMBG);
            Assert.Null(f.Email);
            Assert.False(f.SamoAktivni);
        }

        [Fact]
        public void Properties_set_and_get_round_trip()
        {
            var f = new KandidatSearchFilter
            {
                Ime = "Mika",
                Prezime = "Mikic",
                JMBG = "1234567890123",
                Email = "mika@example.com",
                SamoAktivni = true
            };

            Assert.Equal("Mika", f.Ime);
            Assert.Equal("Mikic", f.Prezime);
            Assert.Equal("1234567890123", f.JMBG);
            Assert.Equal("mika@example.com", f.Email);
            Assert.True(f.SamoAktivni);
        }
    }
}
