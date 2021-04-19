using System;

namespace SharpLzo
{
    public static partial class Lzo
    {
        /// <summary>
        /// Compresses the data with <see cref="CompressionMode.Lzo1x_1"/>.
        /// </summary>
        /// <param name="src">The data to compress.</param>
        /// <returns>
        /// Returns a newly created array with the compressed data if successful
        /// otherwise throws a <see cref="LzoException"/> with the corresponding error code.
        /// </returns>
        /// <exception cref="LzoException">Indicates that the compression failed.</exception>
        /// <remarks>This method <b>is not</b> thread-safe.</remarks>
        public static byte[] Compress(byte[] src)
        {
            return Compress(CompressionMode.Lzo1x_1, src);
        }

        /// <summary>
        /// Compresses the data with the specified <see cref="CompressionMode"/>.
        /// </summary>
        /// <param name="mode">The compression mode.</param>
        /// <param name="src">The data to compress.</param>
        /// <returns>
        /// Returns a newly created array with the compressed data if successful
        /// otherwise throws a <see cref="LzoException"/> with the corresponding error code.
        /// </returns>
        /// <exception cref="LzoException">Indicates that the compression failed.</exception>
        /// <remarks>This method <b>is not</b> thread-safe.</remarks>
        public static byte[] Compress(CompressionMode mode, byte[] src)
        {
            var result = TryCompress(mode, src, out var outData);
            if (result != LzoResult.OK)
                throw new LzoException(result);

            return outData;
        }

        /// <summary>
        /// Tries to compress the data with <see cref="CompressionMode.Lzo1x_1"/>.
        /// </summary>
        /// <param name="src">The data to compress.</param>
        /// <param name="dst">A newly created array with the compressed data.</param>
        /// <returns>Returns the result indicating wether the compression was successful or not.</returns>
        /// <remarks>This method <b>is not</b> thread-safe.</remarks>
        public static LzoResult TryCompress(byte[] src, out byte[] dst)
        {
            return TryCompress(CompressionMode.Lzo1x_1, src, out dst);
        }

        /// <summary>
        /// Tries to compress the data with the specified <see cref="CompressionMode"/>.
        /// </summary>
        /// <param name="mode">The compression mode.</param>
        /// <param name="src">The data to compress.</param>
        /// <param name="dst">A newly created array with the compressed data.</param>
        /// <returns>Returns the result indicating wether the compression was successful or not.</returns>
        /// <remarks>This method <b>is not</b> thread-safe.</remarks>
        public static LzoResult TryCompress(CompressionMode mode, byte[] src, out byte[] dst)
        {
            // See LZO examples: http://www.oberhumer.com/opensource/lzo/
            var tmpDstLength = src.Length + src.Length / 16 + 64 + 3;
            var tmpDst = new byte[tmpDstLength];

            var result = TryCompress(mode, src, src.Length, tmpDst, out var dstLength, s_workMemory);
            if (result != LzoResult.OK)
            {
                dst = default;
                return result;
            }

            Array.Resize(ref tmpDst, dstLength);
            dst = tmpDst;
            return result;
        }

        /// <summary>
        /// Tries to compress the data with the specified <see cref="CompressionMode"/>.
        /// </summary>
        /// <param name="mode">The compression mode.</param>
        /// <param name="src">The data to compress.</param>
        /// <param name="srcLength">The length of <see cref="src"/>.</param>
        /// <param name="dst">The array where the compressed data gets stored.
        ///  Make sure the array is big enough. The official lzo example suggests <code>srcLength + srcLength / 16 + 64 + 3</code>.
        /// </param>
        /// <param name="dstLength">The length of the compressed data.</param>
        /// <returns>Returns the result indicating wether the compression was successful.</returns>
        /// <remarks>This method <b>is not</b> thread-safe. Use <see cref="TryCompress(CompressionMode,ReadOnlySpan&lt;byte&gt;,int,Span&lt;byte&gt;, out int, byte[])"/> for multithreading.</remarks>
        public static LzoResult TryCompress(
            CompressionMode mode,
            ReadOnlySpan<byte> src,
            int srcLength,
            Span<byte> dst,
            out int dstLength
        )
        {
            return TryCompress(mode, src, src.Length, dst, out dstLength, s_workMemory);
        }

        /// <summary>
        /// Tries to compress the data with the specified <see cref="CompressionMode"/>.
        /// </summary>
        /// <param name="mode">The compression mode.</param>
        /// <param name="src">The data to compress.</param>
        /// <param name="srcLength">The length of <see cref="src"/>.</param>
        /// <param name="dst">The array where the compressed data gets stored.
        ///  Make sure the array is big enough. The official lzo example suggests <code>srcLength + srcLength / 16 + 64 + 3</code>.
        /// </param>
        /// <param name="dstLength">The length of the compressed data.</param>
        /// <param name="workMemory">The internal work memory for lzo.
        ///  The array should be at least the size of <see cref="Lzo.WorkMemorySize"/>.
        /// </param>
        /// <returns>Returns the result indicating wether the compression was successful.</returns>
        /// <remarks>This method can be used from multiple threads <b>as long as each concurrent operation has its own</b> <see cref="workMemory"/>.</remarks>
        public static LzoResult TryCompress(
            CompressionMode mode,
            ReadOnlySpan<byte> src,
            int srcLength,
            Span<byte> dst,
            out int dstLength,
            byte[] workMemory
        )
        {
            return LzoNative.Compress(mode, src, srcLength, dst, out dstLength, workMemory);
        }
    }
}
