using System;
using System.Collections.Generic;
using Common.Domain;
using DBBroker;
using Moq;
using SystemOperations;
using Tests.Helpers;
using Xunit;

namespace Tests.SystemOps
{
    public class VratiSveCasoveVoznjeSOTests
    {
        [Fact]
        public void Returns_casovi_from_broker()
        {
            var data = new List<IEntity> { SampleData.ValidCasVoznje() };
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>())).Returns(data);

            var so = new VratiSveCasoveVoznjeSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            broker.Verify(b => b.GetAll(It.IsAny<CasVoznje>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Returns_casovi_ordered_by_datum()
        {
            var earlier = new CasVoznje
            {
                CasId = 2,
                UpisId = 1,
                InstruktorId = 1,
                VoziloId = 1,
                DatumCas = new DateTime(2026, 1, 1, 8, 0, 0),
                TrajanjMin = 45,
                Status = "zakazan",
                Napomena = ""
            };
            var later = new CasVoznje
            {
                CasId = 3,
                UpisId = 1,
                InstruktorId = 1,
                VoziloId = 1,
                DatumCas = new DateTime(2026, 3, 1, 10, 0, 0),
                TrajanjMin = 45,
                Status = "zakazan",
                Napomena = ""
            };
            // deliberately out of order in the broker result
            var data = new List<IEntity> { later, earlier };
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>())).Returns(data);

            var so = new VratiSveCasoveVoznjeSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(2, so.Result.Count);
            Assert.Equal(earlier.DatumCas, so.Result[0].DatumCas);
            Assert.Equal(later.DatumCas, so.Result[1].DatumCas);
        }

        [Fact]
        public void Returns_empty_list_when_broker_returns_empty()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>())).Returns(new List<IEntity>());

            var so = new VratiSveCasoveVoznjeSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        [Fact]
        public void Rolls_back_when_broker_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<CasVoznje>()))
                  .Throws(new InvalidOperationException("db error"));

            var so = new VratiSveCasoveVoznjeSO(broker.Object);

            Assert.Throws<InvalidOperationException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
