using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Common.Validation
{
    /// <summary>
    /// Pomoćne provere za validaciju vrednosti atributa domenskih klasa.
    /// Svaka metoda baca <see cref="ValidacijaException"/> kada uslov nije ispunjen.
    /// </summary>
    public static class Guard
    {
        /// <summary>Vrednost ne sme biti null, prazna ili samo razmaci.</summary>
        public static void Obavezno(string? vrednost, string polje)
        {
            if (string.IsNullOrWhiteSpace(vrednost))
                throw new ValidacijaException($"{polje} je obavezno polje.");
        }

        /// <summary>Ako je vrednost postavljena, ne sme prelaziti <paramref name="max"/> karaktera.</summary>
        public static void MaxDuzina(string? vrednost, int max, string polje)
        {
            if (vrednost != null && vrednost.Length > max)
                throw new ValidacijaException($"{polje} može imati najviše {max} karaktera.");
        }

        /// <summary>Vrednost je obavezna i mora imati dužinu u intervalu [min, max].</summary>
        public static void StringDuzina(string? vrednost, int min, int max, string polje)
        {
            Obavezno(vrednost, polje);
            if (vrednost!.Length < min || vrednost.Length > max)
                throw new ValidacijaException($"{polje} mora imati između {min} i {max} karaktera.");
        }

        /// <summary>Vrednost ne sme sadržati nijedan razmak / belinu.</summary>
        public static void BezRazmaka(string? vrednost, string polje)
        {
            if (vrednost != null && vrednost.Any(char.IsWhiteSpace))
                throw new ValidacijaException($"{polje} ne sme sadržati razmake.");
        }

        /// <summary>Celobrojna vrednost mora biti u intervalu [min, max].</summary>
        public static void Opseg(int vrednost, int min, int max, string polje)
        {
            if (vrednost < min || vrednost > max)
                throw new ValidacijaException($"{polje} mora biti između {min} i {max}.");
        }

        /// <summary>Celobrojna vrednost mora biti veća od nule (npr. strani ključ).</summary>
        public static void Pozitivan(int vrednost, string polje)
        {
            if (vrednost <= 0)
                throw new ValidacijaException($"{polje} mora biti veće od nule.");
        }

        /// <summary>Decimalna vrednost mora biti veća od nule.</summary>
        public static void Pozitivan(decimal vrednost, string polje)
        {
            if (vrednost <= 0)
                throw new ValidacijaException($"{polje} mora biti veće od nule.");
        }

        /// <summary>Datum mora biti postavljen (ne sme biti podrazumevani <c>default(DateTime)</c>).</summary>
        public static void Datum(DateTime vrednost, string polje)
        {
            if (vrednost == default)
                throw new ValidacijaException($"{polje} mora biti validan datum.");
        }

        /// <summary>Vrednost je obavezna i mora biti ispravna email adresa.</summary>
        public static void Email(string? vrednost, string polje)
        {
            Obavezno(vrednost, polje);
            bool valid;
            try { valid = new MailAddress(vrednost!).Address == vrednost; }
            catch { valid = false; }
            if (!valid)
                throw new ValidacijaException($"{polje} nije u ispravnom formatu email adrese.");
        }

        /// <summary>Vrednost je obavezna i mora sadržati tačno <paramref name="n"/> cifara.</summary>
        public static void TacanBrojCifara(string? vrednost, int n, string polje)
        {
            Obavezno(vrednost, polje);
            if (vrednost!.Length != n || !vrednost.All(char.IsDigit))
                throw new ValidacijaException($"{polje} mora sadržati tačno {n} cifara.");
        }

        /// <summary>Vrednost je obavezna i mora biti 6–20 cifara uz opcioni vodeći '+'.</summary>
        public static void Telefon(string? vrednost, string polje)
        {
            Obavezno(vrednost, polje);
            if (!Regex.IsMatch(vrednost!, @"^\+?\d{6,20}$"))
                throw new ValidacijaException(
                    $"{polje} mora sadržati 6–20 cifara uz opcioni vodeći '+'.");
        }

        /// <summary>Vrednost mora biti jedna od dozvoljenih (case-insensitive).</summary>
        public static void JednaOd(string? vrednost, string polje, params string[] dozvoljene)
        {
            if (string.IsNullOrWhiteSpace(vrednost) ||
                !dozvoljene.Any(d => string.Equals(d, vrednost, StringComparison.OrdinalIgnoreCase)))
                throw new ValidacijaException(
                    $"{polje} mora biti jedna od dozvoljenih vrednosti: {string.Join(", ", dozvoljene)}.");
        }

        /// <summary>Referenca (npr. navigacioni objekat) ne sme biti null.</summary>
        public static void NijeNull(object? vrednost, string polje)
        {
            if (vrednost == null)
                throw new ValidacijaException($"{polje} je obavezno polje.");
        }
    }
}
