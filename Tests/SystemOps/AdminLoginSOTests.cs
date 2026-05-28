using System;
using Common.Domain;
using DBBroker;
using Moq;
using SystemOperations;
using Tests.Helpers;
using Xunit;

namespace Tests.SystemOps
{
    public class AdminLoginSOTests
    {
        [Fact]
        public void Sets_Result_to_admin_returned_by_broker()
        {
            var admin = SampleData.ValidAdmin();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByQuery(admin)).Returns(admin);

            var so = new AdminLoginSO(admin, broker.Object);
            so.ExecuteTemplate();

            var returned = Assert.IsType<Admin>(so.Result);
            Assert.Same(admin, returned);
            Assert.Equal(admin.Username, returned.Username);
            broker.Verify(b => b.GetEntityByQuery(admin), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Rolls_back_when_broker_throws()
        {
            var admin = SampleData.ValidAdmin();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByQuery(admin))
                  .Throws(new InvalidOperationException("korisnicki podaci netacni"));

            var so = new AdminLoginSO(admin, broker.Object);

            Assert.Throws<InvalidOperationException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            // CloseConnection is in finally, so it must fire even after the throw.
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Opens_connection_and_begins_transaction()
        {
            var admin = SampleData.ValidAdmin();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByQuery(admin)).Returns(admin);

            new AdminLoginSO(admin, broker.Object).ExecuteTemplate();

            broker.Verify(b => b.OpenConnection(), Times.Once);
            broker.Verify(b => b.BeginTransaction(), Times.Once);
        }
    }
}
