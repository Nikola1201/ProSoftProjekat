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
    public class ZakaziCasVoznjeSOTests
    {
        // ─── Helpers ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Builds a CasVoznje that satisfies all field-level validation
        /// (UpisId, InstruktorId, VoziloId, TrajanjMin, DatumCas all valid).
        /// DatumCas is set to far future so it cannot be "in the past".
        /// </summary>
        private static CasVoznje ValidCas() => new CasVoznje
        {
            CasId     = 0,
            UpisId    = SampleData.ValidUpis().UpisId,
            InstruktorId = SampleData.ValidInstruktor().InstruktorId,
            VoziloId  = SampleData.ValidVozilo().VoziloId,
            DatumCas  = DateTime.Now.AddDays(2),
            TrajanjMin = 45,
            Status    = "",
            Napomena  = ""
        };

        /// <summary>
        /// Returns a fully-wired happy-path broker:
        /// - GetEntityByID(Upis)       → active upis
        /// - GetEntityByID(Instruktor) → active instruktor
        /// - GetEntityByID(Vozilo)     → active vozilo with KategorijaID = 1
        /// - GetEntityByID(InstrKat)   → active instruktor-kategorija link
        /// - GetAll(CasVoznje)         → empty list (no conflicts)
        /// - Add(CasVoznje)            → returns the saved cas
        /// </summary>
        private static Mock<IBroker> HappyBroker(CasVoznje saved)
        {
            var broker = new Mock<IBroker>();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns(SampleData.ValidVozilo());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is InstrKat)))
                  .Returns(SampleData.ValidInstrKat());

            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity>());

            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Returns(saved);

            return broker;
        }

        // ─── Happy path ────────────────────────────────────────────────────────────

        [Fact]
        public void Persists_cas_and_commits_when_valid()
        {
            var cas    = ValidCas();
            var broker = HappyBroker(cas);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(cas), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_set_after_persist()
        {
            var cas    = ValidCas();
            var broker = HappyBroker(cas);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
        }

        [Fact]
        public void Status_set_to_zakazan_before_add()
        {
            var cas    = ValidCas();
            cas.Status = "";
            var broker = HappyBroker(cas);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);
            so.ExecuteTemplate();

            // Validate() sets cas.Status = "zakazan" before broker.Add
            broker.Verify(b => b.Add(It.Is<CasVoznje>(c => c.Status == "zakazan")), Times.Once);
        }

        [Fact]
        public void Blank_napomena_normalized_to_empty_string()
        {
            var cas      = ValidCas();
            cas.Napomena = "   ";
            var broker   = HappyBroker(cas);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.Is<CasVoznje>(c => c.Napomena == string.Empty)), Times.Once);
        }

        [Fact]
        public void Non_empty_napomena_trimmed_before_add()
        {
            var cas      = ValidCas();
            cas.Napomena = "  proba  ";
            var broker   = HappyBroker(cas);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.Is<CasVoznje>(c => c.Napomena == "proba")), Times.Once);
        }

        // ─── Null cas ──────────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_cas_null()
        {
            var broker = new Mock<IBroker>();

            var so = new ZakaziCasVoznjeSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── UpisId <= 0 ───────────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Throws_when_UpisId_invalid(int upisId)
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();
            cas.UpisId = upisId;

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── InstruktorId <= 0 ─────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Throws_when_InstruktorId_invalid(int instruktorId)
        {
            var broker        = new Mock<IBroker>();
            var cas           = ValidCas();
            cas.InstruktorId  = instruktorId;

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── VoziloId <= 0 ─────────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public void Throws_when_VoziloId_invalid(int voziloId)
        {
            var broker    = new Mock<IBroker>();
            var cas       = ValidCas();
            cas.VoziloId  = voziloId;

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── TrajanjMin <= 0 ───────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Throws_when_TrajanjMin_not_positive(int trajanje)
        {
            var broker       = new Mock<IBroker>();
            var cas          = ValidCas();
            cas.TrajanjMin   = trajanje;

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumCas in the past ──────────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumCas_in_past()
        {
            var broker   = new Mock<IBroker>();
            var cas      = ValidCas();
            cas.DatumCas = DateTime.Now.AddMinutes(-1);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Upis not found / not active ──────────────────────────────────────────

        [Fact]
        public void Throws_when_upis_not_found()
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns((IEntity)null!);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Theory]
        [InlineData("polozio")]
        [InlineData("pao")]
        [InlineData("odustao")]
        public void Throws_when_upis_not_aktivan(string status)
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            var inactiveUpis = SampleData.ValidUpis();
            inactiveUpis.Status = status;

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(inactiveUpis);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Instruktor not found / not active ────────────────────────────────────

        [Fact]
        public void Throws_when_instruktor_not_found()
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns((IEntity)null!);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Throws_when_instruktor_not_aktivan()
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            var inactiveInstruktor   = SampleData.ValidInstruktor();
            inactiveInstruktor.Aktivan = false;
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(inactiveInstruktor);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Vozilo not found / not active ────────────────────────────────────────

        [Fact]
        public void Throws_when_vozilo_not_found()
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns((IEntity)null!);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Throws_when_vozilo_not_aktivno()
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            var inactiveVozilo    = SampleData.ValidVozilo();
            inactiveVozilo.Aktivno = false;
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns(inactiveVozilo);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Instruktor not authorized for vozilo's kategorija ────────────────────

        [Fact]
        public void Throws_when_instrkat_not_found()
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns(SampleData.ValidVozilo());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is InstrKat)))
                  .Returns((IEntity)null!);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Throws_when_instrkat_not_aktivno()
        {
            var broker = new Mock<IBroker>();
            var cas    = ValidCas();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns(SampleData.ValidVozilo());

            var inactiveInstrKat   = SampleData.ValidInstrKat();
            inactiveInstrKat.Aktivno = false;
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is InstrKat)))
                  .Returns(inactiveInstrKat);

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Termin conflict: instruktor ───────────────────────────────────────────

        [Fact]
        public void Throws_when_instruktor_already_booked_in_termin()
        {
            var cas     = ValidCas();
            var broker  = new Mock<IBroker>();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns(SampleData.ValidVozilo());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is InstrKat)))
                  .Returns(SampleData.ValidInstrKat());

            // Existing cas: same instruktor, overlapping time window
            var conflict = new CasVoznje
            {
                CasId        = 99,
                InstruktorId = cas.InstruktorId,
                VoziloId     = 999,            // different vozilo — instruktor is the conflict
                DatumCas     = cas.DatumCas.AddMinutes(-10),
                TrajanjMin   = 30,             // end = DatumCas+20 → overlaps with new cas
                Status       = "zakazan"
            };

            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { conflict });

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void No_conflict_when_existing_cas_is_otkazan()
        {
            var cas    = ValidCas();
            var broker = HappyBroker(cas);

            // Otkazan cas that would overlap — should be ignored
            var otkazan = new CasVoznje
            {
                CasId        = 99,
                InstruktorId = cas.InstruktorId,
                VoziloId     = cas.VoziloId,
                DatumCas     = cas.DatumCas.AddMinutes(-10),
                TrajanjMin   = 30,
                Status       = "otkazan"
            };

            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { otkazan });

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.IsAny<CasVoznje>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
        }

        // ─── Termin conflict: vozilo ───────────────────────────────────────────────

        [Fact]
        public void Throws_when_vozilo_already_booked_in_termin()
        {
            var cas    = ValidCas();
            var broker = new Mock<IBroker>();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns(SampleData.ValidVozilo());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is InstrKat)))
                  .Returns(SampleData.ValidInstrKat());

            // Existing cas: different instruktor, same vozilo, overlapping time
            var conflict = new CasVoznje
            {
                CasId        = 88,
                InstruktorId = 999,            // different instruktor
                VoziloId     = cas.VoziloId,
                DatumCas     = cas.DatumCas.AddMinutes(10),
                TrajanjMin   = 60,             // starts inside the new cas window
                Status       = "zakazan"
            };

            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { conflict });

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Rollback on broker failure ────────────────────────────────────────────

        [Fact]
        public void Rollback_when_add_throws()
        {
            var cas    = ValidCas();
            var broker = new Mock<IBroker>();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Instruktor)))
                  .Returns(SampleData.ValidInstruktor());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Vozilo)))
                  .Returns(SampleData.ValidVozilo());

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is InstrKat)))
                  .Returns(SampleData.ValidInstrKat());

            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity>());

            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Throws(new Exception("DB error"));

            var so = new ZakaziCasVoznjeSO(cas, broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
