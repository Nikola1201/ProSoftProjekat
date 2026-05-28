using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class PaketObukeTests
    {
        // GetReaderList hydrates KategorijaID from a joined column — include it in the table.
        private static DataTable PaketObukeTable() => DataReaderBuilder.Empty(
            ("PaketId", typeof(int)),
            ("Naziv", typeof(string)),
            ("KategorijaID", typeof(int)),
            ("BrojCasova", typeof(int)),
            ("Cena", typeof(decimal)),
            ("Opis", typeof(string)));

        [Fact]
        public void TableName_returns_PaketObuke()
            => Assert.Equal("PaketObuke", new PaketObuke().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var p = SampleData.ValidPaketObuke();
            // Values: '{Naziv}', '{Kategorija.KategorijaID}', {BrojCasova}, {Cena}, '{Opis}'
            Assert.Equal("'Standardni B paket', '1', 40, 75000, ''", p.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_PaketId()
        {
            var p = SampleData.ValidPaketObuke();
            Assert.Equal("PaketId = 1", p.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var p = SampleData.ValidPaketObuke();
            Assert.Contains("UPDATE PaketObuke SET", p.Update);
            Assert.Contains("Naziv = 'Standardni B paket'", p.Update);
            Assert.Contains("KategorijaID = '1'", p.Update);
            Assert.Contains("BrojCasova = 40", p.Update);
            Assert.Contains("Cena = 75000", p.Update);
            Assert.Contains("Opis = ''", p.Update);
            Assert.Contains("WHERE PaketId = 1", p.Update);
        }

        [Fact]
        public void Query_filters_by_Naziv()
        {
            var p = SampleData.ValidPaketObuke();
            Assert.Equal("Naziv = 'Standardni B paket'", p.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_paketObukes_from_reader()
        {
            var table = PaketObukeTable();
            table.Rows.Add(1, "Standardni B paket", 1, 40, 75000m, "");
            table.Rows.Add(2, "AM paket", 2, 20, 45000m, "Za motocikle");

            using var reader = DataReaderBuilder.From(table);
            var list = new PaketObuke().GetReaderList(reader).Cast<PaketObuke>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].PaketId);
            Assert.Equal("Standardni B paket", list[0].Naziv);
            Assert.Equal(1, list[0].Kategorija.KategorijaID);
            Assert.Equal(40, list[0].BrojCasova);
            Assert.Equal(75000m, list[0].Cena);
            Assert.Equal(2, list[1].PaketId);
            Assert.Equal("AM paket", list[1].Naziv);
            Assert.Equal(2, list[1].Kategorija.KategorijaID);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(PaketObukeTable());
            Assert.Null(new PaketObuke().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = PaketObukeTable();
            table.Rows.Add(3, "Napredni B paket", 1, 60, 95000m, "Intenzivna obuka");

            using var reader = DataReaderBuilder.From(table);
            var result = (PaketObuke)new PaketObuke().GetReaderResult(reader);

            Assert.Equal(3, result.PaketId);
            Assert.Equal("Napredni B paket", result.Naziv);
            Assert.Equal(1, result.Kategorija.KategorijaID);
            Assert.Equal(60, result.BrojCasova);
            Assert.Equal(95000m, result.Cena);
            Assert.Equal("Intenzivna obuka", result.Opis);
        }
    }
}
