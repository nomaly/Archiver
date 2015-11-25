namespace Archiver
{
    public class CPackage
    {
        public CPackageSummaryInfo SummaryInfo { get; private set; }

        public CPackage(CPackageSummaryInfo summaryInfo)
        {
            CArguments.ThrowIfArgumentNull(summaryInfo, "summaryInfo");
            SummaryInfo = summaryInfo;
        }
    }
}