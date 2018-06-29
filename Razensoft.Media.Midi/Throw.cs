using System;
using System.IO;
using JetBrains.Annotations;

namespace Razensoft.Media.Midi
{
    internal static class Throw
    {
        [ContractAnnotation("=> halt")]
        internal static void EndOfStreamException()
        {
            throw new EndOfStreamException("Unable to read beyond the end of the stream.");
        }

        [ContractAnnotation("=> halt")]
        internal static void InvalidHeaderLengthException()
        {
            throw new ArgumentException("Header should have the Length of exactly four.");
        }
    }
}