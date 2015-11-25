using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public class CBlockInfo
    {
        public byte[] RawBytes
        {
            get; protected set;
        }

        public long Length => RawBytes.LongLength;

        public CBlockInfo(byte[] rawBytes)
        {
            CArguments.ThrowIfArgumentNull(rawBytes, "rawBytes");
            RawBytes = rawBytes;
        }
    }

    static class CByteBlocksHelper
    {
        public static CProcessingBlock CreateForProcessingBlockCreateBlock(this CBlockInfo original, int blockNum)
        {
            return CProcessingBlock.CreateBlock(original.RawBytes, blockNum);
        }
    }
}
