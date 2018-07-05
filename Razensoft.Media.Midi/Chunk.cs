using System;
using System.Text;
using JetBrains.Annotations;

namespace Razensoft.Media.Midi
{
    public class Chunk
    {
        internal const int HeaderLength = 4;
        internal static readonly Encoding Encoding = Encoding.ASCII;

        private byte[] header = new byte[HeaderLength];
        private byte[] data = new byte[0];

        [NotNull]
        public byte[] Header
        {
            get => header;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value.Length != HeaderLength)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Header should have the Length of exactly four.");
                header = value;
            }
        }

        [NotNull]
        public string AsciiHeader
        {
            get => Encoding.GetString(Header);
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                Header = Encoding.GetBytes(value);
            }
        }

        [NotNull]
        public byte[] Data
        {
            get => data;
            set => data = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}