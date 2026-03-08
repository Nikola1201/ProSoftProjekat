using System.Collections.Generic;
using System.Data.SqlClient;

namespace Common.Domain
{
    public class Vozilo : IEntity
    {
        public int VoziloId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Godiste { get; set; }
        public string Tablica { get; set; }
        public string Kategorija { get; set; }  // 'A', 'B', 'C'
        public bool Aktivno { get; set; }

        public string TableName => "Vozilo";

        public string Values =>
            $"'{Marka}', '{Model}', {Godiste}, '{Tablica}', '{Kategorija}'";

        public object Query =>
            $"INSERT INTO Vozilo (Marka, Model, Godiste, Tablica, Kategorija) " +
            $"VALUES ({Values})";

        public object TableKeyColumn => "VoziloId";

        public object SearchQuery =>
            $"SELECT * FROM Vozilo WHERE Marka LIKE '%{Marka}%' OR Model LIKE '%{Model}%'";

        public object TableKeyQuery =>
            $"SELECT * FROM Vozilo WHERE VoziloId = {VoziloId}";

        public object Update =>
            $"UPDATE Vozilo SET " +
            $"Marka = '{Marka}', " +
            $"Model = '{Model}', " +
            $"Godiste = {Godiste}, " +
            $"Tablica = '{Tablica}', " +
            $"Kategorija = '{Kategorija}', " +
            $"Aktivno = {(Aktivno ? 1 : 0)} " +
            $"WHERE VoziloId = {VoziloId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
                list.Add(GetReaderResult(reader));
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            return new Vozilo
            {
                VoziloId = (int)reader["VoziloId"],
                Marka = reader["Marka"].ToString(),
                Model = reader["Model"].ToString(),
                Godiste = (int)reader["Godiste"],
                Tablica = reader["Tablica"].ToString(),
                Kategorija = reader["Kategorija"].ToString(),
                Aktivno = (bool)reader["Aktivno"]
            };
        }
    }
}
