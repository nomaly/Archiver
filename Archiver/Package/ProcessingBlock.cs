namespace Archiver
{
    public class CProcessingBlock
    {
        public int BlockNum
        {
            get; private set;
        }

        public CBlockInfo BlockInfo { get; }
        public long Length => BlockInfo.Length;

        private CProcessingBlock(CBlockInfo block, int blockNum)
        {
            CArguments.ThrowIfArgumentNull(block, "block");
            CArguments.ThrowIfNot(blockNum >= 0, "blockNum cannot be less than zero");

            BlockInfo = block;
            BlockNum = blockNum;
        }

        public static CProcessingBlock CreateBlock(byte[] rawBytes, int blockNum)
        {
            return new CProcessingBlock(new CBlockInfo(rawBytes), blockNum);
        }
    }
}