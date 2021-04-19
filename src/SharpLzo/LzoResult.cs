namespace SharpLzo
{
    public enum LzoResult
    {
        OK = 0,
        Error = -1,
        OutOfMemory = -2,
        NotCompressible = -3,
        InputOverrun = -4,
        OutputOverrun = -5,
        LookbehinOverrun = -6,
        EOFNotFound = -7,
        InputNotConsumed = -8,
        NotYetImplemented = -9,
        InvalidArgument = -10
    }
}
