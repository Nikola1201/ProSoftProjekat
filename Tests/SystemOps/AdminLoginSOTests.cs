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

            Assert.Same(admin, so.Result);
            broker.Verify(b => b.GetEntityByQuery(admin), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Rolls_back_when_broker_throws()
        {
            var admin = SampleData.ValidAdmin();
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetEntityByQuery(admin)).Throws(new System.Exception("boom"));

            var so = new AdminLoginSO(admin, broker.Object);

            Assert.Throws<System.Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        [Fact]
        public void Opens_connection_and_begins_transaction_first()
        {
            var admin = SampleData.ValidAdmin();
            var broker = new Mock<IBroker>(MockBehavior.Loose);
            broker.Setup(b => b.GetEntityByQuery(admin)).Returns(admin);

            new AdminLoginSO(admin, broker.Object).ExecuteTemplate();

            broker.Verify(b => b.OpenConnection(), Times.Once);
            broker.Verify(b => b.BeginTransaction(), Times.Once);
        }
    }
}
