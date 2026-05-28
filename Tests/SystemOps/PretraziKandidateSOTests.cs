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
    public class PretraziKandidateSOTests
    {
        // ─── Helpers ───────────────────────────────────────────────────────────────

        private static Kandidat Mika() => new Kandidat
        {
            KandidatId = 1, Ime = "Mika",  Prezime = "Mikic",
            JMBG = "1234567890123", Email = "mika@example.com", Aktivan = true
        };

        private static Kandidat Sava() => new Kandidat
        {
            KandidatId = 2, Ime = "Sava",  Prezime = "Savic",
            JMBG = "9876543210987", Email = "sava@example.com", Aktivan = true
        };

        private static Kandidat Pera() => new Kandidat
        {
            KandidatId = 3, Ime = "Pera",  Prezime = "Peric",
            JMBG = "1111111111111", Email = "pera@example.com", Aktivan = false
        };

        private static Mock<IBroker> BrokerWith(params Kandidat[] kandidati)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Returns(new List<IEntity>(kandidati));
            return broker;
        }

        // ─── Happy path: no filter → returns all, sorted ───────────────────────────

        [Fact]
        public void Returns_all_kandidati_when_filter_empty()
        {
            var broker = BrokerWith(Sava(), Mika(), Pera());
            var so     = new PretraziKandidateSO(new KandidatSearchFilter(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(3, so.Result.Count);
        }

        [Fact]
        public void Results_sorted_by_prezime_then_ime()
        {
            var broker = BrokerWith(Sava(), Mika(), Pera());
            var so     = new PretraziKandidateSO(new KandidatSearchFilter(), broker.Object);
            so.ExecuteTemplate();

            // Expected order: Mikic, Peric, Savic
            Assert.Equal("Mikic", so.Result[0].Prezime);
            Assert.Equal("Peric", so.Result[1].Prezime);
            Assert.Equal("Savic", so.Result[2].Prezime);
        }

        [Fact]
        public void Returns_empty_list_when_no_kandidati_exist()
        {
            var broker = BrokerWith();
            var so     = new PretraziKandidateSO(new KandidatSearchFilter(), broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        [Fact]
        public void Commits_and_closes_connection()
        {
            var broker = BrokerWith(Mika());
            var so     = new PretraziKandidateSO(new KandidatSearchFilter(), broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Null filter treated as empty ──────────────────────────────────────────

        [Fact]
        public void Null_filter_returns_all_kandidati()
        {
            var broker = BrokerWith(Mika(), Sava());
            var so     = new PretraziKandidateSO(null!, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(2, so.Result.Count);
        }

        // ─── Filter by Ime (case-insensitive contains) ────────────────────────────

        [Fact]
        public void Filters_by_Ime_exact()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { Ime = "Mika" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal("Mika", so.Result[0].Ime);
        }

        [Fact]
        public void Filters_by_Ime_case_insensitive()
        {
            var broker = BrokerWith(Mika(), Sava());
            var filter = new KandidatSearchFilter { Ime = "mika" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal("Mika", so.Result[0].Ime);
        }

        [Fact]
        public void Filters_by_Ime_partial_match()
        {
            var mika2 = new Kandidat
            {
                KandidatId = 10, Ime = "Mikaela", Prezime = "Mikaelovic",
                JMBG = "2222222222222", Email = "mikaela@example.com", Aktivan = true
            };
            var broker = BrokerWith(Mika(), mika2, Sava());
            var filter = new KandidatSearchFilter { Ime = "Mik" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(2, so.Result.Count);
        }

        [Fact]
        public void Ime_filter_returns_empty_when_no_match()
        {
            var broker = BrokerWith(Mika(), Sava());
            var filter = new KandidatSearchFilter { Ime = "Zoran" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        // ─── Filter by Prezime ────────────────────────────────────────────────────

        [Fact]
        public void Filters_by_Prezime_case_insensitive()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { Prezime = "savic" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal("Savic", so.Result[0].Prezime);
        }

        [Fact]
        public void Filters_by_Prezime_partial_match()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { Prezime = "ic" };  // all end with "ic"
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(3, so.Result.Count);
        }

        // ─── Filter by JMBG (exact match) ─────────────────────────────────────────

        [Fact]
        public void Filters_by_JMBG_exact_match()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { JMBG = "1234567890123" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal("Mika", so.Result[0].Ime);
        }

        [Fact]
        public void JMBG_filter_returns_empty_when_partial_jmbg_provided()
        {
            // JMBG filter is exact (not contains), so partial won't match
            var broker = BrokerWith(Mika(), Sava());
            var filter = new KandidatSearchFilter { JMBG = "12345" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Empty(so.Result);
        }

        // ─── Filter by Email (case-insensitive contains) ──────────────────────────

        [Fact]
        public void Filters_by_Email_case_insensitive()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { Email = "SAVA@EXAMPLE.COM" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal("Sava", so.Result[0].Ime);
        }

        [Fact]
        public void Filters_by_Email_partial_domain_match()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { Email = "@example.com" };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(3, so.Result.Count);
        }

        // ─── Filter SamoAktivni ────────────────────────────────────────────────────

        [Fact]
        public void Filters_to_only_active_kandidati_when_SamoAktivni_true()
        {
            // Pera() is Aktivan = false
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { SamoAktivni = true };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(2, so.Result.Count);
            Assert.All(so.Result, k => Assert.True(k.Aktivan));
        }

        [Fact]
        public void Does_not_filter_inactive_when_SamoAktivni_false()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            var filter = new KandidatSearchFilter { SamoAktivni = false };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(3, so.Result.Count);
        }

        // ─── Combined filters ─────────────────────────────────────────────────────

        [Fact]
        public void Combined_Ime_and_SamoAktivni_filters_applied_together()
        {
            // Two "Mika"-named kandidati, one active one not
            var mikaAktivan  = new Kandidat { KandidatId = 1, Ime = "Mika", Prezime = "A", JMBG = "0000000000001", Email = "a@x.com", Aktivan = true };
            var mikaInaktivan = new Kandidat { KandidatId = 2, Ime = "Mika", Prezime = "B", JMBG = "0000000000002", Email = "b@x.com", Aktivan = false };

            var broker = BrokerWith(mikaAktivan, mikaInaktivan, Sava());
            var filter = new KandidatSearchFilter { Ime = "Mika", SamoAktivni = true };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.True(so.Result[0].Aktivan);
            Assert.Equal("Mika", so.Result[0].Ime);
        }

        [Fact]
        public void Combined_Prezime_and_JMBG_filters_applied_together()
        {
            var broker = BrokerWith(Mika(), Sava(), Pera());
            // Prezime contains "ic" AND JMBG exactly "1234567890123" → only Mika
            var filter = new KandidatSearchFilter
            {
                Prezime = "ik",           // "Mikic" contains "ik"
                JMBG    = "1234567890123"
            };
            var so = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal("Mika", so.Result[0].Ime);
        }

        // ─── Filter whitespace normalization ──────────────────────────────────────

        [Fact]
        public void Whitespace_only_Ime_filter_treated_as_no_filter()
        {
            var broker = BrokerWith(Mika(), Sava());
            var filter = new KandidatSearchFilter { Ime = "   " };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            // After normalization, Ime is null → no filtering → returns all
            Assert.Equal(2, so.Result.Count);
        }

        [Fact]
        public void Leading_trailing_spaces_in_Ime_trimmed_before_filter()
        {
            var broker = BrokerWith(Mika(), Sava());
            var filter = new KandidatSearchFilter { Ime = "  Mika  " };
            var so     = new PretraziKandidateSO(filter, broker.Object);
            so.ExecuteTemplate();

            Assert.Single(so.Result);
            Assert.Equal("Mika", so.Result[0].Ime);
        }

        // ─── Rollback on broker failure ────────────────────────────────────────────

        [Fact]
        public void Rollback_when_GetAll_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.GetAll(It.IsAny<Kandidat>()))
                  .Throws(new Exception("DB error"));

            var so = new PretraziKandidateSO(new KandidatSearchFilter(), broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
