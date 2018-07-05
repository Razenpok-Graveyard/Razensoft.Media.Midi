using System.IO;

namespace Razensoft.Media.Midi
{
    internal static class BinaryReaderExtensions
    {
        public static int ReadInt32BigEndian(this BinaryReader reader) => reader.ReadInt32().SwapEndianness();

        public static byte[] ReadBytesChecked(this BinaryReader reader, int length)
        {
            var bytes = reader.ReadBytes(length);
            if (bytes.Length < length)
                throw new EndOfStreamException("Unable to read beyond the end of the stream.");

            return bytes;
        }
    }
}