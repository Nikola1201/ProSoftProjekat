using System;

namespace Common.Validation
{
    [Serializable]
    public class ValidacijaException : Exception
    {
        public ValidacijaException(string message) : base(message)
        {
        }
    }
}
