using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class PlacanjeTests
    {
        private static DataTable PlacanjeTable() => DataReaderBuilder.Empty(
            ("PlacanjeId", typeof(int)),
            ("UpisId", typeof(int)),
            ("Iznos", typeof(decimal)),
            ("DatumPlacanja", typeof(DateTime)),
            ("NacinPlacanja", typeof(string)),
            ("Napomena", typeof(string)));

        [Fact]
        public void TableName_returns_Placanje()
            => Assert.Equal("Placanje", new Placanje().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var p = SampleData.ValidPlacanje();
            Assert.Equal("1, 30000, '2026-01-05', 'gotovina', 'prva rata'", p.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_PlacanjeId()
        {
            var p = SampleData.ValidPlacanje();
            Assert.Equal("PlacanjeId = 1", p.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var p = SampleData.ValidPlacanje();
            Assert.Contains("UPDATE Placanje SET", p.Update);
            Assert.Contains("UpisId = 1", p.Update);
            Assert.Contains("Iznos = 30000", p.Update);
            Assert.Contains("DatumPlacanja = '2026-01-05'", p.Update);
            Assert.Contains("NacinPlacanja = 'gotovina'", p.Update);
            Assert.Contains("Napomena = 'prva rata'", p.Update);
            Assert.Contains("WHERE PlacanjeId = 1", p.Update);
        }

        [Fact]
        public void Query_filters_by_UpisId()
        {
            var p = SampleData.ValidPlacanje();
            Assert.Equal("UpisId = 1", p.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_placanja_from_reader()
        {
            var table = PlacanjeTable();
            table.Rows.Add(1, 1, 30000m, new DateTime(2026, 1, 5), "gotovina", "prva rata");
            table.Rows.Add(2, 1, 45000m, new DateTime(2026, 2, 10), "kartica", "druga rata");

            using var reader = DataReaderBuilder.From(table);
            var list = new Placanje().GetReaderList(reader).Cast<Placanje>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].PlacanjeId);
            Assert.Equal(1, list[0].UpisId);
            Assert.Equal(30000m, list[0].Iznos);
            Assert.Equal(new DateTime(2026, 1, 5), list[0].DatumPlacanja);
            Assert.Equal("gotovina", list[0].NacinPlacanja);
            Assert.Equal(2, list[1].PlacanjeId);
            Assert.Equal(45000m, list[1].Iznos);
            Assert.Equal("kartica", list[1].NacinPlacanja);
        }

        // Placanje.GetReaderList returns an empty list when the reader has no rows.
        [Fact]
        public void GetReaderList_returns_empty_list_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(PlacanjeTable());
            var list = new Placanje().GetReaderList(reader);
            Assert.Empty(list);
        }

        // Placanje.GetReaderResult does not call reader.Read() internally — it delegates
        // from GetReaderList's while(reader.Read()) loop, so it reads the already-positioned row.
        // Pre-advance the reader before calling GetReaderResult directly.
        [Fact]
        public void GetReaderResult_hydrates_current_row_after_read()
        {
            var table = PlacanjeTable();
            table.Rows.Add(7, 3, 60000m, new DateTime(2026, 4, 20), "transfer", "kotizacija");

            using var reader = DataReaderBuilder.From(table);
            Assert.True(reader.Read());
            var result = (Placanje)new Placanje().GetReaderResult(reader);

            Assert.Equal(7, result.PlacanjeId);
            Assert.Equal(3, result.UpisId);
            Assert.Equal(60000m, result.Iznos);
            Assert.Equal(new DateTime(2026, 4, 20), result.DatumPlacanja);
            Assert.Equal("transfer", result.NacinPlacanja);
            Assert.Equal("kotizacija", result.Napomena);
        }
    }
}
