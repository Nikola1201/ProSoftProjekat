using System;

namespace Common.Communication
{
    [Serializable]
    public class Response
    {
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
}
