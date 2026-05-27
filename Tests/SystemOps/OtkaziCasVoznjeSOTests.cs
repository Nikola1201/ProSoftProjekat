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
    public class OtkaziCasVoznjeSOTests
    {
        // ─── Helpers ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Broker pre-wired for a successful cancellation:
        /// GetAll returns a single active cas that matches CasId = 1.
        /// Update succeeds.
        /// </summary>
        private static Mock<IBroker> HappyBroker(CasVoznje existing)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { existing });
            return broker;
        }

        // ─── Happy path ────────────────────────────────────────────────────────────

        [Fact]
        public void Updates_cas_status_to_otkazan_and_commits()
        {
            var existing = SampleData.ValidCasVoznje();  // Status = "zakazan"
            var argument = new CasVoznje { CasId = existing.CasId };
            var broker   = HappyBroker(existing);

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Update(It.Is<CasVoznje>(c => c.Status == "otkazan")), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_is_the_updated_cas()
        {
            var existing = SampleData.ValidCasVoznje();
            var argument = new CasVoznje { CasId = existing.CasId };
            var broker   = HappyBroker(existing);

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
            var result = (CasVoznje)so.Result;
            Assert.Equal("otkazan", result.Status);
        }

        [Fact]
        public void Napomena_from_argument_written_to_result_when_provided()
        {
            var existing = SampleData.ValidCasVoznje();
            existing.Napomena = "";
            var argument = new CasVoznje { CasId = existing.CasId, Napomena = "  bolestan  " };
            var broker   = HappyBroker(existing);

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);
            so.ExecuteTemplate();

            var result = (CasVoznje)so.Result;
            Assert.Equal("bolestan", result.Napomena);
        }

        [Fact]
        public void Napomena_on_existing_kept_when_argument_napomena_blank()
        {
            var existing = SampleData.ValidCasVoznje();
            existing.Napomena = "vec postoji napomena";
            var argument = new CasVoznje { CasId = existing.CasId, Napomena = "" };
            var broker   = HappyBroker(existing);

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);
            so.ExecuteTemplate();

            var result = (CasVoznje)so.Result;
            Assert.Equal("vec postoji napomena", result.Napomena);
        }

        [Fact]
        public void Napomena_normalized_to_empty_string_when_both_blank()
        {
            var existing = SampleData.ValidCasVoznje();
            existing.Napomena = "";
            var argument = new CasVoznje { CasId = existing.CasId, Napomena = null };
            var broker   = HappyBroker(existing);

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);
            so.ExecuteTemplate();

            var result = (CasVoznje)so.Result;
            Assert.Equal(string.Empty, result.Napomena);
        }

        // ─── Null argument / invalid CasId ────────────────────────────────────────

        [Fact]
        public void Throws_when_argument_null()
        {
            var broker = new Mock<IBroker>();

            var so = new OtkaziCasVoznjeSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Update(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Throws_when_CasId_invalid(int casId)
        {
            var broker   = new Mock<IBroker>();
            var argument = new CasVoznje { CasId = casId };

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Update(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Cas not found in system ──────────────────────────────────────────────

        [Fact]
        public void Throws_when_cas_not_found()
        {
            var broker   = new Mock<IBroker>();
            var argument = new CasVoznje { CasId = 999 };

            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity>());

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Update(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Cas already cancelled ────────────────────────────────────────────────

        [Fact]
        public void Throws_when_cas_already_otkazan()
        {
            var existing = SampleData.ValidCasVoznje();
            existing.Status = "otkazan";

            var broker   = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { existing });

            var argument = new CasVoznje { CasId = existing.CasId };
            var so       = new OtkaziCasVoznjeSO(argument, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Update(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // The check is case-insensitive: "OTKAZAN" should also be rejected.
        [Theory]
        [InlineData("OTKAZAN")]
        [InlineData("Otkazan")]
        public void Throws_when_cas_already_otkazan_case_insensitive(string status)
        {
            var existing = SampleData.ValidCasVoznje();
            existing.Status = status;

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { existing });

            var argument = new CasVoznje { CasId = existing.CasId };
            var so       = new OtkaziCasVoznjeSO(argument, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Update(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // Active non-"otkazan" statuses must be cancellable
        [Theory]
        [InlineData("zakazan")]
        [InlineData("odrzan")]
        [InlineData("ZAKAZAN")]
        public void Allows_cancellation_of_non_otkazan_status(string status)
        {
            var existing = SampleData.ValidCasVoznje();
            existing.Status = status;

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { existing });

            var argument = new CasVoznje { CasId = existing.CasId };
            var so       = new OtkaziCasVoznjeSO(argument, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Update(It.Is<CasVoznje>(c => c.Status == "otkazan")), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
        }

        // ─── Rollback on broker failure ────────────────────────────────────────────

        [Fact]
        public void Rollback_when_update_throws()
        {
            var existing = SampleData.ValidCasVoznje();
            var argument = new CasVoznje { CasId = existing.CasId };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Returns(new List<IEntity> { existing });
            broker.Setup(b => b.Update(It.IsAny<IEntity>()))
                  .Throws(new Exception("DB error"));

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Rollback_when_GetAll_throws()
        {
            var argument = new CasVoznje { CasId = 1 };
            var broker   = new Mock<IBroker>();

            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Throws(new Exception("DB error"));

            var so = new OtkaziCasVoznjeSO(argument, broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.Update(It.IsAny<IEntity>()), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
