using System;
using FluentAssertions;
using Xunit;

namespace SharpLzo.Tests
{
    public class CompressionTests
    {
        [Fact]
        public void CanCompress()
        {
            var data = GetData();
            var compressed = Lzo.Compress(data);

            compressed.Should().NotBeNull().And.NotBeEquivalentTo(data);
        }

        [Theory]
        [InlineData(CompressionMode.Lzo1x_1)]
        [InlineData(CompressionMode.Lzo1x_999)]
        public void CanCompressWithMode(CompressionMode compressionMode)
        {
            var data = GetData();
            var compressed = Lzo.Compress(compressionMode, data);

            compressed.Should().NotBeNull().And.NotBeEquivalentTo(data);
        }

        [Fact]
        public void CanTryCompress()
        {
            var data = GetData();
            var result = Lzo.TryCompress(data, out var compressed);

            result.Should().Be(LzoResult.OK);
            compressed.Should().NotBeNull().And.NotBeEquivalentTo(data);
        }

        [Theory]
        [InlineData(CompressionMode.Lzo1x_1)]
        [InlineData(CompressionMode.Lzo1x_999)]
        public void CanTryCompressWithMode(CompressionMode compressionMode)
        {
            var data = GetData();
            var result = Lzo.TryCompress(compressionMode, data, out var compressed);

            result.Should().Be(LzoResult.OK);
            compressed.Should().NotBeNull().And.NotBeEquivalentTo(data);
        }

        [Theory]
        [InlineData(CompressionMode.Lzo1x_1)]
        [InlineData(CompressionMode.Lzo1x_999)]
        public void CanTryCompressWithSpan(CompressionMode compressionMode)
        {
            var data = GetData();
            var compressed = new byte[data.Length + data.Length / 16 + 64 + 3];
            var result = Lzo.TryCompress(compressionMode, data, data.Length, compressed, out var compressedLength);

            result.Should().Be(LzoResult.OK);
            compressedLength.Should().BeGreaterThan(0);
            compressed.Should().NotBeNull().And.NotBeEquivalentTo(data);
        }

        [Theory]
        [InlineData(CompressionMode.Lzo1x_1)]
        [InlineData(CompressionMode.Lzo1x_999)]
        public void CanTryCompressWithSpanAndWorkMemory(CompressionMode compressionMode)
        {
            var data = GetData();
            var workMemory = new byte[Lzo.WorkMemorySize];
            var compressed = new byte[data.Length + data.Length / 16 + 64 + 3];
            var result = Lzo.TryCompress(compressionMode, data, data.Length, compressed, out var compressedLength, workMemory);

            result.Should().Be(LzoResult.OK);
            compressedLength.Should().BeGreaterThan(0);
            compressed.Should().NotBeNull().And.NotBeEquivalentTo(data);
        }

        private static byte[] GetData()
        {
            var rng = new Random();
            var data = new byte[10 * 1024];
            rng.NextBytes(data);
            return data;
        }
    }
}
