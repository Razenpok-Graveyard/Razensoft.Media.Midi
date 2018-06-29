using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Shouldly;

namespace Razensoft.Media.Midi.Tests
{
    [TestFixture]
    public class ChunkReaderTests
    {
        [Test]
        public void Read_NormalChunk_SuccessfulRead()
        {
            var chunkBytes = new byte[]
            {
                0x4D, 0x54, 0x68, 0x64,
                0x00, 0x00, 0x00, 0x03,
                0xFF, 0x10, 0x00
            };

            Chunk chunk;
            using (var reader = GetChunkReader(chunkBytes)) {
                chunk = reader.Read();
            }

            chunk.Header.ShouldBe(new byte[] { 0x4D, 0x54, 0x68, 0x64 });
            chunk.AsciiHeader.ShouldBe("MThd");
            chunk.Data.ShouldBe(new List<byte> { 0xFF, 0x10, 0x00 });
        }

        [Test]
        public void Read_KnownHeaderTypes_ReadAsConcreteTypes()
        {
            var headerChunk = new byte[]
            {
                0x4D, 0x54, 0x68, 0x64,
                0x00, 0x00, 0x00, 0x00,
            };
            ShouldReadAsConcreteType<HeaderChunk>(headerChunk);

            var trackChunk = new byte[]
            {
                0x4D, 0x54, 0x72, 0x6B,
                0x00, 0x00, 0x00, 0x00,
            };
            ShouldReadAsConcreteType<TrackChunk>(trackChunk);
        }

        private static void ShouldReadAsConcreteType<T>(byte[] bytes) where T: Chunk
        {
            using (var reader = GetChunkReader(bytes))
            {
                var chunk = reader.Read();
                chunk.GetType().ShouldBe(typeof(T));
            }
        }

        [Test]
        public void Read_UnfinishedChunk_ExceptionThrown()
        {
            var emptyChunk = new byte[0];
            ShouldThrowExceptionWhenReading(emptyChunk);

            var chunkWithTruncatedHeader = new byte[]
            {
                0x10
            };
            ShouldThrowExceptionWhenReading(chunkWithTruncatedHeader);

            var chunkWithTruncatedLength = new byte[]
            {
                0x10, 0x01, 0xAA, 0xFF,
                0x00, 0x00
            };
            ShouldThrowExceptionWhenReading(chunkWithTruncatedLength);

            var chunkWithoutDataButWithDeclaredLength = new byte[]
            {
                0x10, 0x01, 0xAA, 0xFF,
                0x00, 0x00, 0x00, 0x03
            };
            ShouldThrowExceptionWhenReading(chunkWithoutDataButWithDeclaredLength);

            var chunkWithTruncatedData = new byte[]
            {
                0x10, 0x01, 0xAA, 0xFF,
                0x00, 0x00, 0x00, 0x03,
                0xFF, 0x10
            };
            ShouldThrowExceptionWhenReading(chunkWithTruncatedData);
        }

        private static void ShouldThrowExceptionWhenReading(byte[] bytes)
        {
            using (var reader = GetChunkReader(bytes)) {
                Should.Throw<EndOfStreamException>(() => reader.Read());
            }
        }

        private static ChunkReader GetChunkReader(byte[] bytes) => new ChunkReader(new MemoryStream(bytes));
    }
}