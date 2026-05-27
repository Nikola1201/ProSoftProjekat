using System;
using System.Collections.Generic;
using Common.Domain;
using Common.Validation;
using DBBroker;
using Moq;
using SystemOperations;
using Tests.Helpers;
using Xunit;

namespace Tests.SystemOps
{
    public class KreirajKandidataSOTests
    {
        /// <summary>
        /// Returns a broker mock that reports no existing kandidati.
        /// </summary>
        private static Mock<IBroker> NoExistingKandidati()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>())).Returns(new List<IEntity>());
            return broker;
        }

        // ─── Happy path ────────────────────────────────────────────────────────────

        [Fact]
        public void Persists_kandidat_when_valid()
        {
            var broker = NoExistingKandidati();
            var kandidat = SampleData.ValidKandidat();
            broker.Setup(b => b.Add(kandidat)).Returns(kandidat);

            var so = new KreirajKandidataSO(kandidat, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(kandidat), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
        }

        // ─── Null kandidat ─────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_null()
        {
            var broker = NoExistingKandidati();

            var so = new KreirajKandidataSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Blank Ime ─────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_when_Ime_blank(string? ime)
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.Ime = ime!;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Blank Prezime ─────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_when_Prezime_blank(string? prezime)
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.Prezime = prezime!;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Blank JMBG ────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_when_JMBG_blank(string? jmbg)
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.JMBG = jmbg!;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── JMBG format (not 13 digits / contains non-digit) ─────────────────────

        [Theory]
        [InlineData("123456789")]          // too short (9 chars)
        [InlineData("12345678901234")]     // too long (14 chars)
        [InlineData("123456789012A")]      // 13 chars but last is letter
        [InlineData("ABCDEFGHIJKLM")]      // 13 chars, all letters
        public void Throws_when_JMBG_format_invalid(string jmbg)
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.JMBG = jmbg;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Blank Telefon ─────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_when_Telefon_blank(string? telefon)
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.Telefon = telefon!;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Blank Email ───────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_when_Email_blank(string? email)
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.Email = email!;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Blank Adresa ──────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_when_Adresa_blank(string? adresa)
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.Adresa = adresa!;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumUpisa in future ──────────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumUpisa_in_future()
        {
            var broker = NoExistingKandidati();
            var k = SampleData.ValidKandidat();
            k.DatumUpisa = DateTime.Now.Date.AddDays(1);

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Duplicate JMBG ────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_JMBG_already_exists()
        {
            var existing = SampleData.ValidKandidat();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Returns(new List<IEntity> { existing });

            var k = SampleData.ValidKandidat();
            k.KandidatId = 99; // different record, same JMBG
            k.Email = "other@example.com";

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Duplicate Email (case-insensitive) ────────────────────────────────────

        [Theory]
        [InlineData("mika@example.com")]   // exact match
        [InlineData("MIKA@EXAMPLE.COM")]   // upper-case
        [InlineData("Mika@Example.Com")]   // mixed case
        public void Throws_when_Email_already_exists(string conflictingEmail)
        {
            var existing = SampleData.ValidKandidat(); // email = "mika@example.com"
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Returns(new List<IEntity> { existing });

            var k = SampleData.ValidKandidat();
            k.KandidatId = 99;                       // different record
            k.JMBG = "9999999999999";                // unique JMBG so we reach email check
            k.Email = conflictingEmail;

            var so = new KreirajKandidataSO(k, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }
    }
}
