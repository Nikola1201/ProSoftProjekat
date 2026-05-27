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
    public class VratiSveInstrKatSOTests
    {
        [Fact]
        public void Returns_instrkat_from_broker()
        {
            var data = new List<IEntity> { SampleData.ValidInstrKat() };
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<InstrKat>())).Returns(data);

            var so = new VratiSveInstrKatSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            broker.Verify(b => b.GetAll(It.IsAny<InstrKat>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Returns_empty_list_when_broker_returns_empty()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<InstrKat>())).Returns(new List<IEntity>());

            var so = new VratiSveInstrKatSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        [Fact]
        public void Rolls_back_when_broker_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<InstrKat>()))
                  .Throws(new InvalidOperationException("db error"));

            var so = new VratiSveInstrKatSO(broker.Object);

            Assert.Throws<InvalidOperationException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
