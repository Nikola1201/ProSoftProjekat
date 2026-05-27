using System;
using System.Data;
using System.Linq;
using Common.Domain;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain
{
    public class AdminTests
    {
        [Fact]
        public void TableName_returns_Admin()
            => Assert.Equal("Admin", new Admin().TableName);

        [Fact]
        public void Values_serializes_required_fields_in_order()
        {
            var a = SampleData.ValidAdmin();
            Assert.Equal("'Pera', 'Peric', 'pera', 'pera123', 'pera@auto.rs'", a.Values);
        }

        [Fact]
        public void TableKeyQuery_uses_AdminId()
        {
            var a = SampleData.ValidAdmin();
            Assert.Equal("AdminId = 1", a.TableKeyQuery);
        }

        [Fact]
        public void Update_builds_UPDATE_statement()
        {
            var a = SampleData.ValidAdmin();
            Assert.Contains("UPDATE Admin SET", a.Update);
            Assert.Contains("WHERE AdminId = 1", a.Update);
        }

        [Fact]
        public void Query_filters_by_username_and_password()
        {
            var a = SampleData.ValidAdmin();
            Assert.Equal("[Username] = 'pera' and [Lozinka] = 'pera123'", a.Query);
        }

        [Fact]
        public void GetReaderList_hydrates_admins_from_reader()
        {
            var table = new DataTable();
            table.Columns.Add("AdminId", typeof(int));
            table.Columns.Add("Ime", typeof(string));
            table.Columns.Add("Prezime", typeof(string));
            table.Columns.Add("Username", typeof(string));
            table.Columns.Add("Lozinka", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("DatumKreiranja", typeof(DateTime));
            table.Rows.Add(1, "Pera", "Peric", "pera", "pera123", "p@a.rs", new DateTime(2026, 1, 1));
            table.Rows.Add(2, "Mika", "Mikic", "mika", "mika123", "m@a.rs", new DateTime(2026, 2, 1));

            using var reader = table.CreateDataReader();
            var list = new Admin().GetReaderList(reader).Cast<Admin>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal("Pera", list[0].Ime);
            Assert.Equal("Mika", list[1].Ime);
        }

        [Fact]
        public void GetReaderResult_returns_first_row_or_null()
        {
            var table = new DataTable();
            table.Columns.Add("AdminId", typeof(int));
            table.Columns.Add("Ime", typeof(string));
            table.Columns.Add("Prezime", typeof(string));
            table.Columns.Add("Username", typeof(string));
            table.Columns.Add("Lozinka", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("DatumKreiranja", typeof(DateTime));

            using var emptyReader = table.CreateDataReader();
            Assert.Null(new Admin().GetReaderResult(emptyReader));

            table.Rows.Add(7, "Sava", "Savic", "sava", "sava123", "s@a.rs", new DateTime(2026, 3, 1));
            using var oneReader = table.CreateDataReader();
            var result = (Admin)new Admin().GetReaderResult(oneReader);
            Assert.Equal(7, result.AdminId);
            Assert.Equal("Sava", result.Ime);
        }
    }
}
