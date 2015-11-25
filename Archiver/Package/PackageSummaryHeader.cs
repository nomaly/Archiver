using System;

namespace Archiver
{
    [Serializable]
    public struct CPackageHeaderInfo
    {
        public Int32 Version;
        public Int64 SummarySectionSize;
    }
}