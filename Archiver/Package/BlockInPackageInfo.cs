namespace Archiver
{
    public class CBlockInPackageInfo
    {
        private const string NAME_OFFSET = "Offset";
        private const string NAME_LENGTH = "Length";
        private const string NAME_NUM = "Number";

        private CDomContainer m_container;

        public long Offset { get; }
        public long Length { get; }
        public long Number { get; }
            
        public CBlockInPackageInfo(long offset, long length, long number)
        {
            Offset = offset;
            Length = length;
            Number = number;
        }

        public void Serialize(CDomSection section)
        {
            CArguments.ThrowIfArgumentNull(section,"section");

            section.Set(NAME_OFFSET, Offset);
            section.Set(NAME_LENGTH, Length);
        }

        public CBlockInPackageInfo Deserialize(CDomSection section)
        {
            CArguments.ThrowIfArgumentNull(section, "section");

            long offset = section.GetInt64(NAME_OFFSET);
            long length = section.GetInt64(NAME_LENGTH);
            long num = section.GetInt64(NAME_NUM);

            return new CBlockInPackageInfo(offset, length, num);
        }
    }
}