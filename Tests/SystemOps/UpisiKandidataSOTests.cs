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
    public class UpisiKandidataSOTests
    {
        /// <summary>
        /// Returns a broker mock ready for a successful UpisiKandidata run:
        /// - GetAll(Upis) returns empty list (no existing upisi for kandidat)
        /// - GetEntityByID(Kandidat) returns active kandidat
        /// - GetEntityByID(PaketObuke) returns valid paket
        /// - Add returns the upis
        /// </summary>
        private static Mock<IBroker> HappyBroker(Upis upis)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Upis>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());
            broker.Setup(b => b.Add(It.IsAny<IEntity>())).Returns(upis);
            return broker;
        }

        // ─── Happy path ────────────────────────────────────────────────────────────

        [Fact]
        public void Persists_upis_when_valid()
        {
            var upis = SampleData.ValidUpis();
            var broker = HappyBroker(upis);

            var so = new UpisiKandidataSO(upis, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(upis), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_set_after_persist()
        {
            var upis = SampleData.ValidUpis();
            var broker = HappyBroker(upis);

            var so = new UpisiKandidataSO(upis, broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
        }

        [Fact]
        public void Persists_when_DatumUpisa_is_today()
        {
            var upis = SampleData.ValidUpis();
            upis.DatumUpisa = DateTime.Now.Date;
            var broker = HappyBroker(upis);

            var so = new UpisiKandidataSO(upis, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Add(upis), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
        }

        // ─── Null upis ─────────────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_upis_null()
        {
            var broker = new Mock<IBroker>();

            var so = new UpisiKandidataSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── KandidatId invalid ────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Throws_when_KandidatId_invalid(int kandidatId)
        {
            var broker = new Mock<IBroker>();
            var upis = SampleData.ValidUpis();
            upis.KandidatId = kandidatId;

            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── PaketId invalid ───────────────────────────────────────────────────────

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Throws_when_PaketId_invalid(int paketId)
        {
            var broker = new Mock<IBroker>();
            var upis = SampleData.ValidUpis();
            upis.PaketId = paketId;

            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── DatumUpisa in future ──────────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumUpisa_in_future()
        {
            var broker = new Mock<IBroker>();
            var upis = SampleData.ValidUpis();
            upis.DatumUpisa = DateTime.Now.Date.AddDays(1);

            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Blank Status ──────────────────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Throws_when_Status_blank(string? status)
        {
            var broker = new Mock<IBroker>();
            var upis = SampleData.ValidUpis();
            upis.Status = status!;

            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Kandidat not found ────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_not_found()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Upis>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat))).Returns((IEntity)null!);
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());

            var upis = SampleData.ValidUpis();
            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Kandidat not active ───────────────────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_not_active()
        {
            var neaktivan = SampleData.ValidKandidat();
            neaktivan.Aktivan = false;

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Upis>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat))).Returns(neaktivan);
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());

            var upis = SampleData.ValidUpis();
            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Paket not found ───────────────────────────────────────────────────────

        [Fact]
        public void Throws_when_paket_not_found()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Upis>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke))).Returns((IEntity)null!);

            var upis = SampleData.ValidUpis();
            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Candidate already has active upis ────────────────────────────────────

        [Fact]
        public void Throws_when_kandidat_already_has_active_upis()
        {
            var existingUpis = SampleData.ValidUpis(); // Status = "aktivan", KandidatId = 1

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Upis>()))
                  .Returns(new List<IEntity> { existingUpis });
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());

            var upis = SampleData.ValidUpis();
            upis.UpisId = 99; // different record, same kandidat
            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Add(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Rollback on broker failure ────────────────────────────────────────────

        [Fact]
        public void Rollback_when_add_throws()
        {
            var upis = SampleData.ValidUpis();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Upis>())).Returns(new List<IEntity>());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is Kandidat)))
                  .Returns(SampleData.ValidKandidat());
            broker.Setup(b => b.GetEntityByID(It.Is<IEntity>(e => e is PaketObuke)))
                  .Returns(SampleData.ValidPaketObuke());
            broker.Setup(b => b.Add(It.IsAny<IEntity>()))
                  .Throws(new Exception("DB error"));

            var so = new UpisiKandidataSO(upis, broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
