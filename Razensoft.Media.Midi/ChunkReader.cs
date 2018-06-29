using System;
using System.IO;
using JetBrains.Annotations;

namespace Razensoft.Media.Midi
{
    public class ChunkReader : IDisposable
    {
        private readonly BinaryReader reader;

        public ChunkReader([NotNull] Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            reader = new BinaryReader(stream, Chunk.Encoding);
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        [NotNull]
        public Chunk Read()
        {
            var header = reader.ReadBytesChecked(Chunk.HeaderLength);
            var length = reader.ReadInt32BigEndian();
            var data = reader.ReadBytesChecked(length);
            return new Chunk
            {
                Header = header,
                Data = data
            };
        }
    }
}