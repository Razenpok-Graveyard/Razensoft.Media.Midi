using System;
using NUnit.Framework;
using Shouldly;

namespace Razensoft.Media.Midi.Tests
{
    [TestFixture]
    public class ChunkTests
    {
        [Test]
        [TestCase(new byte[] { 0x00, 0xFF }, TestName = "Header length < 4")]
        [TestCase(new byte[] { 0x00, 0x01, 0x10, 0xAA, 0x1F, 0xFF }, TestName = "Header length > 4")]
        public void Header_LengthNotFour_FailToAssign(byte[] header)
        {
            var chunk = new Chunk();
            Should.Throw<ArgumentException>(() => chunk.Header = header);
        }

        [Test]
        [TestCase("MT", TestName = "Header length < 4")]
        [TestCase("MThdMT", TestName = "Header length > 4")]
        public void AsciiHeader_LengthNotFour_FailToAssign(string asciiHeader)
        {
            var chunk = new Chunk();
            Should.Throw<ArgumentException>(() => chunk.AsciiHeader = asciiHeader);
        }
    }
}