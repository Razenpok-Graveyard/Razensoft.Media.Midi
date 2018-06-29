using System;
using NUnit.Framework;
using Shouldly;

namespace Razensoft.Media.Midi.Tests
{
    [TestFixture]
    public class ChunkTests
    {
        [Test]
        public void Header_LengthNotFour_FailToAssign()
        {
            var chunk = new Chunk();

            var tooShortHeader = new byte[] { 0x00, 0xFF };
            Should.Throw<ArgumentException>(() => chunk.Header = tooShortHeader);

            var tooLongHeader = new byte[] { 0x00, 0x01, 0x10, 0xAA, 0x1F, 0xFF };
            Should.Throw<ArgumentException>(() => chunk.Header = tooLongHeader);
        }

        [Test]
        public void AsciiHeader_LengthNotFour_FailToAssign()
        {
            var chunk = new Chunk();

            const string tooShortHeader = "MT";
            Should.Throw<ArgumentException>(() => chunk.AsciiHeader = tooShortHeader);

            const string tooLongHeader = "MThdMT";
            Should.Throw<ArgumentException>(() => chunk.AsciiHeader = tooLongHeader);
        }
    }
}