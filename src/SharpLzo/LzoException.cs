using System;

namespace SharpLzo
{
    public class LzoException : Exception
    {
        public LzoResult Result { get; }

        public LzoException(LzoResult result)
            : this(result, "")
        {
        }

        public LzoException(LzoResult result, string message)
            : base($"{message}\nresult={result}(0x{result:X})")
        {
            Result = result;
        }
    }
}
