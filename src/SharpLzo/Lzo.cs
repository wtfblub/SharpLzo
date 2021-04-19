namespace SharpLzo
{
    public static partial class Lzo
    {
        public const int WorkMemorySize = 14 * 16384 * sizeof(short);

        private static readonly byte[] s_workMemory = new byte[WorkMemorySize];

        public static uint Version => LzoNative.Version();

        static Lzo()
        {
            var result = LzoNative.Init();
            if (result != LzoResult.OK)
                throw new LzoException(result, "Failed to initialize lzo library");
        }
    }
}
