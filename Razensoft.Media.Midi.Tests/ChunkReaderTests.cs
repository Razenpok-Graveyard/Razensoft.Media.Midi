using System;
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
            chunk.Data.ShouldBe(new byte[] { 0xFF, 0x10, 0x00 });
        }

        [Test]
        [TestCase(new byte[] { 0x4D, 0x54, 0x68, 0x64 }, typeof(HeaderChunk), TestName = "Header Chunk (MThd)")]
        [TestCase(new byte[] { 0x4D, 0x54, 0x72, 0x6B }, typeof(TrackChunk), TestName = "Track Chunk (MTrk)")]
        [TestCase(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF }, typeof(Chunk), TestName = "Unknown Chunk (????)")]
        public void Read_KnownHeaderTypes_ReadAsConcreteTypes(byte[] headerBytes, Type expectedType)
        {
            var chunkBytes = new byte[8];
            Array.Copy(headerBytes, chunkBytes, 4);
            using (var reader = GetChunkReader(chunkBytes))
            {
                var chunk = reader.Read();
                chunk.GetType().ShouldBe(expectedType);
            }
        }

        [Test]
        [TestCase(new byte[0], TestName = "Empty chunk")]
        [TestCase(new byte[]
        {
            0x10
        }, TestName = "Chunk with truncated header")]
        [TestCase(new byte[]
        {
            0x10, 0x01, 0xAA, 0xFF,
            0x00, 0x00
        }, TestName = "Chunk with truncated length")]
        [TestCase(new byte[]
        {
            0x10, 0x01, 0xAA, 0xFF,
            0x00, 0x00, 0x00, 0x03
        }, TestName = "Chunk without data but with declared length")]
        [TestCase(new byte[]
        {
            0x10, 0x01, 0xAA, 0xFF,
            0x00, 0x00, 0x00, 0x03,
            0xFF, 0x10
        }, TestName = "Chunk with truncated data")]
        public void Read_UnfinishedChunk_ExceptionThrown(byte[] chunkBytes)
        {
            using (var reader = GetChunkReader(chunkBytes))
            {
                Should.Throw<EndOfStreamException>(() => reader.Read());
            }
        }

        private static ChunkReader GetChunkReader(byte[] bytes) => new ChunkReader(new MemoryStream(bytes));
    }
}