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
    public class VratiSveKandidateSOTests
    {
        // Helper: a Kandidat that has an active upis (should appear when upisani=true)
        private static Kandidat KandidatUpisani() => new Kandidat
        {
            KandidatId = 10,
            Ime = "Upisani",
            Prezime = "Kandidat",
            JMBG = "1111111111111",
            Telefon = "0641111111",
            Email = "upisani@auto.rs",
            Adresa = "Adresa 1",
            DatumUpisa = new DateTime(2026, 1, 1),
            Aktivan = true
        };

        // Helper: a Kandidat with NO active upis, aktivan=true (should appear when upisani=false)
        private static Kandidat KandidatNeupisani() => new Kandidat
        {
            KandidatId = 20,
            Ime = "Neupisani",
            Prezime = "Kandidat",
            JMBG = "2222222222222",
            Telefon = "0642222222",
            Email = "neupisani@auto.rs",
            Adresa = "Adresa 2",
            DatumUpisa = new DateTime(2026, 1, 1),
            Aktivan = true
        };

        private static Upis UpisZaKandidataId(int kandidatId) => new Upis
        {
            UpisId = kandidatId,
            KandidatId = kandidatId,
            PaketId = 1,
            DatumUpisa = new DateTime(2026, 1, 2),
            Status = "aktivan"
        };

        // --- upisani = true ---

        [Fact]
        public void Returns_enrolled_candidates_when_upisani_is_true()
        {
            var enrolled = KandidatUpisani();
            var notEnrolled = KandidatNeupisani();
            var upis = UpisZaKandidataId(enrolled.KandidatId);

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Returns(new List<IEntity> { enrolled, notEnrolled });
            broker.Setup(b => b.GetAll(It.IsAny<Upis>()))
                  .Returns(new List<IEntity> { upis });

            var so = new VratiSveKandidateSO(upisani: true, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal(enrolled.KandidatId, so.Result[0].KandidatId);
            broker.Verify(b => b.GetAll(It.IsAny<Kandidat>()), Times.Once);
            broker.Verify(b => b.GetAll(It.IsAny<Upis>()), Times.Once);
            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // --- upisani = false ---

        [Fact]
        public void Returns_non_enrolled_active_candidates_when_upisani_is_false()
        {
            var enrolled = KandidatUpisani();
            var notEnrolled = KandidatNeupisani();
            var upis = UpisZaKandidataId(enrolled.KandidatId);

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Returns(new List<IEntity> { enrolled, notEnrolled });
            broker.Setup(b => b.GetAll(It.IsAny<Upis>()))
                  .Returns(new List<IEntity> { upis });

            var so = new VratiSveKandidateSO(upisani: false, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal(notEnrolled.KandidatId, so.Result[0].KandidatId);
        }

        [Fact]
        public void Excludes_inactive_candidates_from_non_enrolled_result()
        {
            var inactive = new Kandidat
            {
                KandidatId = 30,
                Ime = "Neaktivan",
                Prezime = "Kandidat",
                JMBG = "3333333333333",
                Telefon = "0643333333",
                Email = "neaktivan@auto.rs",
                Adresa = "Adresa 3",
                DatumUpisa = new DateTime(2025, 1, 1),
                Aktivan = false
            };

            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Returns(new List<IEntity> { inactive });
            broker.Setup(b => b.GetAll(It.IsAny<Upis>()))
                  .Returns(new List<IEntity>());

            var so = new VratiSveKandidateSO(upisani: false, broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        [Fact]
        public void Rolls_back_when_broker_throws_on_kandidat_getall()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Throws(new InvalidOperationException("db error"));

            var so = new VratiSveKandidateSO(upisani: true, broker.Object);

            Assert.Throws<InvalidOperationException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
