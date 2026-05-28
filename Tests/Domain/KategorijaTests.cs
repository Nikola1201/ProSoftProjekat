using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class KategorijaTests
    {
        private static DataTable KategorijaTable() => DataReaderBuilder.Empty(
            ("KategorijaID", typeof(int)),
            ("NazivKategorije", typeof(string)));

        [Fact]
        public void TableName_returns_Kategorija()
            => Assert.Equal("Kategorija", new Kategorija().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var k = SampleData.ValidKategorija();
            Assert.Equal("'B'", k.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_KategorijaID()
        {
            var k = SampleData.ValidKategorija();
            Assert.Equal("KategorijaID = 1", k.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var k = SampleData.ValidKategorija();
            Assert.Contains("UPDATE Kategorija SET", k.Update);
            Assert.Contains("NazivKategorije = 'B'", k.Update);
            Assert.Contains("WHERE KategorijaID = 1", k.Update);
        }

        [Fact]
        public void Query_filters_by_NazivKategorije()
        {
            var k = SampleData.ValidKategorija();
            Assert.Equal("NazivKategorije = 'B'", k.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_kategorijas_from_reader()
        {
            var table = KategorijaTable();
            table.Rows.Add(1, "B");
            table.Rows.Add(2, "A");

            using var reader = DataReaderBuilder.From(table);
            var list = new Kategorija().GetReaderList(reader).Cast<Kategorija>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].KategorijaID);
            Assert.Equal("B", list[0].NazivKategorije);
            Assert.Equal(2, list[1].KategorijaID);
            Assert.Equal("A", list[1].NazivKategorije);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(KategorijaTable());
            Assert.Null(new Kategorija().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = KategorijaTable();
            table.Rows.Add(3, "C");

            using var reader = DataReaderBuilder.From(table);
            var result = (Kategorija)new Kategorija().GetReaderResult(reader);

            Assert.Equal(3, result.KategorijaID);
            Assert.Equal("C", result.NazivKategorije);
        }

        [Fact]
        public void NazivKategorije_setter_throws_when_longer_than_two_chars()
        {
            var k = new Kategorija();
            Assert.Throws<ArgumentException>(() => k.NazivKategorije = "BBB");
        }

        [Fact]
        public void NazivKategorije_setter_accepts_two_chars()
        {
            var k = new Kategorija();
            k.NazivKategorije = "B1";
            Assert.Equal("B1", k.NazivKategorije);
        }
    }
}
