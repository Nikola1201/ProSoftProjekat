using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class VoziloTests
    {
        private static DataTable VoziloTable() => DataReaderBuilder.Empty(
            ("VoziloId", typeof(int)),
            ("Marka", typeof(string)),
            ("Model", typeof(string)),
            ("Godiste", typeof(int)),
            ("Tablica", typeof(string)),
            ("KategorijaID", typeof(int)),
            ("Aktivno", typeof(bool)));

        [Fact]
        public void TableName_returns_Vozilo()
            => Assert.Equal("Vozilo", new Vozilo().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var v = SampleData.ValidVozilo();
            Assert.Equal("'Skoda', 'Fabia', 2020, 'BG123AB', 1", v.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_VoziloId()
        {
            var v = SampleData.ValidVozilo();
            Assert.Equal("VoziloId = 1", v.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var v = SampleData.ValidVozilo();
            Assert.Contains("UPDATE Vozilo SET", v.Update);
            Assert.Contains("Marka = 'Skoda'", v.Update);
            Assert.Contains("Model = 'Fabia'", v.Update);
            Assert.Contains("Godiste = 2020", v.Update);
            Assert.Contains("Tablica = 'BG123AB'", v.Update);
            Assert.Contains("KategorijaID = 1", v.Update);
            Assert.Contains("Aktivno = 1", v.Update);
            Assert.Contains("WHERE VoziloId = 1", v.Update);
        }

        [Fact]
        public void Query_filters_by_Tablica()
        {
            var v = SampleData.ValidVozilo();
            Assert.Equal("Tablica = 'BG123AB'", v.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_vozilos_from_reader()
        {
            var table = VoziloTable();
            table.Rows.Add(1, "Skoda", "Fabia", 2020, "BG123AB", 1, true);
            table.Rows.Add(2, "Volkswagen", "Golf", 2019, "NS456CD", 2, false);

            using var reader = DataReaderBuilder.From(table);
            var list = new Vozilo().GetReaderList(reader).Cast<Vozilo>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].VoziloId);
            Assert.Equal("Skoda", list[0].Marka);
            Assert.Equal("Fabia", list[0].Model);
            Assert.Equal(2020, list[0].Godiste);
            Assert.Equal("BG123AB", list[0].Tablica);
            Assert.Equal(1, list[0].KategorijaID);
            Assert.True(list[0].Aktivno);
            Assert.Equal(2, list[1].VoziloId);
            Assert.Equal("NS456CD", list[1].Tablica);
            Assert.Equal(2, list[1].KategorijaID);
            Assert.False(list[1].Aktivno);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(VoziloTable());
            Assert.Null(new Vozilo().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = VoziloTable();
            table.Rows.Add(5, "Renault", "Clio", 2018, "KG789EF", 3, true);

            using var reader = DataReaderBuilder.From(table);
            var result = (Vozilo)new Vozilo().GetReaderResult(reader);

            Assert.Equal(5, result.VoziloId);
            Assert.Equal("Renault", result.Marka);
            Assert.Equal("Clio", result.Model);
            Assert.Equal(2018, result.Godiste);
            Assert.Equal("KG789EF", result.Tablica);
            Assert.Equal(3, result.KategorijaID);
            Assert.True(result.Aktivno);
        }
    }
}
