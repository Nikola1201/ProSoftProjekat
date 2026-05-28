using System;

namespace Common.Validation
{
    /// <summary>Izuzetak koji sistemska operacija baca kada ulazni podaci ne zadovoljavaju ograničenja.</summary>
    [Serializable]
    public class ValidacijaException : Exception
    {
        /// <summary>Inicijalizuje novu instancu klase <see cref="ValidacijaException"/> sa opisom greške validacije.</summary>
        /// <param name="message">Poruka koja opisuje koji uslov validacije nije ispunjen.</param>
        public ValidacijaException(string message) : base(message)
        {
        }
    }
}
