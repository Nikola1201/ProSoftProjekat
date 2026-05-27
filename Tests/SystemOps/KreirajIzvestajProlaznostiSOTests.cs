using System;
using System.Collections.Generic;
using Common.DTO.Izvestaji;
using Common.Validation;
using DBBroker;
using Moq;
using SystemOperations;
using Xunit;

namespace Tests.SystemOps
{
    public class KreirajIzvestajProlaznostiSOTests
    {
        // ─── Helpers ─────────────────────────────────────────────────────────────────

        private static IzvestajProlaznostiKriterijum ValidKriterijum() =>
            new IzvestajProlaznostiKriterijum
            {
                DatumOd    = new DateTime(2026, 1, 1),
                DatumDo    = new DateTime(2026, 3, 31),
                Kategorija = "B",
                TipIspita  = TipIspitaFilter.Oba,
                IncludeNoData           = false,
                IncludeOnlyAktivanUpis  = false
            };

        private static IzvestajProlaznostiStavkaDto SampleStavka(StatusProlaznosti status) =>
            new IzvestajProlaznostiStavkaDto
            {
                KandidatId            = 1,
                Ime                   = "Mika",
                Prezime               = "Mikic",
                Jmbg                  = "1234567890123",
                Kategorija            = "B",
                Status                = status,
                DatumPoslednjegIspita = new DateTime(2026, 2, 15),
                BrojPokusajaTeorijski = 1,
                BrojPokusajaPrakticni = 1
            };

        private static Mock<IBroker> BrokerWithStavke(List<IzvestajProlaznostiStavkaDto> stavke)
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()))
                  .Returns(stavke);
            return broker;
        }

        // ─── Happy path ───────────────────────────────────────────────────────────────

        [Fact]
        public void Returns_result_from_broker_report()
        {
            var stavke = new List<IzvestajProlaznostiStavkaDto>
            {
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.Pao)
            };

            var broker = BrokerWithStavke(stavke);
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
            Assert.Equal(2, so.Result.Stavke.Count);
        }

        [Fact]
        public void Calls_execute_report_exactly_once()
        {
            var broker = BrokerWithStavke(new List<IzvestajProlaznostiStavkaDto>());
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            broker.Verify(
                b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()),
                Times.Once);
        }

        [Fact]
        public void Commits_after_successful_report()
        {
            var broker = BrokerWithStavke(new List<IzvestajProlaznostiStavkaDto>());
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.Commit(), Times.Once);
            broker.Verify(b => b.Rollback(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Summary calculations ─────────────────────────────────────────────────────

        [Fact]
        public void Summary_ukupno_polozilo_counts_correctly()
        {
            var stavke = new List<IzvestajProlaznostiStavkaDto>
            {
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.Pao),
                SampleStavka(StatusProlaznosti.UToku)
            };

            var broker = BrokerWithStavke(stavke);
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(2, so.Result.Summary.UkupnoPolozilo);
        }

        [Fact]
        public void Summary_ukupno_palo_counts_correctly()
        {
            var stavke = new List<IzvestajProlaznostiStavkaDto>
            {
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.Pao),
                SampleStavka(StatusProlaznosti.Pao),
                SampleStavka(StatusProlaznosti.UToku)
            };

            var broker = BrokerWithStavke(stavke);
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(2, so.Result.Summary.UkupnoPalo);
        }

        [Fact]
        public void Summary_ukupno_u_toku_counts_correctly()
        {
            var stavke = new List<IzvestajProlaznostiStavkaDto>
            {
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.UToku),
                SampleStavka(StatusProlaznosti.UToku)
            };

            var broker = BrokerWithStavke(stavke);
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(2, so.Result.Summary.UkupnoUToku);
        }

        [Fact]
        public void Summary_procenat_prolaznosti_calculated_correctly()
        {
            // 2 polozilo, 2 pao → denominator = 4, procenat = 50.00
            var stavke = new List<IzvestajProlaznostiStavkaDto>
            {
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.Pao),
                SampleStavka(StatusProlaznosti.Pao)
            };

            var broker = BrokerWithStavke(stavke);
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(50.00m, so.Result.Summary.ProcenatProlaznosti);
        }

        [Fact]
        public void Summary_procenat_prolaznosti_zero_when_no_polozio_or_pao()
        {
            // Only UToku — denominator = 0, procenat should be 0
            var stavke = new List<IzvestajProlaznostiStavkaDto>
            {
                SampleStavka(StatusProlaznosti.UToku),
                SampleStavka(StatusProlaznosti.UToku)
            };

            var broker = BrokerWithStavke(stavke);
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(0m, so.Result.Summary.ProcenatProlaznosti);
        }

        [Fact]
        public void Summary_procenat_100_when_all_polozio()
        {
            var stavke = new List<IzvestajProlaznostiStavkaDto>
            {
                SampleStavka(StatusProlaznosti.Polozio),
                SampleStavka(StatusProlaznosti.Polozio)
            };

            var broker = BrokerWithStavke(stavke);
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.Equal(100.00m, so.Result.Summary.ProcenatProlaznosti);
        }

        [Fact]
        public void Result_stavke_empty_when_broker_returns_empty_list()
        {
            var broker = BrokerWithStavke(new List<IzvestajProlaznostiStavkaDto>());
            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);
            so.ExecuteTemplate();

            Assert.NotNull(so.Result);
            Assert.Empty(so.Result.Stavke);
            Assert.Equal(0, so.Result.Summary.UkupnoPolozilo);
            Assert.Equal(0, so.Result.Summary.UkupnoPalo);
            Assert.Equal(0, so.Result.Summary.UkupnoUToku);
            Assert.Equal(0m, so.Result.Summary.ProcenatProlaznosti);
        }

        // ─── Validation: null kriterijum ──────────────────────────────────────────────

        [Fact]
        public void Throws_when_kriterijum_null()
        {
            var broker = new Mock<IBroker>();
            var so = new KreirajIzvestajProlaznostiSO(null!, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }

        // ─── Validation: DatumOd > DatumDo ────────────────────────────────────────────

        [Fact]
        public void Throws_when_DatumOd_after_DatumDo()
        {
            var broker = new Mock<IBroker>();
            var kriterijum = ValidKriterijum();
            kriterijum.DatumOd = new DateTime(2026, 4, 1);
            kriterijum.DatumDo = new DateTime(2026, 3, 31);

            var so = new KreirajIzvestajProlaznostiSO(kriterijum, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Fact]
        public void Does_not_throw_when_DatumOd_equals_DatumDo()
        {
            // Same day is valid (one-day range)
            var sameDay = new DateTime(2026, 3, 1);
            var kriterijum = ValidKriterijum();
            kriterijum.DatumOd = sameDay;
            kriterijum.DatumDo = sameDay;

            var broker = BrokerWithStavke(new List<IzvestajProlaznostiStavkaDto>());
            var so = new KreirajIzvestajProlaznostiSO(kriterijum, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()), Times.Once);
        }

        // ─── Validation: blank Kategorija ─────────────────────────────────────────────

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Throws_when_Kategorija_blank(string? kategorija)
        {
            var broker = new Mock<IBroker>();
            var kriterijum = ValidKriterijum();
            kriterijum.Kategorija = kategorija!;

            var so = new KreirajIzvestajProlaznostiSO(kriterijum, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        // ─── Validation: invalid TipIspita ────────────────────────────────────────────

        [Theory]
        [InlineData(-1)]
        [InlineData(99)]
        public void Throws_when_TipIspita_invalid(int tipValue)
        {
            var broker = new Mock<IBroker>();
            var kriterijum = ValidKriterijum();
            kriterijum.TipIspita = (TipIspitaFilter)tipValue;

            var so = new KreirajIzvestajProlaznostiSO(kriterijum, broker.Object);

            Assert.Throws<ValidacijaException>(() => so.ExecuteTemplate());
            broker.Verify(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()), Times.Never);
            broker.Verify(b => b.Rollback(), Times.Once);
        }

        [Theory]
        [InlineData(TipIspitaFilter.Teorijski)]
        [InlineData(TipIspitaFilter.Prakticni)]
        [InlineData(TipIspitaFilter.Oba)]
        public void Accepts_all_valid_tip_ispita_filter_values(TipIspitaFilter tip)
        {
            var broker = BrokerWithStavke(new List<IzvestajProlaznostiStavkaDto>());
            var kriterijum = ValidKriterijum();
            kriterijum.TipIspita = tip;

            var so = new KreirajIzvestajProlaznostiSO(kriterijum, broker.Object);
            so.ExecuteTemplate();

            broker.Verify(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()), Times.Once);
        }

        // ─── Rollback on broker failure ───────────────────────────────────────────────

        [Fact]
        public void Rollback_when_execute_report_throws()
        {
            var broker = new Mock<IBroker>();
            broker.Setup(b => b.ExecuteReport(It.IsAny<IReport<IzvestajProlaznostiStavkaDto>>()))
                  .Throws(new Exception("DB error"));

            var so = new KreirajIzvestajProlaznostiSO(ValidKriterijum(), broker.Object);

            Assert.ThrowsAny<Exception>(() => so.ExecuteTemplate());
            broker.Verify(b => b.Rollback(), Times.Once);
            broker.Verify(b => b.Commit(), Times.Never);
            broker.Verify(b => b.CloseConnection(), Times.Once);
        }
    }
}
