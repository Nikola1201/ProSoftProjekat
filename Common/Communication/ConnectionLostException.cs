using System;

namespace Common.Communication
{
    public class ConnectionLostException : Exception
    {
        public ConnectionLostException(Exception inner)
            : base("Veza sa serverom je izgubljena.", inner) { }
    }
}
