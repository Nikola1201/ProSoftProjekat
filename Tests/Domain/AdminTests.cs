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
        private static DataTable AdminTable() => DataReaderBuilder.Empty(
            ("AdminId", typeof(int)),
            ("Ime", typeof(string)),
            ("Prezime", typeof(string)),
            ("Username", typeof(string)),
            ("Lozinka", typeof(string)),
            ("Email", typeof(string)),
            ("DatumKreiranja", typeof(DateTime)));

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
        public void Update_builds_UPDATE_statement_with_all_fields()
        {
            var a = SampleData.ValidAdmin();
            Assert.Contains("UPDATE Admin SET", a.Update);
            Assert.Contains("Ime = 'Pera'", a.Update);
            Assert.Contains("Prezime = 'Peric'", a.Update);
            Assert.Contains("Username = 'pera'", a.Update);
            Assert.Contains("Lozinka = 'pera123'", a.Update);
            Assert.Contains("Email = 'pera@auto.rs'", a.Update);
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
            var table = AdminTable();
            table.Rows.Add(1, "Pera", "Peric", "pera", "pera123", "p@a.rs", new DateTime(2026, 1, 1));
            table.Rows.Add(2, "Mika", "Mikic", "mika", "mika123", "m@a.rs", new DateTime(2026, 2, 1));

            using var reader = DataReaderBuilder.From(table);
            var list = new Admin().GetReaderList(reader).Cast<Admin>().ToList();

            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].AdminId);
            Assert.Equal("Pera", list[0].Ime);
            Assert.Equal("Peric", list[0].Prezime);
            Assert.Equal("pera", list[0].Username);
            Assert.Equal(2, list[1].AdminId);
            Assert.Equal("Mika", list[1].Ime);
        }

        [Fact]
        public void GetReaderResult_returns_null_when_reader_empty()
        {
            using var reader = DataReaderBuilder.From(AdminTable());
            Assert.Null(new Admin().GetReaderResult(reader));
        }

        [Fact]
        public void GetReaderResult_hydrates_first_row()
        {
            var table = AdminTable();
            table.Rows.Add(7, "Sava", "Savic", "sava", "sava123", "s@a.rs", new DateTime(2026, 3, 1));

            using var reader = DataReaderBuilder.From(table);
            var result = (Admin)new Admin().GetReaderResult(reader);

            Assert.Equal(7, result.AdminId);
            Assert.Equal("Sava", result.Ime);
            Assert.Equal("Savic", result.Prezime);
            Assert.Equal("sava", result.Username);
            Assert.Equal("sava123", result.Lozinka);
        }
    }
}
