using System;

namespace Common.Communication
{
    [Serializable]
    public class Request
    {
        public Operation Operation { get; set; }
        public object Argument { get; set; }
    }
}
