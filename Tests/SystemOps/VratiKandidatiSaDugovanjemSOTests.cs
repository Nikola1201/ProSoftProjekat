using System;
using System.Collections.Generic;
using Common.DTO.Izvestaji;
using DBBroker;
using Moq;
using SystemOperations;
using Xunit;

namespace Tests.SystemOps
{
    public class VratiKandidatiSaDugovanjemSOTests
    {
        // ─── Helpers ─────────────────────────────────────────────────────────────────

        private static KandidatDugovanjeDto SampleRow(int id, decimal dugovanje) =>
            new KandidatDugovanjeDto
            {
                KandidatId    = id,
                PunoIme       = $"Ime Prezime{id}",
                JMBG          = $"{id:0000000000000}",
                UkupnaCena    = 75000m,
                UkupnoPlaceno = 75000m - dugovanje,
                Dugovanje     = dugovanje
            };

        private static Mock<IBroker> BrokerWithRows(List<KandidatDugovanjeDto> rows)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.ExecuteReport(It.IsAny<IReport<KandidatDugovanjeDto>>()))
                  .Returns(rows);
            return broker;
        }

        // ─── Happy path ───────────────────────────────────────────────────────────────

        [Fact]
        public void Returns_rows_from_broker_report()
        {
            var rows = new List<KandidatDugovanjeDto>
            {
                SampleRow(1, 30000m),
                SampleRow(2, 15000m)
            };

            var broker = BrokerWithRows(rows);
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
            Assert.Equal(2, so.Result.Count);
        }

        [Fact]
        public void Calls_execute_report_exactly_once()
        {
            var broker = BrokerWithRows(new List<KandidatDugovanjeDto>());
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);
            so.ExecuteTemplate();

            broker.Verify(
                b => b.ExecuteReport(It.IsAny<IReport<KandidatDugovanjeDto>>()),
                Times.Once);
        }

        [Fact]
        public void Commits_after_successful_report()
        {
            var broker = BrokerWithRows(new List<KandidatDugovanjeDto>());
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_empty_list_when_broker_returns_no_rows()
        {
            var broker = BrokerWithRows(new List<KandidatDugovanjeDto>());
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
            Assert.Empty(so.Result);
        }

        [Fact]
        public void Result_preserves_row_fields()
        {
            var row = SampleRow(42, 12345.67m);
            var broker = BrokerWithRows(new List<KandidatDugovanjeDto> { row });
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);
            so.ExecuteTemplate();

            var result = so.Result[0];
            Assert.Equal(42, result.KandidatId);
            Assert.Equal(12345.67m, result.Dugovanje);
            Assert.Equal("Ime Prezime42", result.PunoIme);
        }

        [Fact]
        public void Result_row_count_matches_broker_return()
        {
            var rows = new List<KandidatDugovanjeDto>
            {
                SampleRow(1, 5000m),
                SampleRow(2, 10000m),
                SampleRow(3, 20000m)
            };

            var broker = BrokerWithRows(rows);
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(3, so.Result.Count);
        }

        [Fact]
        public void Result_is_same_reference_as_broker_return()
        {
            var rows = new List<KandidatDugovanjeDto> { SampleRow(1, 1000m) };
            var broker = BrokerWithRows(rows);
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);
            so.ExecuteTemplate();

            // The SO does not transform the list — it is the same object
            Assert.Same(rows, so.Result);
        }

        // ─── Rollback on broker failure ───────────────────────────────────────────────

        [Fact]
        public void Rollback_when_execute_report_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.ExecuteReport(It.IsAny<IReport<KandidatDugovanjeDto>>()))
                  .Throws(new Exception("DB error"));

            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Result_null_before_execute_template()
        {
            var broker = BrokerWithRows(new List<KandidatDugovanjeDto>());
            var so = new VratiKandidatiSaDugovanjemSO(broker.Object);

            // Before calling ExecuteTemplate, Result should be null (default)
            Assert.Null(so.Result);
        }
    }
}
