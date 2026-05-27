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
    public class EvidentirajUplatuSOTests
    {
        // ─── Helpers ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns a valid request without UpisId — will resolve upis via kandidat lookup.
        /// </summary>
        private static EvidentirajUplatuRequest ValidRequest() => new EvidentirajUplatuRequest
        {
            KandidatId    = 1,
            UpisId        = null,
            Iznos         = 10000m,
            NacinPlacanja = "gotovina",
            DatumPlacanja = DateTime.Now.Date.AddDays(-1),
            Napomena      = ""
        };

        /// <summary>
        /// Returns a request that explicitly specifies UpisId = 1.
        /// </summary>
        private static EvidentirajUplatuRequest ValidRequestWithUpisId() => new EvidentirajUplatuRequest
        {
            KandidatId    = 1,
            UpisId        = 1,
            Iznos         = 10000m,
            NacinPlacanja = "gotovina",
            DatumPlacanja = DateTime.Now.Date.AddDays(-1),
            Napomena      = ""
        };

        /// <summary>
        /// Returns a fully-wired happy-path broker (no UpisId path):
        /// - GetEntityByID(Kandidat)    → valid kandidat
        /// - GetEntitiesByQuery(Upis)   → one aktivan upis
        /// - GetEntityByID(PaketObuke)  → paket with Cena = 75000
        /// - GetEntitiesByQuery(Placanje) → empty list (no prior payments)
        /// - Add(Placanje)              → returns the new placanje
        /// </summary>
        private static Mock<IBroker> HappyBroker()
        {
            var broker = new Mock<IBroker>();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());

            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke()); // Cena = 75000

            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Placanje)))
                  .Returns(new List<IEntity>()); // nothing paid yet

            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Returns(SampleData.ValidPlacanje());

            return broker;
        }

        /// <summary>
        /// Returns a happy-path broker for the explicit-UpisId code path.
        /// </summary>
        private static Mock<IBroker> HappyBrokerWithUpisId()
        {
            var broker = new Mock<IBroker>();

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(SampleData.ValidUpis()); // KandidatId = 1

            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());

            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Placanje)))
                  .Returns(new List<IEntity>());

            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Returns(SampleData.ValidPlacanje());

            return broker;
        }

        // ─── Happy path (no UpisId) ───────────────────────────────────────────────────

        [Fact]
        public void Persists_placanje_and_commits_when_valid_no_upisid()
        {
            var broker = HappyBroker();
            var so = new EvidentirajUplatuSO(ValidRequest(), broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.IsAny<Placanje>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_set_after_persist_no_upisid()
        {
            var broker = HappyBroker();
            var so = new EvidentirajUplatuSO(ValidRequest(), broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
            Assert.NotNull(so.Result.Placanje);
            Assert.Equal("Uplata je uspesno evidentirana.", so.Result.Poruka);
        }

        [Fact]
        public void Preostalo_dugovanje_correct_after_partial_payment()
        {
            // Paket Cena = 75000, nothing paid yet, paying 10000 → preostalo = 65000
            var broker = HappyBroker();
            var so = new EvidentirajUplatuSO(ValidRequest(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(65000m, so.Result.PreostaloDugovanje);
        }

        [Fact]
        public void Preostalo_dugovanje_zero_after_full_payment()
        {
            // Paket Cena = 75000, nothing paid yet, paying 75000 → preostalo = 0
            var broker = HappyBroker();
            var request = ValidRequest();
            request.Iznos = 75000m;

            var so = new EvidentirajUplatuSO(request, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(0m, so.Result.PreostaloDugovanje);
        }

        // ─── Happy path (explicit UpisId) ────────────────────────────────────────────

        [Fact]
        public void Persists_placanje_when_valid_with_upisid()
        {
            var broker = HappyBrokerWithUpisId();
            var so = new EvidentirajUplatuSO(ValidRequestWithUpisId(), broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.IsAny<Placanje>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
        }

        [Fact]
        public void Result_set_after_persist_with_upisid()
        {
            var broker = HappyBrokerWithUpisId();
            var so = new EvidentirajUplatuSO(ValidRequestWithUpisId(), broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
        }

        // ─── Null request ─────────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_request_null()
        {
            var broker = new Mock<IBroker>();
            var so = new EvidentirajUplatuSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── KandidatId and UpisId both invalid ───────────────────────────────────────

        [Theory]
        [InlineData(0, null)]
        [InlineData(-1, null)]
        [InlineData(0, 0)]
        [InlineData(-1, -5)]
        public void Throws_when_both_kandidatid_and_upisid_invalid(int kandidatId, int? upisId)
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.KandidatId = kandidatId;
            request.UpisId     = upisId;

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Throws_when_kandidatid_zero_and_upisid_valid_but_kandidat_mismatch()
        {
            // UpisId = 1 (upis.KandidatId = 1), but request.KandidatId = 0 → mismatch → throws
            var broker = HappyBrokerWithUpisId();
            var request = ValidRequestWithUpisId();
            request.KandidatId = 0;

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Iznos <= 0 ───────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-0.01)]
        public void Throws_when_Iznos_not_positive(decimal iznos)
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.Iznos = iznos;

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumPlacanja == MinValue ────────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumPlacanja_min_value()
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.DatumPlacanja = DateTime.MinValue;

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumPlacanja in the future ──────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumPlacanja_in_future()
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.DatumPlacanja = DateTime.Now.Date.AddDays(1);

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Invalid NacinPlacanja ────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ček")]
        [InlineData("online")]
        public void Throws_when_NacinPlacanja_invalid(string nacin)
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.NacinPlacanja = nacin;

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Theory]
        [InlineData("gotovina")]
        [InlineData("GOTOVINA")]
        [InlineData("kartica")]
        [InlineData("KARTICA")]
        [InlineData("transfer")]
        [InlineData("TRANSFER")]
        public void Accepts_valid_nacin_placanja_case_insensitive(string nacin)
        {
            var broker = HappyBroker();
            var request = ValidRequest();
            request.NacinPlacanja = nacin;

            var so = new EvidentirajUplatuSO(request, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.IsAny<Placanje>()), Times.Once);
        }

        // ─── Napomena too long ────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_Napomena_exceeds_500_chars()
        {
            var broker = new Mock<IBroker>();
            var request = ValidRequest();
            request.Napomena = new string('x', 501);

            var so = new EvidentirajUplatuSO(request, broker.Object);

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

            var so = new EvidentirajUplatuSO(request, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.IsAny<Placanje>()), Times.Once);
        }

        // ─── Upis not found when UpisId provided ──────────────────────────────────────

        [Fact]
        public void Throws_when_upisid_provided_but_upis_not_found()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns((IEntity)null!);

            var so = new EvidentirajUplatuSO(ValidRequestWithUpisId(), broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Throws_when_upisid_upis_belongs_to_different_kandidat()
        {
            var upis = SampleData.ValidUpis();
            upis.KandidatId = 999; // different kandidat

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Upis)))
                  .Returns(upis);

            var request = ValidRequestWithUpisId();
            request.KandidatId = 1; // mismatch

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Kandidat not found when no UpisId ────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_not_found_no_upisid()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns((IEntity)null!);

            var so = new EvidentirajUplatuSO(ValidRequest(), broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── No upis when no UpisId ───────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_has_no_upis_and_no_upisid()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity>());

            var so = new EvidentirajUplatuSO(ValidRequest(), broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Iznos exceeds preostalo dugovanje ────────────────────────────────────────

        [Fact]
        public void Throws_when_iznos_exceeds_preostalo_dugovanje()
        {
            // Paket Cena = 75000, already paid 50000, preostalo = 25000, trying to pay 30000
            var existing = new Placanje
            {
                PlacanjeId    = 5,
                UpisId        = 1,
                Iznos         = 50000m,
                DatumPlacanja = DateTime.Now.Date.AddDays(-10),
                NacinPlacanja = "gotovina",
                Napomena      = ""
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke()); // Cena = 75000
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Placanje)))
                  .Returns(new List<IEntity> { existing }); // 50000 paid

            var request = ValidRequest();
            request.Iznos = 30000m; // > 25000 preostalo

            var so = new EvidentirajUplatuSO(request, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Does_not_throw_when_iznos_exactly_equals_preostalo_dugovanje()
        {
            // Paket Cena = 75000, paid 50000 → preostalo = 25000, paying exactly 25000
            var existing = new Placanje
            {
                PlacanjeId    = 5,
                UpisId        = 1,
                Iznos         = 50000m,
                DatumPlacanja = DateTime.Now.Date.AddDays(-10),
                NacinPlacanja = "gotovina",
                Napomena      = ""
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { SampleData.ValidUpis() });
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Placanje)))
                  .Returns(new List<IEntity> { existing });
            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(SampleData.ValidPlacanje());

            var request = ValidRequest();
            request.Iznos = 25000m;

            var so = new EvidentirajUplatuSO(request, broker.Object);
            so.ExecuteTemplate(); // should NOT throw

            broker.Verify(b => b.Add(It.IsAny<Placanje>()), Times.Once);
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
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Placanje)))
                  .Returns(new List<IEntity>());
            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Throws(new Exception("DB error"));

            var so = new EvidentirajUplatuSO(ValidRequest(), broker.Object);

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

            var so = new EvidentirajUplatuSO(request, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(It.Is<Placanje>(p => p.Napomena == string.Empty)), Times.Once);
        }

        // ─── Most-recent upis selected when multiple exist ────────────────────────────

        [Fact]
        public void Uses_latest_upis_when_kandidat_has_multiple()
        {
            var olderUpis = new Upis
            {
                UpisId     = 10,
                KandidatId = 1,
                PaketId    = 1,
                DatumUpisa = new DateTime(2025, 1, 1),
                Status     = "polozio"
            };
            var newerUpis = new Upis
            {
                UpisId     = 20,
                KandidatId = 1,
                PaketId    = 1,
                DatumUpisa = new DateTime(2026, 1, 1),
                Status     = "aktivan"
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Upis)))
                  .Returns(new List<IEntity> { olderUpis, newerUpis });
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());
            broker.Setup(b => b.GetEntitiesByQuery(It.Is<IEntity>(e => e is Placanje)))
                  .Returns(new List<IEntity>());
            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Returns(SampleData.ValidPlacanje());

            var so = new EvidentirajUplatuSO(ValidRequest(), broker.Object);
            so.ExecuteTemplate();

            // Result.UpisId should be newerUpis (20)
            Assert.Equal(20, so.Result.UpisId);
        }
    }
}
