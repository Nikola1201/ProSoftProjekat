using System;
using System.Collections.Generic;
using Common.Domain;
using Common.DTO;
using Common.Validation;
using DBBroker;
using Moq;
using SystemOperations;
using Tests.Helpers;
using Xunit;

namespace Tests.SystemOps
{
    public class KreirajInstruktoraSOTests
    {
        /// <summary>
        /// Returns a broker mock ready for a successful KreirajInstruktora run:
        /// - GetAll returns no existing instruktori (no duplicates)
        /// - GetEntityByID for Kategorija returns a valid kategorija
        /// - GetEntityByQuery returns the saved instruktor
        /// </summary>
        private static Mock<IBroker> HappyBroker(Instruktor saved)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.IsAny<Kategorija>()))
                  .Returns(SampleData.ValidKategorija());
            broker.Setup(b => b.GetEntityByQuery(It.IsAny<Instruktor>()))
                  .Returns(saved);
            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(saved);
            return broker;
        }

        // ─── Happy path ────────────────────────────────────────────────────────────

        [Fact]
        public void Persists_instruktor_and_instrkat_when_valid()
        {
            var instruktor = SampleData.ValidInstruktor();
            var broker = HappyBroker(instruktor);

            var request = new KreirajInstruktoraRequest
            {
                Instruktor = instruktor,
                KategorijaID = SampleData.ValidKategorija().KategorijaID
            };

            var so = new KreirajInstruktoraSO(request, broker.Object);
            so.ExecuteTemplate();

            // Add called at least twice: once for Instruktor, once for InstrKat
            broker.Verify(b => b.Add(instruktor), Times.Once);
            broker.Verify(b => b.Add(It.IsAny<InstrKat>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_set_to_saved_instruktor()
        {
            var instruktor = SampleData.ValidInstruktor();
            var broker = HappyBroker(instruktor);

            var request = new KreirajInstruktoraRequest
            {
                Instruktor = instruktor,
                KategorijaID = SampleData.ValidKategorija().KategorijaID
            };

            var so = new KreirajInstruktoraSO(request, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(instruktor, so.Result);
        }

        [Fact]
        public void Persists_when_DatumZaposlenja_is_today()
        {
            var instruktor = SampleData.ValidInstruktor();
            instruktor.DatumZaposlenja = DateTime.Now.Date;
            var broker = HappyBroker(instruktor);

            var request = new KreirajInstruktoraRequest
            {
                Instruktor = instruktor,
                KategorijaID = SampleData.ValidKategorija().KategorijaID
            };

            var so = new KreirajInstruktoraSO(request, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(instruktor), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
        }

        // ─── Null request ──────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_request_null()
        {
            var broker = new Mock<IBroker>();

            var so = new KreirajInstruktoraSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Null instruktor ───────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_instruktor_null()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var request = new KreirajInstruktoraRequest
            {
                Instruktor = null!,
                KategorijaID = 1
            };

            var so = new KreirajInstruktoraSO(request, broker.Object);

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
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            instruktor.Ime = ime!;

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

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
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            instruktor.Prezime = prezime!;

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

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
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            instruktor.JMBG = jmbg!;

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── JMBG format ───────────────────────────────────────────────────────────

        [Theory]
        [InlineData("123456789")]        // too short
        [InlineData("12345678901234")]   // too long
        [InlineData("123456789012A")]    // contains letter
        [InlineData("ABCDEFGHIJKLM")]   // all letters
        public void Throws_when_JMBG_format_invalid(string jmbg)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            instruktor.JMBG = jmbg;

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

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
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            instruktor.Telefon = telefon!;

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

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
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            instruktor.Email = email!;

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumZaposlenja in future ─────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumZaposlenja_in_future()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            instruktor.DatumZaposlenja = DateTime.Now.Date.AddDays(1);

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Duplicate JMBG ────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_JMBG_already_exists()
        {
            var existing = SampleData.ValidInstruktor();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>()))
                  .Returns(new List<IEntity> { existing });

            var instruktor = SampleData.ValidInstruktor();
            instruktor.InstruktorId = 99;
            instruktor.Email = "other@auto.rs";
            // Same JMBG as existing

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Duplicate Email (case-insensitive) ────────────────────────────────────

        [Theory]
        [InlineData("zika@auto.rs")]
        [InlineData("ZIKA@AUTO.RS")]
        [InlineData("Zika@Auto.Rs")]
        public void Throws_when_Email_already_exists(string conflictingEmail)
        {
            var existing = SampleData.ValidInstruktor(); // email = "zika@auto.rs"
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>()))
                  .Returns(new List<IEntity> { existing });

            var instruktor = SampleData.ValidInstruktor();
            instruktor.InstruktorId = 99;
            instruktor.JMBG = "1111111111111"; // unique JMBG so we reach email check
            instruktor.Email = conflictingEmail;

            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 1 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── KategorijaID invalid (≤ 0) ────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Throws_when_kategorijaId_invalid(int kategorijaId)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());

            var instruktor = SampleData.ValidInstruktor();
            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = kategorijaId };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Kategorija not found ──────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kategorija_not_found()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.IsAny<Kategorija>())).Returns((IEntity)null!);

            var instruktor = SampleData.ValidInstruktor();
            var request = new KreirajInstruktoraRequest { Instruktor = instruktor, KategorijaID = 999 };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Rollback on broker failure ────────────────────────────────────────────

        [Fact]
        public void Rollback_when_add_throws()
        {
            var instruktor = SampleData.ValidInstruktor();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Instruktor>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.IsAny<Kategorija>()))
                  .Returns(SampleData.ValidKategorija());
            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Throws(new Exception("DB error"));

            var request = new KreirajInstruktoraRequest
            {
                Instruktor = instruktor,
                KategorijaID = SampleData.ValidKategorija().KategorijaID
            };
            var so = new KreirajInstruktoraSO(request, broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
