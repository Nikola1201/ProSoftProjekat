namespace Common.Communication
{
    /// <summary>Wire envelope koji server vraća klijentu: rezultat ili poruka greške.</summary>
    public class Response
    {
        /// <summary>Rezultat operacije; <see langword="null"/> kada operacija ne vraća podatke ili je došlo do greške.</summary>
        public object? Result { get; set; }
        /// <summary>Poruka greške; <see langword="null"/> kada je operacija uspešno izvršena.</summary>
        public string? ErrorMessage { get; set; }
    }
}
