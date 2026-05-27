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
    public class VratiSveKategorijeSOTests
    {
        [Fact]
        public void Returns_kategorije_from_broker()
        {
            var data = new List<IEntity> { SampleData.ValidKategorija() };
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kategorija>())).Returns(data);

            var so = new VratiSveKategorijeSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            broker.Verify(b => b.GetAll(It.IsAny<Kategorija>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Returns_empty_list_when_broker_returns_empty()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kategorija>())).Returns(new List<IEntity>());

            var so = new VratiSveKategorijeSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        [Fact]
        public void Rolls_back_when_broker_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kategorija>()))
                  .Throws(new InvalidOperationException("db error"));

            var so = new VratiSveKategorijeSO(broker.Object);

            Assert.Throws<InvalidOperationException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
