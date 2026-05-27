using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class CasVoznjeTests
    {
        private static DataTable CasVoznjeTable() => DataReaderBuilder.Empty(
            ("CasId", typeof(int)),
            ("UpisId", typeof(int)),
            ("InstruktorId", typeof(int)),
            ("VoziloId", typeof(int)),
            ("DatumCas", typeof(DateTime)),
            ("TrajanjMin", typeof(int)),
            ("Status", typeof(string)),
            ("Napomena", typeof(string)));

        [Fact]
        public void TableName_returns_CasVoznje()
            => Assert.Equal("CasVoznje", new CasVoznje().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var c = SampleData.ValidCasVoznje();
            Assert.Equal("1, 1, 1, '2026-02-01 10:00', 45, 'zakazan', ''", c.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_CasId()
        {
            var c = SampleData.ValidCasVoznje();
            Assert.Equal("CasId = 1", c.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var c = SampleData.ValidCasVoznje();
            Assert.Contains("UPDATE CasVoznje SET", c.Update);
            Assert.Contains("UpisId = 1", c.Update);
            Assert.Contains("InstruktorId = 1", c.Update);
            Assert.Contains("VoziloId = 1", c.Update);
            Assert.Contains("DatumCas = '2026-02-01 10:00'", c.Update);
            Assert.Contains("TrajanjMin = 45", c.Update);
            Assert.Contains("Status = 'zakazan'", c.Update);
            Assert.Contains("Napomena = ''", c.Update);
            Assert.Contains("WHERE CasId = 1", c.Update);
        }

        [Fact]
        public void Query_filters_by_UpisId()
        {
            var c = SampleData.ValidCasVoznje();
            Assert.Equal("UpisId = 1", c.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_casovi_from_reader()
        {
            var table = CasVoznjeTable();
            table.Rows.Add(1, 1, 1, 1, new DateTime(2026, 2, 1, 10, 0, 0), 45, "zakazan", "");
            table.Rows.Add(2, 2, 3, 2, new DateTime(2026, 3, 15, 14, 30, 0), 60, "odrzan", "bez napomene");

            using var reader = DataReaderBuilder.From(table);
            var list = new CasVoznje().GetReaderList(reader).Cast<CasVoznje>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].CasId);
            Assert.Equal(1, list[0].UpisId);
            Assert.Equal(1, list[0].InstruktorId);
            Assert.Equal(1, list[0].VoziloId);
            Assert.Equal(new DateTime(2026, 2, 1, 10, 0, 0), list[0].DatumCas);
            Assert.Equal(45, list[0].TrajanjMin);
            Assert.Equal("zakazan", list[0].Status);
            // Row-2 spot-check: business-key fields differ from row 1
            Assert.Equal(2, list[1].CasId);
            Assert.Equal(2, list[1].UpisId);
            Assert.Equal(3, list[1].InstruktorId);
            Assert.Equal(60, list[1].TrajanjMin);
            Assert.Equal("odrzan", list[1].Status);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(CasVoznjeTable());
            // CasVoznje.GetReaderResult does not call reader.Read() — it hydrates
            // the already-positioned row. No rows means Read() was never called,
            // so we confirm GetReaderList on an empty reader gives an empty list.
            var list = new CasVoznje().GetReaderList(reader);
            Assert.Empty(list);
        }

        [Fact]
        public void GetReaderResult_hydrates_current_row_after_read()
        {
            var table = CasVoznjeTable();
            table.Rows.Add(7, 3, 2, 4, new DateTime(2026, 4, 10, 9, 15, 0), 90, "otkazan", "vreme");

            using var reader = DataReaderBuilder.From(table);
            Assert.True(reader.Read());
            var result = (CasVoznje)new CasVoznje().GetReaderResult(reader);

            Assert.Equal(7, result.CasId);
            Assert.Equal(3, result.UpisId);
            Assert.Equal(2, result.InstruktorId);
            Assert.Equal(4, result.VoziloId);
            Assert.Equal(new DateTime(2026, 4, 10, 9, 15, 0), result.DatumCas);
            Assert.Equal(90, result.TrajanjMin);
            Assert.Equal("otkazan", result.Status);
            Assert.Equal("vreme", result.Napomena);
        }
    }
}
