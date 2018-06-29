using System;
using System.IO;
using JetBrains.Annotations;

namespace Razensoft.Media.Midi
{
    public class ChunkWriter : IDisposable
    {
        private readonly BinaryWriter writer;

        public ChunkWriter([NotNull] Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            writer = new BinaryWriter(stream, Chunk.Encoding);
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void Write([NotNull] Chunk chunk)
        {
            if (chunk == null) throw new ArgumentNullException(nameof(chunk));
            writer.Write(chunk.Header);
            writer.WriteInt32BigEndian(chunk.Data.Length);
            writer.Write(chunk.Data);
        }
    }
}