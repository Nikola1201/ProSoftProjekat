namespace Common.Communication
{
    /// <summary>Wire envelope koji klijent šalje serveru: operacija + opciono payload.</summary>
    public class Request
    {
        /// <summary>Tip operacije koja se traži od servera.</summary>
        public Operation Operation { get; set; }
        /// <summary>Payload — argument operacije (DTO, entitet, kriterijum, itd.).</summary>
        public object? Argument { get; set; }
    }
}
