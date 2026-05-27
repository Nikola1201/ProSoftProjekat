using System.Collections.Generic;
using Common.Domain;
using DBBroker;
using Moq;
using SystemOperations;
using Tests.Helpers;
using Xunit;

namespace Tests.SystemOps
{
    public class ObrisiKandidataSOTests
    {
        // ─── Happy path: kandidat with no upisi ────────────────────────────────────

        [Fact]
        public void Deletes_kandidat_with_no_upisi()
        {
            var broker = new Mock<IBroker>();
            var kandidat = SampleData.ValidKandidat();

            broker.Setup(b => b.GetEntitiesByQuery(It.IsAny<Upis>()))
                  .Returns(new List<IEntity>());

            var so = new ObrisiKandidataSO(kandidat, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.GetEntitiesByQuery(It.IsAny<Upis>()), Times.Once);
            broker.Verify(b => b.Delete(kandidat), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Happy path: kandidat with upisi — upisi deleted first ────────────────

        [Fact]
        public void Deletes_all_upisi_before_deleting_kandidat()
        {
            var broker = new Mock<IBroker>();
            var kandidat = SampleData.ValidKandidat();
            var upis1 = SampleData.ValidUpis();
            var upis2 = new Upis { UpisId = 2, KandidatId = kandidat.KandidatId, PaketId = 1, Status = "pao" };

            broker.Setup(b => b.GetEntitiesByQuery(It.IsAny<Upis>()))
                  .Returns(new List<IEntity> { upis1, upis2 });

            var so = new ObrisiKandidataSO(kandidat, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Delete(upis1), Times.Once);
            broker.Verify(b => b.Delete(upis2), Times.Once);
            broker.Verify(b => b.Delete(kandidat), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Rollback on broker failure ────────────────────────────────────────────

        [Fact]
        public void Rollback_when_delete_throws()
        {
            var broker = new Mock<IBroker>();
            var kandidat = SampleData.ValidKandidat();

            broker.Setup(b => b.GetEntitiesByQuery(It.IsAny<Upis>()))
                  .Returns(new List<IEntity>());
            broker.Setup(b => b.Delete(kandidat))
                  .Throws(new System.Exception("DB error"));

            var so = new ObrisiKandidataSO(kandidat, broker.Object);

            Assert.ThrowsAny<System.Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Rollback when fetching upisi throws ───────────────────────────────────

        [Fact]
        public void Rollback_when_GetEntitiesByQuery_throws()
        {
            var broker = new Mock<IBroker>();
            var kandidat = SampleData.ValidKandidat();

            broker.Setup(b => b.GetEntitiesByQuery(It.IsAny<Upis>()))
                  .Throws(new System.Exception("DB error"));

            var so = new ObrisiKandidataSO(kandidat, broker.Object);

            Assert.ThrowsAny<System.Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.Delete(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
