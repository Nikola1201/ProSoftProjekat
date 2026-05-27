using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class UpisTests
    {
        private static DataTable UpisTable() => DataReaderBuilder.Empty(
            ("UpisId", typeof(int)),
            ("KandidatId", typeof(int)),
            ("PaketId", typeof(int)),
            ("DatumUpisa", typeof(DateTime)),
            ("Status", typeof(string)));

        [Fact]
        public void TableName_returns_Upis()
            => Assert.Equal("Upis", new Upis().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var u = SampleData.ValidUpis();
            Assert.Equal("1, 1, '2026-01-02', 'aktivan'", u.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_UpisId()
        {
            var u = SampleData.ValidUpis();
            Assert.Equal("UpisId = 1", u.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var u = SampleData.ValidUpis();
            Assert.Contains("UPDATE Upis SET", u.Update);
            Assert.Contains("KandidatId = 1", u.Update);
            Assert.Contains("PaketId = 1", u.Update);
            Assert.Contains("DatumUpisa = '2026-01-02'", u.Update);
            Assert.Contains("Status = 'aktivan'", u.Update);
            Assert.Contains("WHERE UpisId = 1", u.Update);
        }

        [Fact]
        public void Query_filters_by_KandidatId()
        {
            var u = SampleData.ValidUpis();
            Assert.Equal("KandidatId = 1", u.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_upises_from_reader()
        {
            var table = UpisTable();
            table.Rows.Add(1, 1, 1, new DateTime(2026, 1, 2), "aktivan");
            table.Rows.Add(2, 3, 2, new DateTime(2026, 2, 10), "polozio");

            using var reader = DataReaderBuilder.From(table);
            var list = new Upis().GetReaderList(reader).Cast<Upis>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].UpisId);
            Assert.Equal(1, list[0].KandidatId);
            Assert.Equal(1, list[0].PaketId);
            Assert.Equal(new DateTime(2026, 1, 2), list[0].DatumUpisa);
            Assert.Equal("aktivan", list[0].Status);
            Assert.Equal(2, list[1].UpisId);
            Assert.Equal(3, list[1].KandidatId);
            Assert.Equal("polozio", list[1].Status);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(UpisTable());
            Assert.Null(new Upis().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = UpisTable();
            table.Rows.Add(4, 2, 3, new DateTime(2026, 3, 15), "odustao");

            using var reader = DataReaderBuilder.From(table);
            var result = (Upis)new Upis().GetReaderResult(reader);

            Assert.Equal(4, result.UpisId);
            Assert.Equal(2, result.KandidatId);
            Assert.Equal(3, result.PaketId);
            Assert.Equal(new DateTime(2026, 3, 15), result.DatumUpisa);
            Assert.Equal("odustao", result.Status);
        }
    }
}
