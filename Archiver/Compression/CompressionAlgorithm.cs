using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public interface ICompressionAlgorithm
    {
        byte[] Compress(byte[] rawBytes);
        byte[] Decompress(byte[] compressedBytes);
    }
}
