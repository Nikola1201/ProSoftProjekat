using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class KandidatTests
    {
        private static DataTable KandidatTable() => DataReaderBuilder.Empty(
            ("KandidatId", typeof(int)),
            ("Ime", typeof(string)),
            ("Prezime", typeof(string)),
            ("JMBG", typeof(string)),
            ("Telefon", typeof(string)),
            ("Email", typeof(string)),
            ("Adresa", typeof(string)),
            ("DatumUpisa", typeof(DateTime)),
            ("Aktivan", typeof(bool)));

        [Fact]
        public void TableName_returns_Kandidat()
            => Assert.Equal("Kandidat", new Kandidat().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var k = SampleData.ValidKandidat();
            Assert.Equal(
                "'Mika', 'Mikic', '1234567890123', '0641234567', 'mika@example.com', " +
                "'Knez Mihailova 1, Beograd', '2026-01-01', 1",
                k.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_KandidatId()
        {
            var k = SampleData.ValidKandidat();
            Assert.Equal("KandidatId = 1", k.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var k = SampleData.ValidKandidat();
            Assert.Contains("UPDATE Kandidat SET", k.Update);
            Assert.Contains("Ime = 'Mika'", k.Update);
            Assert.Contains("Prezime = 'Mikic'", k.Update);
            Assert.Contains("JMBG = '1234567890123'", k.Update);
            Assert.Contains("Telefon = '0641234567'", k.Update);
            Assert.Contains("Email = 'mika@example.com'", k.Update);
            Assert.Contains("Adresa = 'Knez Mihailova 1, Beograd'", k.Update);
            Assert.Contains("DatumUpisa = '2026-01-01'", k.Update);
            Assert.Contains("Aktivan = 1", k.Update);
            Assert.Contains("WHERE KandidatId = 1", k.Update);
        }

        [Fact]
        public void Query_filters_by_JMBG()
        {
            var k = SampleData.ValidKandidat();
            Assert.Equal("JMBG = '1234567890123'", k.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_kandidats_from_reader()
        {
            var table = KandidatTable();
            table.Rows.Add(1, "Mika", "Mikic", "1234567890123", "0641234567", "mika@example.com",
                "Knez Mihailova 1, Beograd", new DateTime(2026, 1, 1), true);
            table.Rows.Add(2, "Pera", "Peric", "1111111111111", "0611111111", "pera@example.com",
                "Cara Lazara 5", new DateTime(2026, 2, 1), false);

            using var reader = DataReaderBuilder.From(table);
            var list = new Kandidat().GetReaderList(reader).Cast<Kandidat>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].KandidatId);
            Assert.Equal("Mika", list[0].Ime);
            Assert.Equal("Mikic", list[0].Prezime);
            Assert.Equal("1234567890123", list[0].JMBG);
            Assert.True(list[0].Aktivan);
            Assert.Equal(2, list[1].KandidatId);
            Assert.Equal("Pera", list[1].Ime);
            Assert.False(list[1].Aktivan);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(KandidatTable());
            Assert.Null(new Kandidat().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = KandidatTable();
            table.Rows.Add(5, "Ana", "Anic", "9999999999999", "0699999999", "ana@example.com",
                "Beogradska 10", new DateTime(2026, 3, 1), true);

            using var reader = DataReaderBuilder.From(table);
            var result = (Kandidat)new Kandidat().GetReaderResult(reader);

            Assert.Equal(5, result.KandidatId);
            Assert.Equal("Ana", result.Ime);
            Assert.Equal("Anic", result.Prezime);
            Assert.Equal("9999999999999", result.JMBG);
            Assert.Equal("0699999999", result.Telefon);
            Assert.Equal("ana@example.com", result.Email);
            Assert.Equal("Beogradska 10", result.Adresa);
            Assert.True(result.Aktivan);
        }
    }
}
