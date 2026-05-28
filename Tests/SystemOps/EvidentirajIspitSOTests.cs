using System;
using System.Collections.Generic;
using Common.Domain;
using Common.DTO.Izvestaji;
using Common.Validation;
using DBBroker;
using Moq;
using SystemOperations;
using Tests.Helpers;
using Xunit;

namespace Tests.SystemOps
{
    public class EvidentirajIspitSOTests
    {
        // ─── Helpers ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns a valid request for a teorijski ispit on yesterday's date.
        /// </summary>
        private static EvidentirajIspitRequest ValidRequest() => new EvidentirajIspitRequest
        {
            KandidatId  = 1,
            DatumIspita = DateTime.Now.Date.AddDays(-1),
            Tip         = "teorijski",
            Rezultat    = "polozio",
            Napomena    = ""
        };

        /// <summary>
        /// Returns a broker mock ready for a fully successful EvidentirajIspit run:
        /// - GetEntityByID(Kandidat)   → valid kandidat
        /// - GetEntitiesByQuery(Upis)  → one aktivan upis
        /// - GetEntitiesByQuery(Ispit) → empty list (no prior ispiti)
        /// - Add(Ispit)                → returns the new ispit
        /// - Update(Upis)              → no-op
        /// </summary>
        private static Mock<IBroker> HappyBroker()
        {
            var broker = new Mock<IBroker>();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());

            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });

            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity>());

            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Returns(SampleData.ValidIspit());

            return broker;
        }

        // ─── Happy path ─────────────────────────────────────────────────────────────

        [Fact]
        public void Persists_ispit_and_commits_when_valid()
        {
            var broker = HappyBroker();
            var so = new EvidentirajIspitSO(ValidRequest(), broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.IsAny<Ispit>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_set_after_persist()
        {
            var broker = HappyBroker();
            var so = new EvidentirajIspitSO(ValidRequest(), broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
            Assert.NotNull(so.Result.Ispit);
            Assert.Equal("Ispit je uspesno evidentiran.", so.Result.Poruka);
        }

        [Fact]
        public void Updates_upis_status_to_polozio_when_both_tipovi_passed()
        {
            // Existing prakticni polozio ispit — new teorijski polozio should flip upis status
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());

            var upis = SampleData.ValidUpis(); // status = "aktivan"
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { upis });

            var existingPrakticni = new Ispit
            {
                IspitId   = 10,
                UpisId    = upis.UpisId,
                Tip       = "prakticni",
                Rezultat  = "polozio",
                DatumIspita = DateTime.Now.Date.AddDays(-5),
                Napomena  = ""
            };
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity> { existingPrakticni });

            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(SampleData.ValidIspit());

            var request = ValidRequest(); // teorijski polozio
            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Update(It.Is<Upis>(u => u.Status == "polozio")), Times.Once);
            Assert.True(so.Result.StatusPromenjen);
            Assert.Equal("polozio", so.Result.UpisStatus);
        }

        [Fact]
        public void Does_not_update_upis_when_only_one_tip_passed()
        {
            // No prior ispiti — teorijski polozio alone should NOT flip upis status
            var broker = HappyBroker(); // empty ispiti list
            var so = new EvidentirajIspitSO(ValidRequest(), broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Update(It.IsAny<Upis>()), Times.Never);
            Assert.False(so.Result.StatusPromenjen);
        }

        [Fact]
        public void Does_not_update_upis_when_upis_already_polozio()
        {
            // Upis already polozio — even if both tipovi polozio, no duplicate Update
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());

            var upis = SampleData.ValidUpis();
            upis.Status = "polozio";
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { upis });

            var existingPrakticni = new Ispit
            {
                IspitId   = 10,
                UpisId    = upis.UpisId,
                Tip       = "prakticni",
                Rezultat  = "polozio",
                DatumIspita = DateTime.Now.Date.AddDays(-5),
                Napomena  = ""
            };
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity> { existingPrakticni });

            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(SampleData.ValidIspit());

            var request = ValidRequest(); // teorijski polozio
            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Update(It.IsAny<Upis>()), Times.Never);
            Assert.False(so.Result.StatusPromenjen);
        }

        // ─── Null request ────────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_request_null()
        {
            var broker = new Mock<IBroker>();
            var so = new EvidentirajIspitSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── KandidatId <= 0 ─────────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Throws_when_KandidatId_invalid(int kandidatId)
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.KandidatId = kandidatId;

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumIspita == MinValue ──────────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumIspita_min_value()
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.DatumIspita = DateTime.MinValue;

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumIspita in the future ────────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumIspita_in_future()
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.DatumIspita = DateTime.Now.Date.AddDays(1);

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Invalid Tip ──────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("neki_drugi")]
        [InlineData("teorijskii")]
        public void Throws_when_Tip_invalid(string tip)
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.Tip = tip;

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Theory]
        [InlineData("teorijski")]
        [InlineData("TEORIJSKI")]
        [InlineData("Teorijski")]
        [InlineData("prakticni")]
        [InlineData("PRAKTICNI")]
        public void Accepts_valid_tip_case_insensitive(string tip)
        {
            var broker = HappyBroker();
            var request = ValidRequest();
            request.Tip = tip;
            // Ensure same tip not already passed (valid happy path)
            request.Rezultat = "pao";

            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate(); // should not throw

            broker.Verify(b => b.Add(It.IsAny<Ispit>()), Times.Once);
        }

        // ─── Invalid Rezultat ─────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("prosao")]
        [InlineData("polozio_teorijski")]
        public void Throws_when_Rezultat_invalid(string rezultat)
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.Rezultat = rezultat;

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Theory]
        [InlineData("polozio")]
        [InlineData("POLOZIO")]
        [InlineData("pao")]
        [InlineData("PAO")]
        [InlineData("nije_pristupio")]
        [InlineData("NIJE_PRISTUPIO")]
        public void Accepts_valid_rezultat_case_insensitive(string rezultat)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            // No prior ispiti so "polozio" won't hit the already-passed check
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity>());
            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(SampleData.ValidIspit());

            var request = ValidRequest();
            request.Rezultat = rezultat;

            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate(); // should not throw

            broker.Verify(b => b.Add(It.IsAny<Ispit>()), Times.Once);
        }

        // ─── Napomena too long ────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_Napomena_exceeds_500_chars()
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.Napomena = new string('x', 501);

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Accepts_napomena_exactly_500_chars()
        {
            var broker = HappyBroker();
            var request = ValidRequest();
            request.Napomena = new string('x', 500);
            request.Rezultat = "pao"; // avoid "already passed" conflict

            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.IsAny<Ispit>()), Times.Once);
        }

        // ─── Kandidat not found ───────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_not_found()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns((IEntity)null!);

            var so = new EvidentirajIspitSO(ValidRequest(), broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── No upis for kandidat ─────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_has_no_upis()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity>());

            var so = new EvidentirajIspitSO(ValidRequest(), broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Same tip same day already exists ─────────────────────────────────────────

        [Fact]
        public void Throws_when_same_tip_same_day_already_exists()
        {
            var request = ValidRequest(); // teorijski yesterday
            var conflictingIspit = new Ispit
            {
                IspitId    = 5,
                UpisId     = 1,
                Tip        = "teorijski",
                Rezultat   = "pao",
                DatumIspita = request.DatumIspita.Date,
                Napomena   = ""
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity> { conflictingIspit });

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void No_conflict_when_same_tip_different_day()
        {
            var request = ValidRequest(); // teorijski yesterday
            var differentDayIspit = new Ispit
            {
                IspitId    = 5,
                UpisId     = 1,
                Tip        = "teorijski",
                Rezultat   = "pao",
                DatumIspita = request.DatumIspita.Date.AddDays(-3),
                Napomena   = ""
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity> { differentDayIspit });
            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(SampleData.ValidIspit());

            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate(); // should NOT throw

            broker.Verify(b => b.Add(It.IsAny<Ispit>()), Times.Once);
        }

        // ─── Already passed same tip ──────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_already_passed_same_tip_and_new_is_polozio()
        {
            var request = ValidRequest(); // teorijski polozio
            var alreadyPassedIspit = new Ispit
            {
                IspitId    = 3,
                UpisId     = 1,
                Tip        = "teorijski",
                Rezultat   = "polozio",
                DatumIspita = DateTime.Now.Date.AddDays(-10),
                Napomena   = ""
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity> { alreadyPassedIspit });

            var so = new EvidentirajIspitSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Does_not_throw_when_kandidat_already_passed_same_tip_but_new_is_pao()
        {
            // Already passed teorijski, but new result is "pao" — not a duplicate pass
            var alreadyPassedIspit = new Ispit
            {
                IspitId    = 3,
                UpisId     = 1,
                Tip        = "teorijski",
                Rezultat   = "polozio",
                DatumIspita = DateTime.Now.Date.AddDays(-10),
                Napomena   = ""
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity> { alreadyPassedIspit });
            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(SampleData.ValidIspit());

            var request = ValidRequest();
            request.Rezultat = "pao";

            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate(); // should NOT throw

            broker.Verify(b => b.Add(It.IsAny<Ispit>()), Times.Once);
        }

        // ─── Rollback on broker failure ───────────────────────────────────────────────

        [Fact]
        public void Rollback_when_add_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity>());
            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Throws(new Exception("DB error"));

            var so = new EvidentirajIspitSO(ValidRequest(), broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Rollback_when_update_upis_throws()
        {
            // Setup so both tipovi would be polozio → triggers Update(upis)
            var upis = SampleData.ValidUpis();
            var existingPrakticni = new Ispit
            {
                IspitId    = 10,
                UpisId     = upis.UpisId,
                Tip        = "prakticni",
                Rezultat   = "polozio",
                DatumIspita = DateTime.Now.Date.AddDays(-5),
                Napomena   = ""
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { upis });
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Ispit)))
                  .Returns(new List<IEntity> { existingPrakticni });
            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(SampleData.ValidIspit());
            broker.Setup(b => b.Update(It.IsAny<IEntity>()))
                  .Throws(new Exception("DB update error"));

            var so = new EvidentirajIspitSO(ValidRequest(), broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Napomena null normalization ──────────────────────────────────────────────

        [Fact]
        public void Null_napomena_normalized_to_empty_string()
        {
            var broker = HappyBroker();
            var request = ValidRequest();
            request.Napomena = null;
            request.Rezultat = "pao";

            var so = new EvidentirajIspitSO(request, broker.Object);
            so.ExecuteTemplate();

            // The SO sets Napomena = request.Napomena ?? string.Empty on the new Ispit
            broker.Verify(b => b.Add(It.Is<Ispit>(i => i.Napomena == string.Empty)), Times.Once);
        }
    }
}
