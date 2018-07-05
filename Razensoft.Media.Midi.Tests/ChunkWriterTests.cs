using System.IO;
using NUnit.Framework;
using Shouldly;

namespace Razensoft.Media.Midi.Tests
{
    [TestFixture]
    public class ChunkWriterTests
    {
        [Test]
        public void Write_NormalChunk_SuccessfulWrite()
        {
            var targetStream = new MemoryStream();
            var chunk = new Chunk
            {
                AsciiHeader = "MTrk",
                Data = new byte[] { 0xFF, 0x10, 0x00 }
            };

            using (var writer = new ChunkWriter(targetStream)) {
                writer.Write(chunk);
            }

            var expectedBytes = new byte[]
            {
                0x4D, 0x54, 0x72, 0x6B,
                0x00, 0x00, 0x00, 0x03,
                0xFF, 0x10, 0x00
            };
            targetStream.ToArray().ShouldBe(expectedBytes);
        }
    }
}