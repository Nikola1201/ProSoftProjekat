using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class IspitTests
    {
        private static DataTable IspitTable() => DataReaderBuilder.Empty(
            ("IspitId", typeof(int)),
            ("UpisId", typeof(int)),
            ("DatumIspita", typeof(DateTime)),
            ("Tip", typeof(string)),
            ("Rezultat", typeof(string)),
            ("Napomena", typeof(string)));

        [Fact]
        public void TableName_returns_Ispit()
            => Assert.Equal("Ispit", new Ispit().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var i = SampleData.ValidIspit();
            Assert.Equal("1, '2026-03-01', 'teorijski', 'polozio', ''", i.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_IspitId()
        {
            var i = SampleData.ValidIspit();
            Assert.Equal("IspitId = 1", i.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var i = SampleData.ValidIspit();
            Assert.Contains("UPDATE Ispit SET", i.Update);
            Assert.Contains("UpisId = 1", i.Update);
            Assert.Contains("DatumIspita = '2026-03-01'", i.Update);
            Assert.Contains("Tip = 'teorijski'", i.Update);
            Assert.Contains("Rezultat = 'polozio'", i.Update);
            Assert.Contains("Napomena = ''", i.Update);
            Assert.Contains("WHERE IspitId = 1", i.Update);
        }

        [Fact]
        public void Query_filters_by_UpisId()
        {
            var i = SampleData.ValidIspit();
            Assert.Equal("UpisId = 1", i.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_ispiti_from_reader()
        {
            var table = IspitTable();
            table.Rows.Add(1, 1, new DateTime(2026, 3, 1), "teorijski", "polozio", "");
            table.Rows.Add(2, 2, new DateTime(2026, 4, 15), "prakticni", "nije polozio", "drugi pokusaj");

            using var reader = DataReaderBuilder.From(table);
            var list = new Ispit().GetReaderList(reader).Cast<Ispit>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].IspitId);
            Assert.Equal(1, list[0].UpisId);
            Assert.Equal(new DateTime(2026, 3, 1), list[0].DatumIspita);
            Assert.Equal("teorijski", list[0].Tip);
            Assert.Equal("polozio", list[0].Rezultat);
            // Row-2 spot-check: type and result differ to catch swap bugs
            Assert.Equal(2, list[1].IspitId);
            Assert.Equal(2, list[1].UpisId);
            Assert.Equal("prakticni", list[1].Tip);
            Assert.Equal("nije polozio", list[1].Rezultat);
            Assert.Equal(new DateTime(2026, 4, 15), list[1].DatumIspita);
        }

        // Ispit.GetReaderResult does not call reader.Read() internally — it delegates
        // from GetReaderList's while(reader.Read()) loop, so it reads the already-positioned row.
        // Pre-advance the reader before calling GetReaderResult directly.
        [Fact]
        public void GetReaderList_returns_empty_list_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(IspitTable());
            var list = new Ispit().GetReaderList(reader);
            Assert.Empty(list);
        }

        [Fact]
        public void GetReaderResult_hydrates_current_row_after_read()
        {
            var table = IspitTable();
            table.Rows.Add(9, 5, new DateTime(2026, 5, 20), "prakticni", "polozio", "odlican");

            using var reader = DataReaderBuilder.From(table);
            Assert.True(reader.Read());
            var result = (Ispit)new Ispit().GetReaderResult(reader);

            Assert.Equal(9, result.IspitId);
            Assert.Equal(5, result.UpisId);
            Assert.Equal(new DateTime(2026, 5, 20), result.DatumIspita);
            Assert.Equal("prakticni", result.Tip);
            Assert.Equal("polozio", result.Rezultat);
            Assert.Equal("odlican", result.Napomena);
        }
    }
}
