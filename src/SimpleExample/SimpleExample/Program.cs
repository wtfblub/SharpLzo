using System;
using System.Text;
using SharpLzo;

namespace SimpleExample
{
    internal static class Program
    {
        private static void Main()
        {
            var sample = Encoding.UTF8.GetBytes("Hello World");
            var workMemory = new byte[Lzo.WorkMemorySize];
            var compressed = Lzo.Compress(CompressionMode.Lzo1x_999, sample);
            var decompressed = Lzo.Decompress(compressed);
            Console.WriteLine("sample: {0}", BitConverter.ToString(sample));
            Console.WriteLine("compressed: {0}", BitConverter.ToString(compressed));
            Console.WriteLine("decompressed: {0}", BitConverter.ToString(decompressed));
        }
    }
}
