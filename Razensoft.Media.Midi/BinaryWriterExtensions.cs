using System.IO;

namespace Razensoft.Media.Midi
{
    internal static class BinaryWriterExtensions
    {
        public static void WriteInt32BigEndian(this BinaryWriter writer, int value)
        {
            writer.Write(value.SwapEndianness());
        }
    }
}