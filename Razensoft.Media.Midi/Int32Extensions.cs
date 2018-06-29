using System;

namespace Razensoft.Media.Midi
{
    internal static class Int32Extensions
    {
        public static int SwapEndianness(this int val)
        {
            var bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}