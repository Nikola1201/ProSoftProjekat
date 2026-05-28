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
    public class VratiSveVozilaSOTests
    {
        [Fact]
        public void Returns_active_vozila_from_broker()
        {
            var data = new List<IEntity> { SampleData.ValidVozilo() };
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Vozilo>())).Returns(data);

            var so = new VratiSveVozilaSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            broker.Verify(b => b.GetAll(It.IsAny<Vozilo>()), Times.Once);
            broker.Verify(b => b.OpenConnection(), Times.Once);
            broker.Verify(b => b.BeginTransaction(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Filters_out_inactive_vozila()
        {
            var active = SampleData.ValidVozilo();   // Aktivno = true
            var inactive = new Vozilo
            {
                VoziloId = 2,
                Marka = "Renault",
                Model = "Clio",
                Godiste = 2018,
                Tablica = "NS001AB",
                KategorijaID = 1,
                Aktivno = false
            };
            var data = new List<IEntity> { active, inactive };
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Vozilo>())).Returns(data);

            var so = new VratiSveVozilaSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.True(so.Result[0].Aktivno);
        }

        [Fact]
        public void Returns_empty_list_when_broker_returns_empty()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Vozilo>())).Returns(new List<IEntity>());

            var so = new VratiSveVozilaSO(broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        [Fact]
        public void Rolls_back_when_broker_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Vozilo>()))
                  .Throws(new InvalidOperationException("db error"));

            var so = new VratiSveVozilaSO(broker.Object);

            Assert.Throws<InvalidOperationException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
