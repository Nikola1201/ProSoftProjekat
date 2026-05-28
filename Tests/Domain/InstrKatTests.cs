using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class InstrKatTests
    {
        private static DataTable InstrKatTable() => DataReaderBuilder.Empty(
            ("InstruktorId", typeof(int)),
            ("KategorijaID", typeof(int)),
            ("DatumDodele", typeof(DateTime)),
            ("Aktivno", typeof(bool)));

        [Fact]
        public void TableName_returns_InstrKat()
            => Assert.Equal("InstrKat", new InstrKat().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var ik = SampleData.ValidInstrKat();
            Assert.Equal("1, 1, '2025-01-01 00:00:00', 1", ik.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_composite_primary_key()
        {
            var ik = SampleData.ValidInstrKat();
            Assert.Equal("InstruktorId = 1 AND KategorijaID = 1", ik.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement_with_mutable_fields_and_composite_where()
        {
            var ik = SampleData.ValidInstrKat();
            Assert.Contains("UPDATE InstrKat SET", ik.Update);
            Assert.Contains("DatumDodele = '2025-01-01 00:00:00'", ik.Update);
            Assert.Contains("Aktivno = 1", ik.Update);
            Assert.Contains("WHERE InstruktorId = 1 AND KategorijaID = 1", ik.Update);
        }

        [Fact]
        public void Query_filters_by_InstruktorId()
        {
            var ik = SampleData.ValidInstrKat();
            Assert.Equal("InstruktorId = 1", ik.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_instrkat_from_reader()
        {
            var table = InstrKatTable();
            table.Rows.Add(1, 1, new DateTime(2025, 1, 1), true);
            table.Rows.Add(2, 3, new DateTime(2024, 6, 15), false);

            using var reader = DataReaderBuilder.From(table);
            var list = new InstrKat().GetReaderList(reader).Cast<InstrKat>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].InstruktorId);
            Assert.Equal(1, list[0].KategorijaID);
            Assert.Equal(new DateTime(2025, 1, 1), list[0].DatumDodele);
            Assert.True(list[0].Aktivno);
            // Row-2 spot-check: different instruktor and kategorija to catch swap bugs
            Assert.Equal(2, list[1].InstruktorId);
            Assert.Equal(3, list[1].KategorijaID);
            Assert.Equal(new DateTime(2024, 6, 15), list[1].DatumDodele);
            Assert.False(list[1].Aktivno);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            // InstrKat.GetReaderResult calls reader.Read() internally
            using var reader = DataReaderBuilder.From(InstrKatTable());
            Assert.Null(new InstrKat().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = InstrKatTable();
            table.Rows.Add(5, 2, new DateTime(2023, 9, 10), false);

            // InstrKat.GetReaderResult calls reader.Read() internally — do NOT pre-advance
            using var reader = DataReaderBuilder.From(table);
            var result = (InstrKat)new InstrKat().GetReaderResult(reader);

            Assert.Equal(5, result.InstruktorId);
            Assert.Equal(2, result.KategorijaID);
            Assert.Equal(new DateTime(2023, 9, 10), result.DatumDodele);
            Assert.False(result.Aktivno);
        }
    }
}
