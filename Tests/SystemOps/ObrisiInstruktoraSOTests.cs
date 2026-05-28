using System;
using Common.Domain;
using DBBroker;
using Moq;
using SystemOperations;
using Tests.Helpers;
using Xunit;

namespace Tests.SystemOps
{
    public class ObrisiInstruktoraSOTests
    {
        // ─── Happy path ────────────────────────────────────────────────────────────

        [Fact]
        public void Deletes_instruktor()
        {
            var broker = new Mock<IBroker>();
            var instruktor = SampleData.ValidInstruktor();

            var so = new ObrisiInstruktoraSO(instruktor, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Delete(instruktor), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Rollback on broker failure ────────────────────────────────────────────

        [Fact]
        public void Rollback_when_delete_throws()
        {
            var broker = new Mock<IBroker>();
            var instruktor = SampleData.ValidInstruktor();

            broker.Setup(b => b.Delete(instruktor))
                  .Throws(new Exception("DB error"));

            var so = new ObrisiInstruktoraSO(instruktor, broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
