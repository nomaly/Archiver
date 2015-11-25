using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public class CCompressedBlocksPackagerOptions
    {
        public ECompressionAlgorithms CompressionAlgorithm { get; set; }


        public CCompressedBlocksPackagerOptions(ECompressionAlgorithms compressionAlgorithm)
        {
            CompressionAlgorithm = compressionAlgorithm;
        }
    }
}
