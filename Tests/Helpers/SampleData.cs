using System;
using Common.Domain;

namespace Tests.Helpers
{
    public static class SampleData
    {
        public static Admin ValidAdmin() => new Admin
        {
            AdminId = 1,
            Ime = "Pera",
            Prezime = "Peric",
            Username = "pera",
            Lozinka = "pera123",
            Email = "pera@auto.rs",
            DatumKreiranja = new DateTime(2026, 1, 1)
        };

        public static Kandidat ValidKandidat() => new Kandidat
        {
            KandidatId = 1,
            Ime = "Mika",
            Prezime = "Mikic",
            JMBG = "1234567890123",
            Telefon = "0641234567",
            Email = "mika@example.com",
            Adresa = "Knez Mihailova 1, Beograd",
            DatumUpisa = new DateTime(2026, 1, 1),
            Aktivan = true
        };

        public static Instruktor ValidInstruktor() => new Instruktor
        {
            InstruktorId = 1,
            Ime = "Zika",
            Prezime = "Zikic",
            JMBG = "9876543210987",
            Telefon = "0659876543",
            Email = "zika@auto.rs",
            DatumZaposlenja = new DateTime(2025, 1, 1),
            Aktivan = true
        };

        public static Kategorija ValidKategorija() => new Kategorija
        {
            KategorijaID = 1,
            NazivKategorije = "B"
        };

        public static Vozilo ValidVozilo() => new Vozilo
        {
            VoziloId = 1,
            Marka = "Skoda",
            Model = "Fabia",
            Godiste = 2020,
            Tablica = "BG123AB",
            KategorijaID = 1,
            Aktivno = true
        };

        public static PaketObuke ValidPaketObuke() => new PaketObuke
        {
            PaketId = 1,
            Naziv = "Standardni B paket",
            BrojCasova = 40,
            Cena = 75000m,
            Opis = "",
            Kategorija = ValidKategorija()
        };

        public static Upis ValidUpis() => new Upis
        {
            UpisId = 1,
            KandidatId = 1,
            PaketId = 1,
            DatumUpisa = new DateTime(2026, 1, 1),
            Status = "aktivan"
        };

        public static Placanje ValidPlacanje() => new Placanje
        {
            PlacanjeId = 1,
            UpisId = 1,
            Iznos = 30000m,
            DatumPlacanja = new DateTime(2026, 1, 5),
            NacinPlacanja = "gotovina",
            Napomena = "prva rata"
        };

        public static CasVoznje ValidCasVoznje() => new CasVoznje
        {
            CasId = 1,
            UpisId = 1,
            InstruktorId = 1,
            VoziloId = 1,
            DatumCas = new DateTime(2026, 2, 1, 10, 0, 0),
            TrajanjMin = 45,
            Status = "zakazan",
            Napomena = ""
        };

        public static Ispit ValidIspit() => new Ispit
        {
            IspitId = 1,
            UpisId = 1,
            Tip = "teorijski",
            Rezultat = "polozio",
            DatumIspita = new DateTime(2026, 3, 1),
            Napomena = ""
        };

        public static InstrKat ValidInstrKat() => new InstrKat
        {
            InstruktorId = 1,
            KategorijaID = 1,
            DatumDodele = new DateTime(2025, 1, 1),
            Aktivno = true
        };
    }
}
