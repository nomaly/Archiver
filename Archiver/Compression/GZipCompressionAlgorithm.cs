using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public class GZipCompressionAlgorithm : ICompressionAlgorithm
    {
        public GZipCompressionAlgorithm()
        {
            
        }

        public byte[] Compress(byte[] rawBytes)
        {
            CArguments.ThrowIfArgumentNull(rawBytes, "rawBytes");

            using (var resultStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(resultStream, CompressionMode.Compress))
                {
                    zipStream.Write(rawBytes, 0, rawBytes.Length);
                    zipStream.Close();

                    return resultStream.ToArray();
                }
            }
        }

        public byte[] Decompress(byte[] compressedBytes)
        {
            CArguments.ThrowIfArgumentNull(compressedBytes, "uncompressedBytes");

            using (var resultStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(compressedBytes))
                {
                    using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(resultStream);
                        zipStream.Close();

                        var result = resultStream.ToArray();

                        if (result.Length == 0)
                            throw new ArgumentException("compressedBytes cannot be decompressed because it was not compressed by GZip");

                        return result;
                    }
                }
            }
        }
    }
}
