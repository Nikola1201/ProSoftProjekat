using System;

namespace Common.Communication
{
    /// <summary>Izuzetak koji klijent baca kada serverska konekcija nestane ili bude prekinuta.</summary>
    public class ConnectionLostException : Exception
    {
        /// <summary>Inicijalizuje novu instancu klase <see cref="ConnectionLostException"/> sa izvornim izuzetkom.</summary>
        /// <param name="inner">Izvorni izuzetak koji je uzrokovao gubitak konekcije.</param>
        public ConnectionLostException(Exception inner)
            : base("Veza sa serverom je izgubljena.", inner) { }
    }
}
