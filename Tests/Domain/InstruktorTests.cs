using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class InstruktorTests
    {
        private static DataTable InstruktorTable() => DataReaderBuilder.Empty(
            ("InstruktorId", typeof(int)),
            ("Ime", typeof(string)),
            ("Prezime", typeof(string)),
            ("JMBG", typeof(string)),
            ("Telefon", typeof(string)),
            ("Email", typeof(string)),
            ("DatumZaposlenja", typeof(DateTime)),
            ("Aktivan", typeof(bool)));

        [Fact]
        public void TableName_returns_Instruktor()
            => Assert.Equal("Instruktor", new Instruktor().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var i = SampleData.ValidInstruktor();
            Assert.Equal(
                "'Zika', 'Zikic', '9876543210987', '0659876543', 'zika@auto.rs', '2025-01-01',1",
                i.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_InstruktorId()
        {
            var i = SampleData.ValidInstruktor();
            Assert.Equal("InstruktorId = 1", i.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var i = SampleData.ValidInstruktor();
            Assert.Contains("UPDATE Instruktor SET", i.Update);
            Assert.Contains("Ime = 'Zika'", i.Update);
            Assert.Contains("Prezime = 'Zikic'", i.Update);
            Assert.Contains("JMBG = '9876543210987'", i.Update);
            Assert.Contains("Telefon = '0659876543'", i.Update);
            Assert.Contains("Email = 'zika@auto.rs'", i.Update);
            Assert.Contains("DatumZaposlenja = '2025-01-01'", i.Update);
            Assert.Contains("Aktivan = 1", i.Update);
            Assert.Contains("WHERE InstruktorId = 1", i.Update);
        }

        [Fact]
        public void Query_filters_by_JMBG()
        {
            var i = SampleData.ValidInstruktor();
            Assert.Equal("JMBG = '9876543210987'", i.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_instruktors_from_reader()
        {
            var table = InstruktorTable();
            table.Rows.Add(1, "Zika", "Zikic", "9876543210987", "0659876543", "zika@auto.rs",
                new DateTime(2025, 1, 1), true);
            table.Rows.Add(2, "Laza", "Lazic", "1111111111112", "0611111112", "laza@auto.rs",
                new DateTime(2024, 6, 1), false);

            using var reader = DataReaderBuilder.From(table);
            var list = new Instruktor().GetReaderList(reader).Cast<Instruktor>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].InstruktorId);
            Assert.Equal("Zika", list[0].Ime);
            Assert.Equal("Zikic", list[0].Prezime);
            Assert.Equal("9876543210987", list[0].JMBG);
            Assert.True(list[0].Aktivan);
            Assert.Equal(2, list[1].InstruktorId);
            Assert.Equal("Laza", list[1].Ime);
            Assert.Equal("1111111111112", list[1].JMBG);
            Assert.False(list[1].Aktivan);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(InstruktorTable());
            Assert.Null(new Instruktor().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = InstruktorTable();
            table.Rows.Add(3, "Sava", "Savic", "5555555555555", "0655555555", "sava@auto.rs",
                new DateTime(2023, 3, 15), true);

            using var reader = DataReaderBuilder.From(table);
            var result = (Instruktor)new Instruktor().GetReaderResult(reader);

            Assert.Equal(3, result.InstruktorId);
            Assert.Equal("Sava", result.Ime);
            Assert.Equal("Savic", result.Prezime);
            Assert.Equal("5555555555555", result.JMBG);
            Assert.Equal("0655555555", result.Telefon);
            Assert.Equal("sava@auto.rs", result.Email);
            Assert.Equal(new DateTime(2023, 3, 15), result.DatumZaposlenja);
            Assert.True(result.Aktivan);
        }
    }
}
