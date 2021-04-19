using System;

namespace SharpLzo
{
    public static partial class Lzo
    {
        /// <summary>
        /// Decompresses the data.
        /// </summary>
        /// <param name="src">The data to decompress.</param>
        /// <returns>
        /// Returns a newly created array with the decompressed data if successful
        /// otherwise throws a <see cref="LzoException"/> with the corresponding error code.
        /// </returns>
        /// <exception cref="LzoException">Indicates that the decompression failed.</exception>
        /// <remarks>This method thread-safe.</remarks>
        public static byte[] Decompress(byte[] src)
        {
            return Decompress(src, src.Length * 10);
        }

        /// <summary>
        /// Decompresses the data.
        /// </summary>
        /// <param name="src">The data to decompress.</param>
        /// <param name="decompressedLength">
        /// The length of the decompressed data.
        /// This can also be a rough estimate as long as it is big enough.
        /// If the given length does not match the actual decompressed length the array will be resized accordingly.
        /// </param>
        /// <returns>
        /// Returns a newly created array with the decompressed data if successful
        /// otherwise throws a <see cref="LzoException"/> with the corresponding error code.
        /// </returns>
        /// <exception cref="LzoException">Indicates that the decompression failed.</exception>
        /// <remarks>This method thread-safe.</remarks>
        public static byte[] Decompress(byte[] src, int decompressedLength)
        {
            var result = TryDecompress(src, out var dst, decompressedLength);
            if (result != LzoResult.OK)
                throw new LzoException(result);

            return dst;
        }

        /// <summary>
        /// Tries to decompress the data.
        /// </summary>
        /// <param name="src">The data to decompress.</param>
        /// <param name="dst">A newly created array with the decompressed data.</param>
        /// <returns>Returns the result indicating wether the decompression was successful or not.</returns>
        /// <remarks>This method thread-safe.</remarks>
        public static LzoResult TryDecompress(byte[] src, out byte[] dst)
        {
            return TryDecompress(src, out dst, src.Length * 10);
        }

        /// <summary>
        /// Tries to decompress the data.
        /// </summary>
        /// <param name="src">The data to decompress.</param>
        /// <param name="dst">A newly created array with the decompressed data.</param>
        /// <param name="decompressedLength">
        /// The length of the decompressed data.
        /// This can also be a rough estimate as long as it is big enough.
        /// If the given length does not match the actual decompressed length the array will be resized accordingly.
        /// </param>
        /// <returns>Returns the result indicating wether the decompression was successful or not.</returns>
        /// <remarks>This method thread-safe.</remarks>
        public static LzoResult TryDecompress(byte[] src, out byte[] dst, int decompressedLength)
        {
            dst = new byte[decompressedLength];
            var result = TryDecompress(src, src.Length, dst, out var dstLength);
            if (result != LzoResult.OK)
            {
                dst = default;
                return result;
            }

            if (dstLength == 0)
                dst = Array.Empty<byte>();
            else if (dstLength != dst.Length)
                Array.Resize(ref dst, dstLength);

            return result;
        }

        /// <summary>
        /// Tries to decompress the data.
        /// </summary>
        /// <param name="src">The data to decompress.</param>
        /// <param name="srcLength">The length of <see cref="src"/>.</param>
        /// <param name="dst">The array where the decompressed data gets stored.</param>
        /// <param name="dstLength">The length of the decompressed data.</param>
        /// <returns>Returns the result indicating wether the decompression was successful.</returns>
        /// <remarks>This method can be used from multiple threads.</remarks>
        public static LzoResult TryDecompress(
            ReadOnlySpan<byte> src,
            int srcLength,
            Span<byte> dst,
            out int dstLength
        )
        {
            return LzoNative.DecompressSafe(src, srcLength, dst, out dstLength);
        }
    }
}
