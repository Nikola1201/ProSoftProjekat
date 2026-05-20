using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Common.Domain
{
    [Serializable]
    public class Vozilo : IEntity
    {
        public int VoziloId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Godiste { get; set; }
        public string Tablica { get; set; }
        public int KategorijaID { get; set; }
        public bool Aktivno { get; set; }

        public string TableName => "Vozilo";

        public string Values =>
            $"'{Marka}', '{Model}', {Godiste}, '{Tablica}', {KategorijaID}";

        public object Query =>
            $"INSERT INTO Vozilo (Marka, Model, Godiste, Tablica, KategorijaID) " +
            $"VALUES ({Values})";

        public object TableKeyColumn => "VoziloId";

        public object SearchQuery =>
            $"SELECT * FROM Vozilo WHERE Marka LIKE '%{Marka}%' OR Model LIKE '%{Model}%'";

        public object TableKeyQuery =>
            $"{TableKeyColumn} = {VoziloId}";

        public object Update =>
            $"UPDATE Vozilo SET " +
            $"Marka = '{Marka}', " +
            $"Model = '{Model}', " +
            $"Godiste = {Godiste}, " +
            $"Tablica = '{Tablica}', " +
            $"KategorijaID = {KategorijaID}, " +
            $"Aktivno = {(Aktivno ? 1 : 0)} " +
            $"WHERE VoziloId = {VoziloId}";

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var list = new List<IEntity>();
            while (reader.Read())
            {
                list.Add(new Vozilo
                {
                    VoziloId = (int)reader["VoziloId"],
                    Marka = reader["Marka"].ToString(),
                    Model = reader["Model"].ToString(),
                    Godiste = (int)reader["Godiste"],
                    Tablica = reader["Tablica"].ToString(),
                    KategorijaID = (int)reader["KategorijaID"],
                    Aktivno = (bool)reader["Aktivno"]
                });
            }
            return list;
        }

        public IEntity GetReaderResult(SqlDataReader reader)
        {
            if (reader.Read())
            {
                return new Vozilo
                {
                    VoziloId = (int)reader["VoziloId"],
                    Marka = reader["Marka"].ToString(),
                    Model = reader["Model"].ToString(),
                    Godiste = (int)reader["Godiste"],
                    Tablica = reader["Tablica"].ToString(),
                    KategorijaID = (int)reader["KategorijaID"],
                    Aktivno = (bool)reader["Aktivno"]
                };
            }
            return null;
        }

        public override string ToString()
        {
            return $"{Marka} {Model} ({Tablica}) - Kategorija {KategorijaID}";
        }
    }
}
